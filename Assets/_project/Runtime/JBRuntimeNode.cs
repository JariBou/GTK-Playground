using System;
using System.Collections.Generic;

namespace _project.Runtime
{
    [Serializable]
    public abstract class JBRuntimeNode
    {
        // public int NextNodeIndex;
        public List<int> NextNodesIndices = new();
    }
    
    [Serializable]
    public class StartRuntimeNode : JBRuntimeNode {}
    
    [Serializable]
    public class UpdateRuntimeNode : JBRuntimeNode {}
    
    [Serializable]
    public class FloatToStringRuntimeNode : JBRuntimeNode {}

    [Serializable]
    public class BranchRuntimeNode : JBRuntimeNode
    {
        public bool Condition;

        public int CaseFalseNextNodeId;
    }

    [Serializable]
    public class DebugLogRuntimeNode : JBRuntimeNode
    {
        public string Message;
    }
}