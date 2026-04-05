using System;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector
{
    /// <summary>
    /// The serializable data representing a runtime node in the visual novel graph 
    /// that presents two dialogue options to the player.
    /// </summary>
    [Serializable]
    public class TwoOptionRuntimeNode : VisualNovelRuntimeNode
    {
        public string Option1Text;
        public string Option2Text;
        
        /// <summary>
        /// The index of the node to execute if Option 1 is selected.
        /// -1 indicates no continuation for this branch.
        /// </summary>
        public int Option1NextNodeIndex = -1;
        
        /// <summary>
        /// The index of the node to execute if Option 2 is selected.
        /// -1 indicates no continuation for this branch.
        /// </summary>
        public int Option2NextNodeIndex = -1;
        
        /// <summary>
        /// Records which option the player selected (0 for option 1, 1 for option 2).
        /// Set during runtime execution.
        /// </summary>
        public int SelectedOption;
    }
}

