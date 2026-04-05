using System;
using UnityEditor;
using Unity.GraphToolkit.Editor;
using UnityEngine;

[Graph(AssetExtension)]
[Serializable]
public class JBGraph : Graph
{
    public const string AssetExtension = "jbgraph";

    [MenuItem("Assets/Create/JB Graphs/JB Graph", false)]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<JBGraph>("JB Graph");
    }

    public override void OnGraphChanged(GraphLogger graphLogger)
    {
        base.OnGraphChanged(graphLogger);
        
        // foreach (INode node in GetNodes())
        // {
        //     Debug.Log($"===== {node.GetType().Name} =====");
        //     foreach (IPort outputPort in node.GetOutputPorts())
        //     {
        //         bool t = outputPort.dataType == null;
        //         Debug.Log($"\t- {outputPort.dataType}");
        //     }
        //     Debug.Log("===================================");
        // }
        //
        // foreach (IVariable variable in this.GetVariables())
        // {
        //     Debug.Log(variable.name);
        // }
        // TODO: add error checking
    }
}
