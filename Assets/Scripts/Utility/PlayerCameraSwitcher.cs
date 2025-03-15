using UnityEngine;
using Cinemachine;

public class PlayerCameraInventorySwitch : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera; // Kamera utama saat bermain
    public CinemachineVirtualCamera inventoryCamera; // Kamera inventory
    
    private bool isInventoryOpen = false;
    [SerializeField] private bool isInDungeon = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !isInDungeon) // Jika "B" ditekan, buka/tutup inventory
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        
        if (isInventoryOpen)
        {
            // Saat inventory dibuka, aktifkan kamera inventory dan nonaktifkan FreeLook
            freeLookCamera.Priority = 0;
            inventoryCamera.Priority = 10;
        }
        else
        {
            // Saat inventory ditutup, aktifkan kembali FreeLook Camera
            freeLookCamera.Priority = 10;
            inventoryCamera.Priority = 0;
        }
    }
}
