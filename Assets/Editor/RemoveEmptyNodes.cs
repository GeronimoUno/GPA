using UnityEditor;
using UnityEngine;

public class RemoveEmptyNodes : EditorWindow
{
    [MenuItem("Tools/Clean/Remove Empty Nodes")]
    static void RemoveEmpty()
    {
        var all = GameObject.FindObjectsOfType<Transform>();
        int cnt = 0;
        foreach (var t in all)
        {
            // если нет ни одного рендерящего компонента и нет детей
            if (t.GetComponent<MeshFilter>() == null 
             && t.GetComponent<MeshRenderer>() == null
             && t.childCount == 0
             && t.name.StartsWith("Box")) 
            {
                DestroyImmediate(t.gameObject);
                cnt++;
            }
        }
        Debug.Log($"Removed {cnt} empty nodes");
    }
}
