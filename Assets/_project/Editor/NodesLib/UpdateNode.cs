using System;
using Unity.GraphToolkit.Editor;

namespace _project.Editor.NodesLib
{
    [Serializable]
    internal class UpdateNode : JBNode
    {
        public const string DELTA_TIME_PORT_NAME = "DeltaTime";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddDefaultOutputPort(context, "");
            
            context.AddOutputPort<float>(DELTA_TIME_PORT_NAME)
                .WithDisplayName("Delta Time")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
        }   
    }
}