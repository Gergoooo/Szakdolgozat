using UnityEngine;
using UnityEditor;

public class TreePlacer : EditorWindow
{
    public GameObject treePrefab;    // The tree prefab to place
    public Terrain terrain;           // The terrain component directly
    public float minHeight = 25f;     // Minimum height for tree placement
    public float maxHeight = 300f;    // Maximum height for tree placement
    public int numberOfTrees = 10;    // Number of trees to place

    [MenuItem("Tools/Tree Placer")]
    public static void ShowWindow()
    {
        GetWindow<TreePlacer>("Tree Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Place Trees in Terrain", EditorStyles.boldLabel);

        treePrefab = (GameObject)EditorGUILayout.ObjectField("Tree Prefab", treePrefab, typeof(GameObject), true);
        terrain = (Terrain)EditorGUILayout.ObjectField("Terrain", terrain, typeof(Terrain), true);
        minHeight = EditorGUILayout.FloatField("Min Height", minHeight);
        maxHeight = EditorGUILayout.FloatField("Max Height", maxHeight);
        numberOfTrees = EditorGUILayout.IntField("Number of Trees", numberOfTrees); // Input for number of trees

        if (GUILayout.Button("Place Trees"))
        {
            PlaceTrees();
        }
    }

    private void PlaceTrees()
    {
        // Check if the terrain and treePrefab are assigned
        if (terrain == null || treePrefab == null)
        {
            Debug.LogError("Please assign a terrain and tree prefab.");
            return;
        }

        TerrainData terrainData = terrain.terrainData;

        // Place trees
        for (int i = 0; i < numberOfTrees; i++)
        {
            // Generate random x and z positions within the terrain's bounds
            float x = Random.Range(0, terrainData.size.x);
            float z = Random.Range(0, terrainData.size.z);
            float height = terrain.SampleHeight(new Vector3(x, 0, z));

            // Check if the height is within the specified range
            if (height >= minHeight && height <= maxHeight)
            {
                Vector3 position = new Vector3(x + terrain.transform.position.x, height, z + terrain.transform.position.z);
                GameObject treeInstance = (GameObject)PrefabUtility.InstantiatePrefab(treePrefab);
                treeInstance.transform.position = position; // Set the position of the instantiated tree
            }
        }
    }
}
