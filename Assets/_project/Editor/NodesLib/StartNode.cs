using System;
using Unity.GraphToolkit.Editor;

namespace _project.Editor.NodesLib
{
    [Serializable]
    internal class StartNode : JBNode
    {
        protected override void OnDefinePorts(IPortDefinitionContext context)
        {
            AddDefaultOutputPort(context, "");
        }
    }
}