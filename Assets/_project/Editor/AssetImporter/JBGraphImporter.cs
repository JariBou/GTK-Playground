using System;
using System.Collections.Generic;
using System.Linq;
using _project.Editor.NodesLib;
using _project.Runtime;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace _project.Editor.AssetImporter
{
    [ScriptedImporter(1, JBGraph.AssetExtension)]
    public class JBGraphImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            JBGraph graph = GraphDatabase.LoadGraphForImporter<JBGraph>(ctx.assetPath);
            
            if (graph == null)
            {
                Debug.LogError($"Failed to load JB Graph asset: {ctx.assetPath}");
                return;
            }
            
            StartNode startNodeModel = graph.GetNodes().OfType<StartNode>().FirstOrDefault();
            if (startNodeModel == null)
            {
                return;
            }
            
            JBRuntimeGraph runtimeAsset = ScriptableObject.CreateInstance<JBRuntimeGraph>();
            BuildRuntimeGraph(startNodeModel, runtimeAsset);

            // Add the runtime object to the graph asset and set it to be the main asset.
            // This allows the same asset to be used in inspectors wherever a runtime asset is expected.
            // Refer to the BasicVisualNovelCanvas.prefab for an example of this.
            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        private static void BuildRuntimeGraph(INode startNode, JBRuntimeGraph runtimeAsset)
        {
            // Map from editor nodes to their starting index in the runtime nodes list
            var nodeToRuntimeIndex = new Dictionary<INode, int>();
            
            // Queue for breadth-first traversal
            var nodesToProcess = new Queue<INode>();
            
            // Start with the first node after the start node
            INode firstNode = GetNextNode(startNode);
            if (firstNode != null)
            {
                nodesToProcess.Enqueue(firstNode);
            }

            // Process all reachable nodes
            while (nodesToProcess.Count > 0)
            {
                INode currentNode = nodesToProcess.Dequeue();
                
                // Skip if we've already processed this node
                if (nodeToRuntimeIndex.ContainsKey(currentNode))
                    continue;

                // Record the starting index for this node's runtime nodes
                int startIndex = runtimeAsset.Nodes.Count;
                nodeToRuntimeIndex[currentNode] = startIndex;

    
                // Convert the editor node to runtime node(s)
                List<JBRuntimeNode> runtimeNodes = TranslateNodeModelToRuntimeNodes(currentNode);
                runtimeAsset.Nodes.AddRange(runtimeNodes);

                // So... Unity is weird and the TOTALLY VALID FlowExecution RUNTIME type is inaccessible and so this appears to be null
                IEnumerable<IPort> ports = currentNode.GetOutputPorts().Where(port => port.dataType == null);

                foreach (IPort port in ports)
                {
                    INode nextNodeFromPort = GetNextNodeFromPort(port);
                    if (nextNodeFromPort != null)
                    {
                        nodesToProcess.Enqueue(nextNodeFromPort);
                    }
                }
            }

            // Second pass: set up NextNodeIndex references
            SetupNodeReferences(runtimeAsset, nodeToRuntimeIndex, startNode);
        }

        private static INode GetNextNodeFromPort(IPort port)
        {
            IPort nextNodePort = port?.firstConnectedPort;
            return nextNodePort?.GetNode();
        }

        private static INode GetNextNode(INode currentNode)
        {   
            IPort outputPort = currentNode.GetOutputPortByName(JBNode.EXECUTION_PORT_DEFAULT_NAME);
            IPort nextNodePort = outputPort.firstConnectedPort;
            INode nextNode = nextNodePort?.GetNode();

            return nextNode;
        }


        private static List<JBRuntimeNode> TranslateNodeModelToRuntimeNodes(INode nodeModel)
        {
            var returnedNodes = new List<JBRuntimeNode>();
            switch (nodeModel)
            {
                case BranchNode branchNode:
                    returnedNodes.Add(new BranchRuntimeNode()
                    {
                        Condition = GetInputPortValue<bool>(branchNode.GetInputPortByName(BranchNode.IN_CONDITION_PORT_NAME))
                    });
                    break;
                case DebugLogNode debugLogNode:
                    returnedNodes.Add(new DebugLogRuntimeNode()
                    {
                        Message = GetInputPortValue<string>(
                            debugLogNode.GetInputPortByName(DebugLogNode.IN_MESSAGE_PORT_NAME)),
                    });
                    break;
                case StartNode startNode:
                    returnedNodes.Add(new StartRuntimeNode());
                    break;
                case UpdateNode updateNode:
                    returnedNodes.Add(new UpdateRuntimeNode());
                    break;
                case FloatToStringNode node:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nodeModel));
            }

            return returnedNodes;
        }

        /// <summary>
        /// Gets the value of an input port on a node.
        /// <br/><br/>
        /// The value is obtained from (in priority order):<br/>
        /// 1. Connections to the port (variable nodes, constant nodes, wire portals)<br/>
        /// 2. Embedded value on the port<br/>
        /// 3. Default value of the port<br/>
        /// </summary>
        private static T GetInputPortValue<T>(IPort port)
        {
            T value = default;

            // If port is connected to another node, get value from connection
            if (port.isConnected)
            {
                switch (port.firstConnectedPort.GetNode())
                {
                    case IVariableNode variableNode:
                        variableNode.variable.TryGetDefaultValue<T>(out value);
                        return value;
                    case IConstantNode constantNode:
                        constantNode.TryGetValue<T>(out value);
                        return value;
                    default:
                        // Comes here when comming from custom node lmao
                        Debug.Log("Eyaaa");
                        break;
                }
            }
            else
            {
                // If port has embedded value, return it.
                // Otherwise, return the default value of the port
                port.TryGetValue(out value);
            }

            return value;
        }

        private static void SetupNodeReferences(JBRuntimeGraph runtimeAsset, Dictionary<INode, int> nodeToRuntimeIndex, INode startNode)
        {
            var processedNodes = new HashSet<INode>();
            var nodesToProcess = new Queue<INode>();
            
            INode firstNode = GetNextNode(startNode);
            if (firstNode != null)
            {
                nodesToProcess.Enqueue(firstNode);
            }

            while (nodesToProcess.Count > 0)
            {
                INode currentNode = nodesToProcess.Dequeue();
                
                if (!processedNodes.Add(currentNode))
                    continue;

                if (!nodeToRuntimeIndex.TryGetValue(currentNode, out int currentRuntimeIndex))
                    continue;
                
                // We check for flow output
                IPort[] ports = currentNode.GetOutputPorts().Where(port => port.dataType == null).ToArray();

                if (ports.Length > 1)
                {
                    JBRuntimeNode currentRuntimeNode = runtimeAsset.Nodes[currentRuntimeIndex];
                    
                    foreach (IPort port in ports)
                    {
                        INode nextNodeFromPort = GetNextNodeFromPort(port);
                        if (nextNodeFromPort != null)
                        {
                            currentRuntimeNode.NextNodesIndices.Add(nodeToRuntimeIndex.GetValueOrDefault(nextNodeFromPort, -1));
                            nodesToProcess.Enqueue(nextNodeFromPort);
                        }
                        else
                        {
                            currentRuntimeNode.NextNodesIndices.Add(-1); // End of line for this exec
                        }
                    }
                }
                else
                {
                    INode nextNode = GetNextNode(currentNode);
                    
                    // Get all runtime nodes created from this editor node
                    List<JBRuntimeNode> runtimeNodes = TranslateNodeModelToRuntimeNodes(currentNode);
                    for (int i = 0; i < runtimeNodes.Count; i++)
                    {
                        int runtimeNodeIndex = currentRuntimeIndex + i;
                        JBRuntimeNode runtimeNode = runtimeAsset.Nodes[runtimeNodeIndex];
                        
                        // If this is the last runtime node from this editor node
                        if (i == runtimeNodes.Count - 1)
                        {
                            // Point to the next editor node's first runtime node
                            runtimeNode.NextNodesIndices.Add(nextNode != null && nodeToRuntimeIndex.TryGetValue(nextNode, out int nextIdx) ? nextIdx : -1);
                        }
                        else
                        {
                            // Point to the next runtime node in the sequence
                            runtimeNode.NextNodesIndices.Add(runtimeNodeIndex + 1);
                        }
                    }

                    if (nextNode != null)
                        nodesToProcess.Enqueue(nextNode);
                    
                }
            }
        }
    }
}