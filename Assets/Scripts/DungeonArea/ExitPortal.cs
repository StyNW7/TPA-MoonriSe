using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitPortal : MonoBehaviour
{
    public GameObject confirmationPanel;
    public Button confirmButton;
    public Button cancelButton;

    public Transform player; // Player Transform
    public float detectionRadius = 3f; // Radius untuk mendeteksi player

    private bool isPanelVisible = false;
    private bool isCanceled = false; // Flag untuk mencegah panel muncul setelah dibatalkan

    void Start()
    {
        // Pastikan panel konfirmasi tidak terlihat di awal
        confirmationPanel.SetActive(false);

        // Tambahkan listener untuk tombol
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
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
        SceneManager.LoadScene("DungeonScene");
    }

    void OnCancel()
    {
        HideConfirmationPanel();
        isCanceled = true; // Set flag agar panel tidak muncul lagi sampai player keluar radius
    }
}
