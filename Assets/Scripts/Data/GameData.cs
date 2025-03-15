using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{

    // Player Data

    public float playerHealth;
    public float playerMaxHealth;
    public int playerLevel;
    public int playerMoney;
    public int playerExperience;

    public int worldLevel;

    // NPC Data

    public List<SavedStoreItem> ownedItems;
    public List<SavedResourceItem> ownedResources;

    // Farm Data

}
