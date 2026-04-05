using System;
using Unity.GraphToolkit.Editor;
using UnityEngine;

namespace _project.Editor.NodesLib
{
    [Serializable]
    internal class BranchNode : JBNode
    {
        public const string IN_CONDITION_PORT_NAME = "Condition";

        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddDefaultInputPort(context);

            context.AddInputPort<bool>(IN_CONDITION_PORT_NAME)
                .WithDisplayName("Condition")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();

            context.AddOutputPort("True")
                .WithDisplayName("True")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();

            context.AddOutputPort("False")
                .WithDisplayName("False")
                .WithConnectorUI(PortConnectorUI.Arrowhead)
                .Build();
        }
    }
}