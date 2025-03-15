using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
public class PlayerInventory : MonoBehaviour
{
    [Header("Player Equipment & Resources")]
    public List<StoreItem> ownedEquipment = new List<StoreItem>();
    public List<ResourceItem> ownedResources = new List<ResourceItem>();
    private string equippedWeapon = "";

    public static PlayerInventory Instance;

    [SerializeField] private NPCInteraction npcItem;
    [SerializeField] private NPCInteraction2 npcResource;

    [Header("UI Elements")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject equipmentTab;
    [SerializeField] private GameObject resourceTab;
    [SerializeField] private Button equipmentButton;
    [SerializeField] private Button resourceButton;
    [SerializeField] private Button[] equipmentButtons;
    [SerializeField] private Button[] resourceButtons;
    [SerializeField] private TMP_Text[] resourceQuantityText;

    [SerializeField] private PlayerManager playerManager;

    // Camera

    public Camera mainCamera;
    public CinemachineVirtualCamera inventoryCamera;

    public float transitionSpeed = 3f;
    public Transform inventoryCameraPosition;
    public Transform mainCameraPosition;

    // Equipment Manager

    public EquipmentManager equipmentManager;
    public CameraSwitcher cameraSwitcher;

    public ThirdPersonController tpc;
    public PlayerInput playerInput;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        inventoryPanel.SetActive(false); // Pastikan inventory tertutup saat awal game
        ShowEquipmentTab(); // Default buka tab Equipment

        equipmentButton.onClick.AddListener(ShowEquipmentTab);
        resourceButton.onClick.AddListener(ShowResourceTab);

        RefreshInventory();
        //tpc = FindObjectOfType<ThirdPersonController>();
        //inventoryCamera.enabled = false;

        tpc = FindObjectOfType<ThirdPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            //Debug.Log("Tombol B ditekan.");
            ToggleInventory();
        }
    }


    public void SwitchInventoryCamera(bool open)
    {

        if (open == true)
        {
            //mainCamera.transform.position = Vector3.Lerp(
            //    mainCamera.transform.position, inventoryCameraPosition.position, Time.deltaTime * transitionSpeed);
            //mainCamera.transform.rotation = Quaternion.Slerp(
            //    mainCamera.transform.rotation, inventoryCameraPosition.rotation, Time.deltaTime * transitionSpeed);
            mainCamera.transform.position = inventoryCameraPosition.position;
            //mainCamera.transform.rotation = inventoryCameraPosition.rotation;
            mainCamera.transform.rotation = Quaternion.Euler(
                mainCamera.transform.rotation.eulerAngles.x, // Tetap menggunakan sudut X yang ada
                180f, // Mengubah sudut Y menjadi 180 derajat
                mainCamera.transform.rotation.eulerAngles.z  // Tetap menggunakan sudut Z yang ada
            );
            GetComponent<ThirdPersonController>().enabled = false;
        }
        else
        {
            //mainCamera.transform.position = Vector3.Lerp(
            //    mainCamera.transform.position, mainCameraPosition.position, Time.deltaTime * transitionSpeed);
            //mainCamera.transform.rotation = Quaternion.Slerp(
            //    mainCamera.transform.rotation, mainCameraPosition.rotation, Time.deltaTime * transitionSpeed);
            mainCamera.transform.position = mainCameraPosition.position;
            //mainCamera.transform.rotation = mainCameraPosition.rotation;
            mainCamera.transform.rotation = Quaternion.Euler(
                mainCamera.transform.rotation.eulerAngles.x, // Tetap menggunakan sudut X yang ada
                0f, // Mengubah sudut Y menjadi 180 derajat
                mainCamera.transform.rotation.eulerAngles.z  // Tetap menggunakan sudut Z yang ada
            );
            GetComponent<ThirdPersonController>().enabled = true;
        }

    }


    public void ToggleInventory()
    {
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            // tpc.ChangeInventoryCamera(true);
            // SwitchInventoryCamera(true);
            // cameraSwitcher.SwitchInventoryCamera(true);
            if (tpc != null) tpc.enabled = false;
            if (playerInput != null) playerInput.DeactivateInput();
        }

        else
        {
            // SwitchInventoryCamera(false);
            // cameraSwitcher.SwitchInventoryCamera(false);
            // tpc.ChangeInventoryCamera(false);
            if (tpc != null) tpc.enabled = true;
            if (playerInput != null) playerInput.ActivateInput();
        }

        Debug.Log("Inventory Panel " + (isActive ? "Dibuka" : "Ditutup"));

        if (isActive)
        {
            RefreshInventory();
        }
    }

    // === SWITCH TAB LOGIC ===
    public void ShowEquipmentTab()
    {
        equipmentTab.SetActive(true);
        resourceTab.SetActive(false);
        ResetButtonColor(resourceButton);
        SetButtonDark(equipmentButton);
    }

    public void ShowResourceTab()
    {
        equipmentTab.SetActive(false);
        resourceTab.SetActive(true);
        ResetButtonColor(equipmentButton);
        SetButtonDark(resourceButton);
    }

    // === EQUIPMENT LOGIC ===
    public void EquipWeapon(string weaponName)
    {

        if (weaponName == "Sword" && ownedEquipment[0].isOwned == true)
        {
            equippedWeapon = weaponName;
            equipmentManager.EquipItem(weaponName);
        }

        else if (weaponName == "Fishing Rod" && ownedEquipment[1].isOwned == true)
        {
            equippedWeapon = weaponName;
            equipmentManager.EquipItem(weaponName);
        }

        else if (weaponName == "Bow" && ownedEquipment[2].isOwned == true)
        {
            equippedWeapon = weaponName;
            equipmentManager.EquipItem(weaponName);
        }

        else if (weaponName == "Axe")
        {
            equippedWeapon = weaponName;
            equipmentManager.EquipItem(weaponName);
        }
        else
        {
            Debug.Log("Test");
            equippedWeapon = weaponName;
            equipmentManager.EquipItem(weaponName);
        }

        RefreshInventory();

    }

    public string GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public bool IsWeaponEquipped(string weaponName)
    {
        return equippedWeapon == weaponName;
    }

    // === RESOURCE LOGIC ===
    public void ConsumeResource(string resourceName)
    {
        ResourceItem resource = ownedResources.Find(x => x.itemName == resourceName);
        if (resource != null && resource.ownedQuantity > 0)
        {
            int healthGain = GetHealthValue(resourceName);
            playerManager.PlayerGetHealth(healthGain);

            npcResource.ReduceResource(resourceName, 1);

            RefreshInventory();
        }
    }

    private int GetHealthValue(string resourceName)
    {
        switch (resourceName)
        {
            case "Tomato": return 5;
            case "Berries": return 10;
            case "Bamboo": return 15;
            case "Fish": return 7;
            default: return 0;
        }
    }

    // === REFRESH UI ===
    private void RefreshInventory()
    {

        ownedEquipment = npcItem.GetAllItems();
        ownedResources = npcResource.GetAllResources();

        // Refresh Equipment UI
        for (int i = 0; i < 4; i++)
        {
            int buttonIndex = i; // Salin nilai i ke variabel lokal

            if (buttonIndex < ownedEquipment.Count && ownedEquipment[buttonIndex].isOwned)
            {
                equipmentButtons[buttonIndex].interactable = true;
                equipmentButtons[buttonIndex].onClick.RemoveAllListeners();
                equipmentButtons[buttonIndex].onClick.AddListener(() => EquipWeapon(ownedEquipment[buttonIndex].itemName));
                equipmentButtons[buttonIndex].gameObject.transform.parent.gameObject.SetActive(true);
            }
            // Ini jika item ga ada
            else if (buttonIndex < ownedEquipment.Count && !ownedEquipment[buttonIndex].isOwned)
            {
                equipmentButtons[buttonIndex].interactable = false;
                equipmentButtons[buttonIndex].onClick.RemoveAllListeners();
                equipmentButtons[buttonIndex].gameObject.transform.parent.gameObject.SetActive(false);
            }
            // Ini khusus Axe pasti ada
            else
            {
                equipmentButtons[buttonIndex].gameObject.SetActive(true);
                equipmentButtons[buttonIndex].interactable = true;
                equipmentButtons[buttonIndex].onClick.RemoveAllListeners();
                equipmentButtons[buttonIndex].onClick.AddListener(() => EquipWeapon("Axe"));
            }
        }

        // Refresh Resource UI
        for (int i = 0; i < resourceButtons.Length; i++)
        {
            Debug.Log("Masuk refresh UI Resources");
            ResetButtonColor(resourceButtons[i]);
            if (i < ownedResources.Count)
            {
                Debug.Log("Resources Mau diliat di inventory");
                resourceButtons[i].interactable = true;
                resourceQuantityText[i].text = ownedResources[i].ownedQuantity.ToString();

                string resourceName = ownedResources[i].itemName;
                resourceButtons[i].onClick.RemoveAllListeners();
                if (i == 1 || i == 2 || i == 3 || i == 7)
                {
                    resourceButtons[i].onClick.AddListener(() => ConsumeResource(resourceName));
                }
                if (ownedResources[i].ownedQuantity <= 0)
                {
                    Debug.Log("Resources 0");
                    SetButtonDark(resourceButtons[i]);
                    resourceButtons[i].interactable = false;
                }
                else ResetButtonColor(resourceButtons[i]);
                if (ownedResources[i].ownedQuantity <= 0) resourceButtons[i].gameObject.transform.parent.gameObject.SetActive(false);
                else resourceButtons[i].gameObject.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                //resourceButtons[i].gameObject.SetActive(false);
                resourceButtons[i].interactable = false;
            }
        }
    }

    void SetButtonDark(Button button)
    {
        // Mengubah warna tombol menjadi gelap jika item dimiliki
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = Color.grey;  // Warna gelap
        button.colors = colorBlock;
    }

    void ResetButtonColor(Button button)
    {
        // Mengembalikan warna tombol ke default (terang)
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = Color.white;  // Warna normal
        button.colors = colorBlock;
    }

}
