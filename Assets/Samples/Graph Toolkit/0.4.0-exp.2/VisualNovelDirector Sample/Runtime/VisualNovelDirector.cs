using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// The main class that controls the visual novel direction.
    /// <br/><br/>
    /// This class is a <see cref="MonoBehaviour"/> intended to be attached to a specific hierarchy of GameObjects.
    /// In this sample we can see it being used in the `BasicVisualNovelCanvas` prefab. This is how it gets all the
    /// necessary components to run the visual novel, including the scene references, global settings, input handling
    /// and the runtime graph to execute.
    /// <br/><br/>
    /// When running (in PlayMode or in a build) this class executes the runtime visual novel graph, one node at a time,
    /// using their respective <see cref="IVisualNovelNodeExecutor{TNode}"/>. The execution supports both linear
    /// narrative flow and branching paths.
    /// </summary>
    public class VisualNovelDirector : MonoBehaviour
    {
        [Header("Graph")]
        // The runtime graph to execute. Note that the runtime graph asset is the same as the authoring graph asset
        // because we export the runtime asset object into the same asset and set it as the 'main' asset in our importer.
        // This allows us to edit the authoring graph in the editor and drag-drop the same asset into inspector fields
        // that expect the runtime graph.
        public VisualNovelRuntimeGraph RuntimeGraph;

        [Header("Scene References")]
        public Image BackgroundImage;
        public List<Image> ActorLocationList;
        public GameObject DialoguePanel;
        public TextMeshProUGUI DialogueText;
        public TextMeshProUGUI ActorNameText;
        public GameObject OptionPanel;
        public TextMeshProUGUI Option1Text;
        public TextMeshProUGUI Option2Text;

        [Header("Settings")]
        public float GlobalFadeDuration = 0.5f;
        public float GlobalTextDelayPerCharacter = 0.03f;

        [Header("Input")]
        public MonoBehaviour InputComponent;
        public IVisualNovelInputProvider InputProvider => InputComponent as IVisualNovelInputProvider;

        private async void Start()
        {
            // Create each executor once
            var setBackgroundExecutor = new SetBackgroundExecutor();
            var setDialogueExecutor = new SetDialogueExecutor();
            var waitForInputExecutor = new WaitForInputExecutor();
            var twoOptionExecutor = new TwoOptionExecutor();

            // Execute nodes following the graph structure, supporting branching paths
            int currentNodeIndex = 0;
            
            while (currentNodeIndex >= 0 && currentNodeIndex < RuntimeGraph.Nodes.Count)
            {
                var node = RuntimeGraph.Nodes[currentNodeIndex];
                int nextNodeIndex = -1;

                switch (node)
                {
                    case SetBackgroundRuntimeNode bgNode:
                        await setBackgroundExecutor.ExecuteAsync(bgNode, this);
                        nextNodeIndex = bgNode.NextNodeIndex;
                        break;
                        
                    case SetDialogueRuntimeNode dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        nextNodeIndex = dialogueNode.NextNodeIndex;
                        break;
                        
                    case SetDialogueRuntimeNodeWithPreviousActor dialogueNode:
                        await setDialogueExecutor.ExecuteAsync(dialogueNode, this);
                        nextNodeIndex = dialogueNode.NextNodeIndex;
                        break;
                        
                    case WaitForInputRuntimeNode waitNode:
                        await waitForInputExecutor.ExecuteAsync(waitNode, this);
                        nextNodeIndex = waitNode.NextNodeIndex;
                        break;
                        
                    case TwoOptionRuntimeNode twoOptionNode:
                        await twoOptionExecutor.ExecuteAsync(twoOptionNode, this);
                        
                        // Follow the branch based on the player's choice
                        nextNodeIndex = twoOptionNode.SelectedOption == 0 
                            ? twoOptionNode.Option1NextNodeIndex 
                            : twoOptionNode.Option2NextNodeIndex;
                        break;
                        
                    default:
                        Debug.LogError($"No executor found for node type: {node.GetType()}");
                        return;
                }

                currentNodeIndex = nextNodeIndex;
            }
        }
    }
}
