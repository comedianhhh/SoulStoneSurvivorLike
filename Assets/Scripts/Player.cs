using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Manages player-related data, specifically handling various currencies and their operations.
/// </summary>
public class Player : MonoBehaviour, IPlayerVisitor
{
    #region Structs
    /// <summary>
    /// Represents an event when the Soulstone currency is updated.
    /// </summary>
    public struct SoulstoneUpdatedEvent
    {
        public SoulstoneCache UpdatedStones { get; private set; }

        public SoulstoneUpdatedEvent(SoulstoneCache updatedStones)
        {
            UpdatedStones = updatedStones;
        }
    }

    /// <summary>
    /// Struct to hold Soulstone currency data and quantity.
    /// </summary>
    [Serializable]
    public struct SoulstoneCache
    {
        public string uniqueID; 
        public int quantity;
        public StonesDataSO soulstoneData; 
    }

    #endregion

    #region Variables
    private string SaveFilePath => Path.Combine(Application.persistentDataPath, "playerSave.json");

    [SerializeField]
    private List<SoulstoneCache> stones = new List<SoulstoneCache>();
    private Dictionary<string, StonesDataSO> stonesDataMap = new Dictionary<string, StonesDataSO>();
    #endregion

    #region Unity Methods
    private void Awake()
    {
        // Assuming all StonesDataSO objects are located in a Resources folder
        var allStonesData = Resources.LoadAll<StonesDataSO>("Currencies");
        foreach (var stoneData in allStonesData)
        {
            if (!stonesDataMap.ContainsKey(stoneData.uniqueID))
            {
                stonesDataMap.Add(stoneData.uniqueID, stoneData);
            }
        }
    }
    private void Start()
    {
        LoadPlayerData();
    }

    private void OnApplicationQuit()
    {
        SavePlayerData();
    }
    #endregion

    #region Currency Management
    /// <summary>
    /// Updates the amount of a specific Soulstone currency.
    /// </summary>
    /// <param name="soulstone">The Soulstone to update.</param>
    /// <param name="amount">The amount to add or subtract.</param>
    public void UpdateCurrencyAmount(StonesDataSO soulstone, int amount)
    {
        SoulstoneCache soulstoneCache = stones.Find(c => c.soulstoneData == soulstone);
        if (soulstoneCache.soulstoneData != null)
        {
            soulstoneCache.quantity += amount;
            int index = stones.FindIndex(c => c.soulstoneData == soulstone);
            if (index != -1) stones[index] = soulstoneCache;

            EventBus.Instance.Publish(new SoulstoneUpdatedEvent(stones[index]));
        }
    }

    /// <summary>
    /// Retrieves the quantity of a specific Soulstone currency.
    /// </summary>
    /// <param name="soulstone">The Soulstone to retrieve.</param>
    /// <returns>The quantity of the specified Soulstone.</returns>
    public int GetCurrencyAmount(StonesDataSO soulstone)
    {
        SoulstoneCache soulstoneCache = stones.Find(c => c.soulstoneData == soulstone);
        if (soulstoneCache.soulstoneData != null)
        {
            return soulstoneCache.quantity;
        }
        return 0;
    }

    /// <summary>
    /// Retrieves the current list of Soulstone currencies the player has.
    /// </summary>
    /// <returns>A list of <see cref="SoulstoneCache"/> representing the player's current Soulstone currencies.</returns>
    public List<SoulstoneCache> GetStones()
    {
        GetStonesUniqueID();
        return stones;
    }

    #endregion

    #region Persistence
    /// <summary>
    /// Saves player data, including Soulstone currencies, to a persistent file.
    /// </summary>
    public void SavePlayerData()
    {
        GetStonesUniqueID();
        PlayerData data = new PlayerData { stones = stones };
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Player data saved at: {SaveFilePath}");
    }

    private void GetStonesUniqueID()
    {
        for (int i = 0; i < stones.Count; i++)
        {
            SoulstoneCache stone = stones[i];
            if(stone.soulstoneData != null)
            {
                stone.uniqueID = stone.soulstoneData.uniqueID;
                stones[i] = stone;
            }
        }
    }

    /// <summary>
    /// Loads player data, including Soulstone currencies, from a persistent file. Initializes default cache if the file doesn't exist.
    /// </summary>
    public void LoadPlayerData()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // Relink StonesDataSO based on uniqueID
            for (int i = 0; i < stones.Count; i++)
            {
                SoulstoneCache stone = stones[i];
                if (stonesDataMap.TryGetValue(stone.uniqueID, out StonesDataSO soulstoneData))
                {
                    stone.soulstoneData = soulstoneData; // Re-link the correct StonesDataSO instance
                    stones[i] = stone;
                }
            }
            stones = data.stones;
        }
        else
        {
            InitializeDefaultCache();
        }
    }

    /// <summary>
    /// Initializes default Soulstone currencies for the player.
    /// </summary>
    private void InitializeDefaultCache()
    {
        // Initialize default Soulstone cache
    }

    [Serializable]
    public class PlayerData
    {
        public List<SoulstoneCache> stones;
    }
    #endregion

    // Additional methods for gameplay logic...
    public void Visit(SoulstoneCollectible soulstone)
    {
        // Example method to update player currency
        UpdateCurrencyAmount(soulstone.soulstoneData, soulstone.quantity);
        // Assume the collectible handles its own destruction or deactivation
    }

    public void Visit(PowerUpCollectible powerUp)
    {
        // Apply power-up effect
    }

    // Method to get the visitor instance (this Player)
    public IPlayerVisitor GetVisitor()
    {
        return this;
    }
}
