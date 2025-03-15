using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

public class NPCInteraction : MonoBehaviour, NPCInterface
{
    [Header("NPC Info")]
    public string npcName;
    public string npcTitle;
    public string npcInteractName;

    [Header("UI Elements")]
    public TMP_Text interactionText;
    public GameObject interactionPanel;
    public GameObject storePanel;
    public Button closeButton;

    [Header("Player Detection")]
    public GameObject playerBody;
    public float interactionRange = 3f;

    private bool isPlayerNearby = false;
    public bool isStorePanelOpen = false;

    [Header("Store Items")]
    [SerializeField]
    public List<StoreItem> storeItems = new List<StoreItem>();

    [Header("Item Detail UI")]
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;
    public TMP_Text itemPriceText;
    public RawImage itemImage;
    public Button buyButton;

    [Header("Item Buttons")]
    public Button buttonItem1;
    public Button buttonItem2;
    public Button buttonItem3;

    private StoreItem activeItem; // Item yang sedang ditampilkan

    public PlayerManager playerManager;
    public LootSpawner lootSpawner;

    public ErrorPanel errorPanel;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    public CinemachineFreeLook freeLookCamera;
    public FreeLookCamera freeLookCameraScript;

    void Start()
    {
        closeButton.onClick.AddListener(() => CloseStore());

        // Menyambungkan tombol dengan item yang sesuai
        buttonItem1.onClick.AddListener(() => UpdateItemDetail(0));
        buttonItem2.onClick.AddListener(() => UpdateItemDetail(1));
        buttonItem3.onClick.AddListener(() => UpdateItemDetail(2));

        buyButton.onClick.AddListener(() => BuyItem());

        interactionPanel.SetActive(false);
        storePanel.SetActive(false);

        freeLookCameraScript = FindObjectOfType<FreeLookCamera>();
        playerController = FindObjectOfType<ThirdPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();
        freeLookCamera = GameObject.FindGameObjectWithTag("FreeLookCamera").GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerBody.transform.position);
        isPlayerNearby = distance <= interactionRange;

