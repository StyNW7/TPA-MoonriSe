using System.Collections;
using UnityEngine;
using TMPro;

public class DungeonAreaLoad : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private NPCInteraction npc1;
    [SerializeField] private NPCInteraction2 npc2;

    [SerializeField] private TMP_Text worldLevelTxt;

    private PlayerBar playerBar;

    private void Awake()
    {
        Debug.Log("DungeonAreaLoad Awakened!");

        // Cari ulang referensi untuk memastikan semuanya di-load ulang
        playerManager = FindObjectOfType<PlayerManager>();
        npc1 = FindObjectOfType<NPCInteraction>();
        npc2 = FindObjectOfType<NPCInteraction2>();
        playerBar = FindObjectOfType<PlayerBar>();

        // Cari worldLevelTxt setelah scene di-load ulang
        if (worldLevelTxt == null)
        {
            worldLevelTxt = GameObject.Find("WorldLevelText")?.GetComponent<TMP_Text>();
        }
    }

    private void Start()
    {
        Debug.Log("DungeonAreaLoad Start Running...");
        StartCoroutine(WaitAndLoadData());
    }

    private IEnumerator WaitAndLoadData()
    {
        yield return new WaitForSeconds(0.1f);

        // Pastikan game tidak load data jika pertama kali dimainkan
        if (!DataManager.Instance.IsFirstLoad())
        {
            Debug.Log("Loading player data...");
            LoadPlayerData();
        }
        else
        {
            Debug.Log("Skipping load data, first time playing.");
        }
        // LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        GameData data = DataManager.Instance.LoadGame();
        if (data != null)
        {
            Debug.Log($"Data Loaded! Player Money: {data.playerMoney}, World Level: {data.worldLevel}");

            playerManager.SetHealth(data.playerHealth);
            playerManager.SetMaxHealth(data.playerMaxHealth);
            playerManager.SetXP(data.playerExperience);
            playerManager.SetMoney(data.playerMoney);
            playerManager.SetLevel(data.playerLevel);

            // ✅ Pastikan worldLevel tidak reset
            playerManager.SetWorldLevel(data.worldLevel);

            // ✅ Update tampilan worldLevelTxt
            if (worldLevelTxt != null)
            {
                Debug.Log("Updating World Level UI: " + data.worldLevel);
                worldLevelTxt.text = data.worldLevel.ToString();
            }
            else
            {
                Debug.LogWarning("worldLevelTxt not found!");
            }

            // ✅ Update item dan resource NPC
            npc1.SetSavedItems(data.ownedItems);
            npc2.SetSavedResources(data.ownedResources);

            // ✅ Update tampilan player health bar
            if (playerBar != null)
            {
                playerBar.InitializeBars(data.playerMaxHealth, playerManager.GetNextLevelXP());
                playerBar.UpdateHealth(data.playerHealth);
            }
            else
            {
                Debug.LogWarning("playerBar reference missing!");
            }
        }
        else
        {
            Debug.LogWarning("No saved data found!");
        }
    }
}
