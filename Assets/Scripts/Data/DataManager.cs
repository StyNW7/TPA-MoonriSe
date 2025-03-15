using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    private string saveFilePath;
    private bool isFirstLoad = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        saveFilePath = Application.persistentDataPath + "/GameData.json";

        if (!PlayerPrefs.HasKey("HasPlayedBefore"))
        {
            isFirstLoad = true;
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            PlayerPrefs.Save();
        }
        else
        {
            isFirstLoad = false;
        }
    }

    public bool IsFirstLoad()
    {
        return isFirstLoad;
    }

    public void SaveGame(float playerHealth, float playerMaxHealth2, int playerLevel, int playerMoney, int playerExperience, int worldLevel, List<StoreItem> storeItems, List<ResourceItem> resourceItems)
    {
        List<SavedStoreItem> savedStoreItems = new List<SavedStoreItem>();
        foreach (StoreItem item in storeItems)
        {
            savedStoreItems.Add(new SavedStoreItem(item.itemName, item.isOwned));
        }

        List<SavedResourceItem> savedResourceItems = new List<SavedResourceItem>();
        foreach (ResourceItem resource in resourceItems)
        {
            savedResourceItems.Add(new SavedResourceItem(resource.itemName, resource.ownedQuantity));
        }

        GameData data = new GameData
        {
            playerHealth = playerHealth,
            playerMaxHealth = playerMaxHealth2,
            playerLevel = playerLevel,
            playerMoney = playerMoney,
            playerExperience = playerExperience,
            worldLevel = worldLevel,
            ownedItems = savedStoreItems,
            ownedResources = savedResourceItems
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved: " + json);
    }

    public GameData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("Game Loaded: " + json);
            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
}
