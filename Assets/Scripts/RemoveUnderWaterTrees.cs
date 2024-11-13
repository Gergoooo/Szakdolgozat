using UnityEngine;
using System.Collections.Generic;

public class RemoveUnderwaterTrees : MonoBehaviour
{
    public Terrain terrain; // Reference to the terrain
    private TreeInstance[] backupTreeInstances; // Backup for the original tree instances
    private List<TreeInstance> newTreeInstances; // List to hold trees within height range

    // Enum to define actions
    public enum TreeActions
    {
        BackupCurrentTrees,
        RestoreBackupTrees,
        RemoveOutsideHeightRange
    }

    public TreeActions performAction;

    // Context menu to modify tree data
    [ContextMenu("Modify Tree Data")]
    private void ModifyTreeData()
    {
        if (terrain == null)
        {
            terrain = Terrain.activeTerrain; // Use the active terrain if none specified
        }

        switch (performAction)
        {
            case TreeActions.BackupCurrentTrees:
                backupTreeInstances = terrain.terrainData.treeInstances;
                Debug.Log("Current trees have been stored in backup data.");
                break;

            case TreeActions.RestoreBackupTrees:
                if (backupTreeInstances != null)
                {
                    terrain.terrainData.treeInstances = backupTreeInstances;
                    Debug.Log("Trees have been restored from the backup data.");
                }
                else
                {
                    Debug.Log("NO backup data FOUND ....");
                }
                break;

            case TreeActions.RemoveOutsideHeightRange:
                Debug.Log("Removing trees outside the height range ....");
                Vector3 terrainSize = terrain.terrainData.size;
                TreeInstance[] treeInstances = terrain.terrainData.treeInstances;
                Debug.Log("Old: Total Trees = " + treeInstances.Length);

                newTreeInstances = new List<TreeInstance>();
                float minHeight = 25f; // Minimum height
                float maxHeight = 300f; // Maximum height

                // Check each tree
                foreach (var tree in treeInstances)
                {
                    float treeHeight = tree.position.y * terrainSize.y; // Calculate tree height in world space
                    if (treeHeight >= minHeight && treeHeight <= maxHeight)
                    {
                        newTreeInstances.Add(tree); // Keep the tree if within height range
                    }
                }

                // Apply new list of tree instances
                terrain.terrainData.treeInstances = newTreeInstances.ToArray();
                Debug.Log("New: Total Trees = " + terrain.terrainData.treeInstances.Length);
                break;
        }
    }
}
