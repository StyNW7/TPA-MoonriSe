using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class CameraSwitcher : MonoBehaviour
{

    public Camera mainCamera;
    public Transform inventoryCameraPosition;
    public Transform mainCameraPosition;
    public float transitionSpeed = 3f;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    private bool isSwitching = false; // Cegah perpindahan berulang
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;

    void Start()
    {
        playerController = GetComponent<ThirdPersonController>();
        playerInput = GetComponent<PlayerInput>();

        // Simpan posisi dan rotasi awal kamera
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;
    }

    public void SwitchInventoryCamera(bool open)
    {

        if (isSwitching) return; // Mencegah perpindahan berulang saat animasi berjalan

        if (open)
        {
            // Simpan posisi & rotasi terakhir sebelum berpindah
            originalCameraPosition = mainCamera.transform.position;
            originalCameraRotation = mainCamera.transform.rotation;

            // Matikan pergerakan karakter & input
            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.DeactivateInput();

            //inventoryCameraPosition.position = mainCamera.transform.position + mainCamera.transform.forward * 2f;

            // Mulai animasi kamera ke posisi inventory
            StartCoroutine(SmoothCameraTransition(inventoryCameraPosition.position, Quaternion.Euler(
                mainCamera.transform.rotation.eulerAngles.x, // Biarkan X sama
                180f, // Rotasi Y menjadi 180 derajat
                mainCamera.transform.rotation.eulerAngles.z // Biarkan Z sama
            )));
        }

        else
        {
            // Aktifkan kembali pergerakan karakter & input
            if (playerController != null) playerController.enabled = true;
            if (playerInput != null) playerInput.ActivateInput();

            // Mulai animasi kamera kembali ke posisi awal
            StartCoroutine(SmoothCameraTransition(originalCameraPosition, originalCameraRotation));
        }

    }

    private IEnumerator SmoothCameraTransition(Vector3 targetPosition, Quaternion targetRotation)
    {
        isSwitching = true; // Menandakan animasi sedang berjalan
        float elapsedTime = 0f;
        Vector3 startPosition = mainCamera.transform.position;
        Quaternion startRotation = mainCamera.transform.rotation;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * transitionSpeed;
            mainCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            mainCamera.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime);
            yield return null;
        }

        // Pastikan posisi & rotasi sudah sesuai target di akhir animasi
        mainCamera.transform.position = targetPosition;
        mainCamera.transform.rotation = targetRotation;

        isSwitching = false; // Animasi selesai
    }

}
