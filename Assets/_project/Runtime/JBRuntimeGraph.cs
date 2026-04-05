using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _project.Runtime
{
    public class JBRuntimeGraph : ScriptableObject
    {
        [SerializeReference] public List<JBRuntimeNode> Nodes = new();

        public JBRuntimeNode GetStartNode()
        {
            return Nodes.FirstOrDefault(node => node.GetType() == typeof(StartRuntimeNode));
        }
        
        public JBRuntimeNode GetUpdateNode()
        {
            return Nodes.FirstOrDefault(node => node.GetType() == typeof(UpdateRuntimeNode));
        }

        public int GetNodeIndex(JBRuntimeNode targetNode)
        {
            return Nodes.IndexOf(targetNode);
        }
    }
}