using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInitialization : MonoBehaviour
{
    [System.Serializable]
    private struct RangeByLevel
    {
        public DungeonLevelSO dungeonLevel;
        [Range(0, 100)] public int min;
        [Range(0, 100)] public int max;
    }
    
    
    #region Tooltip
    [Tooltip("The minimum number of items to spawn (note that a maximum of 1 of each type of ammo, health, and weapon will be spawned")]
    #endregion Tooltip
    [SerializeField] [Range(0, 2)] private int numberOfItemsToSpawnMin;
    #region Tooltip
    [Tooltip("The maximum number of items to spawn (note that a maximum of 1 of each type of ammo, health, and weapon will be spawned")]
    #endregion Tooltip
    [SerializeField] [Range(0, 2)] private int numberOfItemsToSpawnMax;


    #region Tooltip
    [Tooltip("The range of health to spawn for each level")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> healthSpawnByLevelList;
    #region Tooltip
    [Tooltip("The range of ammo to spawn for each level")]
    #endregion Tooltip
    [SerializeField] private List<RangeByLevel> ammoSpawnByLevelList;

    void Start()
    {
        SetResource();
    }

    private void SetResource()
    {
        // Get Number Of Ammo,Health, & Weapon Items To Spawn (max 1 of each)
        GetItemsToSpawn(out int ammoNum, out int healthNum);
        
        foreach (ResourcePoint resourcePoint in GetComponentsInChildren<ResourcePoint>())
        {
            resourcePoint.Initialize(GetHealthPercentToSpawn(healthNum), GetAmmoPercentToSpawn(ammoNum));
        }
    }
    
    /// <summary>
    /// Get ammo percent to spawn
    /// </summary>
    private int GetAmmoPercentToSpawn(int ammoNumber)
    {
        if (ammoNumber == 0) return 0;

        // Get ammo spawn percent range for level
        foreach (RangeByLevel spawnPercentByLevel in ammoSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }

    /// <summary>
    /// Get health percent to spawn
    /// </summary>
    private int GetHealthPercentToSpawn(int healthNumber)
    {
        if (healthNumber == 0) return 0;

        // Get ammo spawn percent range for level
        foreach (RangeByLevel spawnPercentByLevel in healthSpawnByLevelList)
        {
            if (spawnPercentByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
            {
                return Random.Range(spawnPercentByLevel.min, spawnPercentByLevel.max);
            }
        }

        return 0;
    }
    
    private void GetItemsToSpawn(out int ammo, out int health)
    {
        ammo = 0;
        health = 0;

        int numberOfItemsToSpawn = Random.Range(numberOfItemsToSpawnMin, numberOfItemsToSpawnMax + 1);

        int choice;

        if (numberOfItemsToSpawn == 1)
        {
            choice = Random.Range(0, 2);
            if (choice == 0) { ammo++; return; }
            if (choice == 1) { health++; return; }
            return;
        }
        else if (numberOfItemsToSpawn >= 2)
        {
            ammo++;
            health++;
        }
    }
}
