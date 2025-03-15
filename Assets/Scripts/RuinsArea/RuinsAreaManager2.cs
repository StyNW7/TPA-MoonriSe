using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RuinsAreaManager2 : MonoBehaviour
{
    public GameObject confirmationPanel;
    public Button confirmButton;
    public Button cancelButton;

    public Transform player; // Player Transform
    public float detectionRadius = 3f; // Radius untuk mendeteksi player

    private bool isPanelVisible = false;
    private bool isCanceled = false; // Flag untuk mencegah panel muncul setelah dibatalkan

    // Untuk save game data

    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private NPCInteraction npc1;
    [SerializeField] private NPCInteraction2 npc2;

    void Start()
    {
        // Pastikan panel konfirmasi tidak terlihat di awal
        confirmationPanel.SetActive(false);

        // Tambahkan listener untuk tombol
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);

        if (DataManager.Instance == null)
        {
            Debug.LogWarning("DataManager missing! Creating new instance.");
            GameObject dataManager = new GameObject("DataManager");
            dataManager.AddComponent<DataManager>();
        }
    }

    void Update()
    {
        // Hitung jarak player ke portal
        float distance = Vector3.Distance(player.position, transform.position);

        // Jika dalam radius dan tidak dibatalkan, tampilkan panel
        if (distance <= detectionRadius && !isPanelVisible && !isCanceled)
        {
            ShowConfirmationPanel();
        }
        // Jika player keluar dari radius, reset isCanceled agar panel bisa muncul lagi saat masuk radius
        else if (distance > detectionRadius && isCanceled)
        {
            isCanceled = false;
        }

        // Tekan tombol F untuk konfirmasi
        if (isPanelVisible && Input.GetKeyDown(KeyCode.F)) OnConfirm();
    }

    void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        Time.timeScale = 0f; // Hentikan waktu
        isPanelVisible = true;
    }

    void HideConfirmationPanel()
    {
        Time.timeScale = 1f; // Waktu kembali normal
        confirmationPanel.SetActive(false);
        isPanelVisible = false;
    }

    void OnConfirm()
    {

        Debug.Log("Go to dungeon scene");
        Time.timeScale = 1f; // Pastikan waktu kembali normal sebelum pindah scene

        // Save data dulu disini


        // float playerHealth = playerManager.GetHealth();
        // int playerLevel = playerManager.GetLevel();
        // int playerMoney = playerManager.GetMoney();
        // int playerExperience = playerManager.GetXP();

        npc1.ResetItemOwnership();
        npc2.ResetResource();

        List<StoreItem> ownedItems = npc1.GetAllItems();
        List<ResourceItem> ownedResources = npc2.GetAllResources();
        int worldLevel = playerManager.GetWorldLevel();
        worldLevel += 1;
        playerManager.SetWorldLevel(worldLevel);

        // if (DataManager != null)
            DataManager.Instance.SaveGame(150, 150, 1, 100,
                0, worldLevel, ownedItems, ownedResources);

        SceneManager.LoadScene("MainMenu");

    }

    void OnCancel()
    {
        HideConfirmationPanel();
        isCanceled = true; // Set flag agar panel tidak muncul lagi sampai player keluar radius
    }
}
