using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace _project.Runtime
{
    public class JBGraphExecutionner : MonoBehaviour
    {
        [Header("Graph"), SerializeField]
        private JBRuntimeGraph _runtimeGraph;
        
        [Header("Runtime Options")]
        [SerializeField] private bool _doStart;
        [SerializeField] private bool _doUpdate;

        private Dictionary<Type, object> _executors;
        private JBRuntimeNode _currentNode;
        private Awaitable _startTask;
        private Awaitable _updateTask;

        void Awake() {
            _executors = new Dictionary<Type, object> {
                { typeof(StartRuntimeNode), new StartNodeExecutor() },
                { typeof(UpdateRuntimeNode), new UpdateNodeExecutor() },
                { typeof(BranchRuntimeNode), new BranchNodeExecutor() },
                { typeof(DebugLogRuntimeNode), new DebugLogNodeExecutor() },
            };

            // TODO: Should work, actually nope, doesn't work
            // object executor = executors[typeof(StartRuntimeNode)];
            // var jbGraphNodeExecutor = (IJBGraphNodeExecutor<JBRuntimeNode>)executor;
            // StartRuntimeNode startRuntimeNode = new StartRuntimeNode();
            // jbGraphNodeExecutor.ExecuteAsync(startRuntimeNode);
        }
        
        void Start() {
            if (_runtimeGraph == null) {
                Debug.LogError("No runtime graph assigned!");
                return;
            }
            
            _currentNode = _runtimeGraph.GetStartNode();

            if (_doStart)
            {
                _startTask = Run();
            }
        }

        private async void Update()
        {
            if (_doUpdate)
            {
                await Run(_runtimeGraph.GetUpdateNode(), new RuntimeExecutionContext(){DeltaTime = Time.deltaTime});
                // if (_updateTask.IsCompleted)
                // {
                //     _updateTask = await Run();
                // }
            }
            
        }

        public async Awaitable Run()
        {
            await Run(_runtimeGraph.GetStartNode());
        }
        public async Awaitable Run(JBRuntimeNode startingNode, RuntimeExecutionContext ctx = null)
        {
            int currentNodeIndex = _runtimeGraph.GetNodeIndex(startingNode);
            
            while (currentNodeIndex >= 0 && currentNodeIndex < _runtimeGraph.Nodes.Count)
            {
                var node = _runtimeGraph.Nodes[currentNodeIndex];

                _executors.TryGetValue(node.GetType(), out object executorObj);
                // IJBGraphNodeExecutor<JBRuntimeNode> executor = (IJBGraphNodeExecutor<JBRuntimeNode>)executorObj;
                // if (executor != null)
                if (executorObj is IJBGraphNodeExecutor executor)
                {
                    currentNodeIndex = await executor.ExecuteAsync(node, ctx ?? new RuntimeExecutionContext());
                }
                else
                {
                    currentNodeIndex = -1;
                }
            }
        }
    }
}