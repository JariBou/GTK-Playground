using System;

namespace _project.Editor.NodesLib
{
    [Serializable]
    internal class FloatToStringNode : JBNode 
    {
        public const string IN_FLOAT_VALUE_PORT_NAME = "FloatValue";
        public const string OUT_STRING_VALUE_PORT_NAME = "StringValue";
        
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            context.AddInputPort<float>(IN_FLOAT_VALUE_PORT_NAME)
                .WithDisplayName("Float").Build();
            
            context.AddOutputPort<string>(OUT_STRING_VALUE_PORT_NAME)
                .WithDisplayName("String").Build();
        }
    }
}