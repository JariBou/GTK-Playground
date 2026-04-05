using System;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// The base class for the runtime data for all visual novel nodes.
    /// </summary>
    [Serializable]
    public abstract class VisualNovelRuntimeNode
    {
        /// <summary>
        /// The index of the next node to execute in the runtime graph.
        /// -1 indicates no next node (end of execution).
        /// For branching nodes, this represents the default path.
        /// </summary>
        public int NextNodeIndex = -1;
    }
}