        if (isPlayerNearby)
        {
            interactionPanel.SetActive(true);
            if (isStorePanelOpen) interactionPanel.SetActive(false);
            interactionText.text = npcInteractName;

            if (Input.GetKeyDown(KeyCode.F))
            {
                isStorePanelOpen = !isStorePanelOpen;
                if (isStorePanelOpen) OpenStore();
                else CloseStore();
            }
        }
        else
        {
            interactionPanel.SetActive(false);
            isStorePanelOpen = false;
        }
    }

    public void OpenStore()
    {
        storePanel.SetActive(true);
        interactionPanel.SetActive(false);
        // playerBody.SetActive(false);
        // Time.timeScale = 0f;

        // Default: Tampilkan item pertama saat toko dibuka
        UpdateItemDetail(0);
        // lootSpawner.SpawnLoot(transform.position);
        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.DeactivateInput();
        freeLookCamera.m_XAxis.m_InputAxisName = "";
        freeLookCamera.m_YAxis.m_InputAxisName = "";
        freeLookCamera.m_XAxis.m_InputAxisValue = 0f;
freeLookCamera.m_YAxis.m_InputAxisValue = 0f;
freeLookCameraScript.LockCamera(true);
    }

    public void CloseStore()
    {
        storePanel.SetActive(false);
        // playerBody.SetActive(true);
        // Time.timeScale = 1f;
        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.ActivateInput();
        freeLookCamera.m_XAxis.m_InputAxisName = "Mouse X";
        freeLookCamera.m_YAxis.m_InputAxisName = "Mouse Y";
        freeLookCameraScript.LockCamera(false);
        playerManager.GainXP(5);
    }

    void UpdateItemDetail(int index)
    {
        if (index < 0 || index >= storeItems.Count) return;

        activeItem = storeItems[index];

        itemNameText.text = activeItem.itemName;
        itemDescriptionText.text = activeItem.itemDescription;

        // Cek apakah pemain memiliki cukup uang dan tampilkan harga sesuai
        if (playerManager.GetMoney() >= activeItem.price)
        {
            itemPriceText.text = "$" + activeItem.price.ToString();
            itemPriceText.color = Color.yellow;  // Uang cukup, teks warna kuning
        }
        else
        {
            itemPriceText.text = "$" + activeItem.price.ToString();
            itemPriceText.color = Color.red;  // Uang tidak cukup, teks warna merah
        }

        // Pastikan ada gambar item jika tersedia
        if (activeItem.itemSprite != null)
            itemImage.texture = activeItem.itemSprite;

        // Ubah tampilan tombol beli berdasarkan kepemilikan item
        if (activeItem.isOwned)
        {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Owned";  // Ubah tombol menjadi Owned
            buyButton.interactable = false;
            if (activeItem == storeItems[0])  // Jika item pertama yang dibeli
            {
                SetButtonDark(buttonItem1);
            }
            else if (activeItem == storeItems[1])  // Jika item kedua yang dibeli
            {
                SetButtonDark(buttonItem2);
            }
            else if (activeItem == storeItems[2])  // Jika item ketiga yang dibeli
            {
                SetButtonDark(buttonItem3);
            }
        }
        else
        {
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Buy";  // Tombol beli
            buyButton.interactable = true;
            if (activeItem == storeItems[0])  // Jika item pertama yang dibeli
            {
                ResetButtonColor(buttonItem1);
            }
            else if (activeItem == storeItems[1])  // Jika item kedua yang dibeli
            {
                ResetButtonColor(buttonItem2);
            }
            else if (activeItem == storeItems[2])  // Jika item ketiga yang dibeli
            {
                ResetButtonColor(buttonItem3);
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

    public void BuyItem()
    {
        if (activeItem == null) return;

        if (!activeItem.isOwned && playerManager.GetMoney() >= activeItem.price)
        {
            Debug.Log("Membeli: " + activeItem.itemName);
            activeItem.isOwned = true;

            // Deduct money
            playerManager.DeductMoney(activeItem.price);

            // Perbarui tampilan tombol beli setelah membeli
            buyButton.GetComponentInChildren<TextMeshProUGUI>().text = "Owned";
            buyButton.interactable = false;

            if (activeItem == storeItems[0])  // Jika item pertama yang dibeli
            {
                SetButtonDark(buttonItem1);
            }
            else if (activeItem == storeItems[1])  // Jika item kedua yang dibeli
            {
                SetButtonDark(buttonItem2);
            }
            else if (activeItem == storeItems[2])  // Jika item ketiga yang dibeli
            {
                SetButtonDark(buttonItem3);
            }
        }
        else if (playerManager.GetMoney() < activeItem.price)
        {
            Vector3 mousePos = Input.mousePosition;
            errorPanel.ShowError("You Don't Have The Money", mousePos);
        }
    }

    // Class untuk StoreItem
    //[System.Serializable]
    //public class StoreItem
    //{
    //    public string itemName;
    //    public string itemDescription;
    //    public int price;
    //    public bool isOwned;
    //    public Sprite itemSprite;
    //}

    public List<StoreItem> GetOwnedItems()
    {
        return storeItems.FindAll(item => item.isOwned);
    }

    public List<StoreItem> GetAllItems()
    {
        return storeItems;
    }

    // Setter: Update status kepemilikan item
    public void UpdateItemOwnership(string itemName, bool isOwned)
    {
        StoreItem item = storeItems.Find(x => x.itemName == itemName);
        if (item != null)
        {
            item.isOwned = isOwned;
            if (item.itemName == "Sword")SetButtonDark(buttonItem1);
            if (item.itemName == "Fishing Rod")SetButtonDark(buttonItem2);
            if (item.itemName == "Bow")SetButtonDark(buttonItem3);
        }
    }

    public void ResetItemOwnership()
    {
        foreach (StoreItem storeItem in storeItems)
        {
            storeItem.isOwned = false;
        }
        ResetButtonColor(buttonItem1);
        ResetButtonColor(buttonItem2);
        ResetButtonColor(buttonItem3);
    }

    public void GetItemOwnership()
    {
        foreach (StoreItem storeItem in storeItems)
        {
            storeItem.isOwned = true;
        }
        SetButtonDark(buttonItem1);
        SetButtonDark(buttonItem2);
        SetButtonDark(buttonItem3);
    }

    public void SetAllItems(List<StoreItem> saveItems)
    {
        storeItems = saveItems;
    }

    public void SetSavedItems(List<SavedStoreItem> savedItems)
    {
        foreach (SavedStoreItem savedItem in savedItems)
        {
            StoreItem existingItem = storeItems.Find(x => x.itemName == savedItem.itemName);
            if (existingItem != null)
            {
                existingItem.isOwned = savedItem.isOwned;
            }
        }
    }


}
