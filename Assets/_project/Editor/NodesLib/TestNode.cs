using System;
using System.Collections.Generic;
using Unity.GraphToolkit.Editor;

namespace _project.Editor.NodesLib
{
    [Serializable]
    public class TEstClass
    {
        public string Name;
        public int health;
        public List<string> aList;
    }
    
    [Serializable]
    internal class TestNode : JBNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<List<string>>("Value")
                .WithDisplayName("Value")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
            
            context.AddInputPort<int>("Value2")
                .WithDisplayName("Value2")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();
            
            context.AddInputPort<TEstClass>("Value3")
                .WithDisplayName("Value3")
                .WithConnectorUI(PortConnectorUI.Circle)
                .Build();

            context.AddOutputPort<bool>("eha").Build();
        }
    }
}