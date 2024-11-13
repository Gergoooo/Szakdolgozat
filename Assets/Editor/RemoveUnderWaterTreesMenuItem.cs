using UnityEngine;
using UnityEditor;

public class RemoveUnderwaterTreesMenuItem
{
    [MenuItem("Terrain/Remove Underwater Trees")]
    public static void NewRemoveUnderwaterTrees()
    {
        GameObject go = new GameObject("RemoveUnderwaterTrees");
        go.transform.position = Vector3.zero;
        go.AddComponent<RemoveUnderwaterTrees>(); // Attach the script to the new GameObject
    }
}
