using System;
using Unity.GraphToolkit.Editor;

namespace _project.Editor.NodesLib
{
    [Serializable]
    internal class DebugLogNode : JBNode
    {
        public const string IN_MESSAGE_PORT_NAME = "Message";
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            //TODO: Create attribute in Runtime node and Editornodes will be like JBNode<JBRuntimeNode> and we create ports from that!
            AddInputOutputExecutionPorts(context);

            context.AddInputPort<string>(IN_MESSAGE_PORT_NAME)
                .WithDisplayName("Message")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }
    }
}