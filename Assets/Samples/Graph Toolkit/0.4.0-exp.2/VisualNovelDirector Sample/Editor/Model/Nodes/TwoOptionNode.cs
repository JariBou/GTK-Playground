using System;
using Unity.GraphToolkit.Editor;

namespace Unity.GraphToolkit.Samples.VisualNovelDirector.Editor
{
    /// <summary>
    /// Represents a Two Dialogue Option Node in the Visual Novel Director tool.
    /// </summary>
    /// <remarks>
    /// This node presents two dialogue options to the player and branches the narrative
    /// based on their choice. Is converted to a <see cref="TwoOptionRuntimeNode"/> for the runtime.
    /// </remarks>
    [Serializable]
    internal class TwoOptionNode : VisualNovelNode
    {
        public const string IN_PORT_OPTION1_NAME = "Option1";
        public const string IN_PORT_OPTION2_NAME = "Option2";
        public const string OUT_PORT_OPTION1_NAME = "Option1Execution";
        public const string OUT_PORT_OPTION2_NAME = "Option2Execution";

        /// <summary>
        /// Defines the ports for the node.
        /// </summary>
        /// <param name="context">The scope to define the node.</param>
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            // Input execution port
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(string.Empty)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddInputPort<string>(IN_PORT_OPTION1_NAME)
                .WithDisplayName("Option 1")
                .Build();

            context.AddInputPort<string>(IN_PORT_OPTION2_NAME)
                .WithDisplayName("Option 2")
                .Build();

            // Two output execution ports for branching
            context.AddOutputPort(OUT_PORT_OPTION1_NAME)
                .WithDisplayName("Option 1")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                //.AsVertical()
                .Build();

            context.AddOutputPort(OUT_PORT_OPTION2_NAME)
                .WithDisplayName("Option 2")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                //.AsVertical()
                .Build();
        }
    }
}

