using UnityEngine;
using System.Collections;
using Cinemachine;
using UnityEngine.InputSystem;
using StarterAssets;

public class FishingArea : MonoBehaviour
{
    public GameObject fishingPanel;  // UI Fishing Mini-Game
    public GameObject interactionPanel; // UI "Press F to Fish"
    public Transform fishingCameraPosition; // Posisi kamera saat memancing
    public Transform normalCameraPosition;
    public CinemachineFreeLook freeLookCamera; // Kamera utama saat bermain
    public CinemachineVirtualCamera fishingCamera; // Kamera inventory
    public Animator playerAnimator;
    public FishingMiniGame miniGame;

    public Transform player; // Referensi ke posisi player
    public float fishingDistance = 3.0f; // Jarak maksimal agar bisa memancing

    private bool isFishing = false;
    private bool isFishingInProgress = false; // Mencegah input berulang saat animasi berjalan


    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    public PlayerManager playerManager;


    void Start()
    {
        fishingPanel.SetActive(false);
        interactionPanel.SetActive(false);

        playerController = FindObjectOfType<ThirdPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerManager = FindObjectOfType<PlayerManager>();

    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= fishingDistance && !isFishing &&
                EquipmentManager.Instance.GetEquipmentWeapon() == "Fishing Rod") // Cek jika belum memancing
        {
            interactionPanel.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F) && !isFishingInProgress) // Cegah spam input saat animasi berjalan
            {
                StartCoroutine(StartFishing());
            }
        }
        else
        {
            interactionPanel.SetActive(false);
        }
    }

    IEnumerator StartFishing()
    {
        isFishingInProgress = true; // Mencegah input tambahan selama animasi berjalan
        interactionPanel.SetActive(false); // Sembunyikan UI "Press F to Fish"
        playerAnimator.SetBool("isFishing", true); // Jalankan animasi memancing

        yield return new WaitForSeconds(2.0f); // Tunggu animasi selesai sebelum ganti kamera

        isFishing = true;
        // mainCamera.transform.position = fishingCameraPosition.position;
        // mainCamera.transform.rotation = fishingCameraPosition.rotation;

        freeLookCamera.Priority = 0;
        fishingCamera.Priority = 10;

        fishingPanel.SetActive(true);
        miniGame.StartFishingGame(); // Memulai mini-game

        isFishingInProgress = false;
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.DeactivateInput();
    }

    public void EndFishing()
    {
        isFishing = false;
        fishingPanel.SetActive(false);
        interactionPanel.SetActive(false); // Pastikan UI tetap tersembunyi sampai pemain selesai

        playerAnimator.SetBool("isFishing", false);
        // mainCamera.transform.position = normalCameraPosition.position;
        // mainCamera.transform.rotation = normalCameraPosition.rotation;

        freeLookCamera.Priority = 0;
        fishingCamera.Priority = 10;

        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.ActivateInput();
        playerManager.GainXP(10);

    }

}
