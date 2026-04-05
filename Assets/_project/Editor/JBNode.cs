using System;
using Unity.GraphToolkit.Editor;

namespace _project.Editor
{
    [Serializable]
    internal abstract class JBNode : Node
    {
        public const string EXECUTION_PORT_DEFAULT_NAME = "ExecutionPort";

        /// <summary>
        /// Defines common input and output execution ports for all nodes in the Visual Novel Director tool.
        /// </summary>
        /// <param name="scope">The scope to define the node.</param>
        protected void AddInputOutputExecutionPorts(IPortDefinitionContext context)
        {
            AddDefaultInputPort(context);

            AddDefaultOutputPort(context);
        }

        protected static void AddDefaultOutputPort(IPortDefinitionContext context, string displayName = "To")
        {
            context.AddOutputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }

        protected static void AddDefaultInputPort(IPortDefinitionContext context, string displayName = "From")
        {
            context.AddInputPort(EXECUTION_PORT_DEFAULT_NAME)
                .WithDisplayName(displayName)
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}