using System.Threading.Tasks;
using NUnit.Framework.Interfaces;
using UnityEngine;

namespace _project.Runtime
{
    // TODO: Attribute and then we can retrieve this and use reflection to create a dictionnary of (NodeType, ExecutionerType)
    // and like save it to a file that has a static method that adds it to the executor

    public interface IJBGraphNodeExecutor
    {
        public Task<int> ExecuteAsync(JBRuntimeNode node, RuntimeExecutionContext ctx);
    }
    
    public interface IJBGraphNodeExecutor<in TNode> : IJBGraphNodeExecutor where TNode : JBRuntimeNode
    {
        Task<int> ExecuteAsync(TNode node, RuntimeExecutionContext ctx); // TODO: add  context
    }

    public abstract class ExecutorBase<TNode> : IJBGraphNodeExecutor<TNode> where TNode : JBRuntimeNode
    {
        public virtual Task<int> ExecuteAsync(TNode node, RuntimeExecutionContext ctx)
        {
            return Task.FromResult(node.NextNodesIndices[0]);
        }
        
        public Task<int> ExecuteAsync(JBRuntimeNode node, RuntimeExecutionContext ctx)
        {
            return ExecuteAsync(node as TNode, ctx);
        }
    }

    public class StartNodeExecutor : ExecutorBase<StartRuntimeNode>
    {
    }
    
    public class UpdateNodeExecutor : ExecutorBase<UpdateRuntimeNode>
    {
    }
    
    public class BranchNodeExecutor : ExecutorBase<BranchRuntimeNode>
    {
        public override Task<int> ExecuteAsync(BranchRuntimeNode node, RuntimeExecutionContext ctx)
        {
            Debug.Log($"BranchRuntimeNode Called: Condition evaluated to '{node.Condition}'");
            
            return Task.FromResult(node.NextNodesIndices[node.Condition ? 0 : 1]);
        }
    }
    
    public class DebugLogNodeExecutor : ExecutorBase<DebugLogRuntimeNode>
    {
        public override Task<int> ExecuteAsync(DebugLogRuntimeNode node, RuntimeExecutionContext ctx)
        {
            Debug.Log($"DebugLogRuntimeNode Called : {node.Message}");
            return Task.FromResult(node.NextNodesIndices[0]);
        }
    }
    
    public class FloatToStringNodeExecutor : ExecutorBase<FloatToStringRuntimeNode>
    {
        public override Task<int> ExecuteAsync(FloatToStringRuntimeNode node, RuntimeExecutionContext ctx)
        {
            
            return Task.FromResult(node.NextNodesIndices[0]);
        }
    }
}