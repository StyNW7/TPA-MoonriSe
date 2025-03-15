using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using StarterAssets;
using UnityEngine.InputSystem;
using Cinemachine;

public class NPCInteraction2 : MonoBehaviour, NPCInterface
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

    [Header("Resources Items")]
    public List<ResourceItem> storeItems = new List<ResourceItem>();
    public Button[] buttonItems;
    public TMP_Text[] resourceOwnedText;

    [Header("Resource UI")]

    public TMP_Text itemNameText;
    public RawImage itemImage;
    public TMP_Text resourceQuantityInput;
    public Button addButton;
    public Button lessButton;
    public Button buyButton;
    public Button sellButton;
    public TMP_Text totalPriceText;

    private ResourceItem activeItem;
    private int activeItemIndex = -1;
    public PlayerManager playerManager;
    public ErrorPanel errorPanel;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    public CinemachineFreeLook freeLookCamera;

    public FreeLookCamera freeLookCameraScript;

    void Start()
    {
        closeButton.onClick.AddListener(CloseStore);

        for (int i = 0; i < buttonItems.Length; i++)
        {
            int index = i;
            buttonItems[i].onClick.AddListener(() => SelectItem(index));
        }

        addButton.onClick.AddListener(() => ChangeQuantity(1));
        lessButton.onClick.AddListener(() => ChangeQuantity(-1));
        buyButton.onClick.AddListener(BuyItem);
        sellButton.onClick.AddListener(SellItem);

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
        SelectItem(0);
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
    }

    void SelectItem(int index)
    {
        if (index < 0 || index >= storeItems.Count) return;

        activeItem = storeItems[index];
        activeItemIndex = index;

        itemNameText.text = activeItem.itemName;

        // Pastikan ada gambar item jika tersedia
        if (activeItem.itemSprite != null)
            itemImage.texture = activeItem.itemSprite;

        resourceQuantityInput.text = "1";

        UpdateTotalPrice();
    }

    void ChangeQuantity(int change)
    {
        if (activeItem == null) return;

        int currentQuantity = int.Parse(resourceQuantityInput.text);
        int newQuantity = Mathf.Max(1, currentQuantity + change);
        resourceQuantityInput.text = newQuantity.ToString();

        UpdateTotalPrice();
    }

    void UpdateTotalPrice()
    {

        if (activeItem == null) return;

        int quantity = int.Parse(resourceQuantityInput.text);
        int totalPrice = activeItem.price * quantity;
        totalPriceText.text = "Total: $" + totalPrice.ToString();

        // Cek apakah pemain memiliki cukup uang dan tampilkan harga sesuai
        if (playerManager.GetMoney() >= totalPrice)
        {
            //totalPriceText.text = "$" + activeItem.price.ToString();
            totalPriceText.color = Color.yellow;  // Uang cukup, teks warna kuning
        }
        else
        {
            //totalPriceText.text = "$" + activeItem.price.ToString();
            totalPriceText.color = Color.red;  // Uang tidak cukup, teks warna merah
        }

    }

    public void BuyItem()
    {
        if (activeItem == null) return;

        int quantityToBuy = int.Parse(resourceQuantityInput.text);

        if (activeItem.price * quantityToBuy > playerManager.GetMoney())
        {
            Vector3 mousePos = Input.mousePosition;
            errorPanel.ShowError("You don't have enough money", mousePos);
            return;
        }

        activeItem.ownedQuantity += quantityToBuy;
        playerManager.DeductMoney(activeItem.price * quantityToBuy);
        resourceOwnedText[activeItemIndex].text = activeItem.ownedQuantity.ToString();
        SelectItem(activeItemIndex);
    }

    void SellItem()
    {
        if (activeItem == null) return;

        int quantityToSell = int.Parse(resourceQuantityInput.text);

        if (activeItem.ownedQuantity < quantityToSell)
        {
            Vector3 mousePos = Input.mousePosition;
            errorPanel.ShowError("You don't have enough resources", mousePos);
            return;
        }

        activeItem.ownedQuantity -= quantityToSell;
        playerManager.AddMoney(activeItem.price * quantityToSell);
        resourceOwnedText[activeItemIndex].text = activeItem.ownedQuantity.ToString();
        playerManager.GainXP(10);
        SelectItem(activeItemIndex);
    }

    //[System.Serializable]
    //public class ResourceItem
    //{
    //    public string itemName;
    //    public int price;
    //    public int ownedQuantity;
    //    public Sprite itemSprite;
    //}

    
    public void UpdateShopText(){

        int index = 0;
        
        foreach (TMP_Text itemText in resourceOwnedText){
            itemText.text = storeItems[index++].ownedQuantity.ToString();
        }

    }

    public List<ResourceItem> GetAllResources()
    {
        return storeItems;
    }

    public void SetAllResources(List<ResourceItem> newResources)
    {
        storeItems = newResources;
        UpdateShopText();
    }

    public void ReduceResource(string itemName, int quantity)
    {
        int index = 0;
        foreach (var resource in storeItems)
        {
            Debug.Log(resource.itemName);
            if (resource.itemName == itemName)
            {
                resource.ownedQuantity = Mathf.Max(0, resource.ownedQuantity - quantity);
                resourceOwnedText[index].text = resource.ownedQuantity.ToString();
                return;
            }
            index++;
        }
    }

    public void AddResource(string itemName, int quantity)
    {
        int index = 0;
        foreach (var resource in storeItems)
        {
            if (resource.itemName == itemName)
            {
                resource.ownedQuantity = Mathf.Max(0, resource.ownedQuantity + quantity);
                resourceOwnedText[index].text = resource.ownedQuantity.ToString();
                return;
            }
            index++;
        }
    }

    public void ResetResource()
    {
        int index = 0;
        foreach (ResourceItem resource in storeItems)
        {
            resource.ownedQuantity = 0;
            resourceOwnedText[index].text = resource.ownedQuantity.ToString();
            index++;
        }
        // UpdateShopText();
    }

    public void GetAllResource(int quantity)
    {
        int index = 0;
        foreach (ResourceItem resource in storeItems)
        {
            resource.ownedQuantity = Mathf.Max(0, resource.ownedQuantity + quantity);
            resourceOwnedText[index].text = resource.ownedQuantity.ToString();
            index++;
        }
    }

    public bool GetSaplings(string itemName)
    {
        
        foreach (ResourceItem resource in storeItems)
        {
            if (resource.itemName == itemName)
            {
                if (resource.ownedQuantity == 0) return false;
                else if (resource.ownedQuantity > 0) return true;
            }
        }

        return false;

    }

    public bool HasEnoughWood(int woodNeeded)
    {
        ResourceItem woodResource = storeItems.Find(item => item.itemName == "Wood");

        if (woodResource != null)
        {
            return woodResource.ownedQuantity >= woodNeeded;
        }

        return false; // Jika tidak ada wood sama sekali
    }


    public bool HasSaplings(string itemName)
    {
        foreach (ResourceItem resource in storeItems)
        {
            if (resource.itemName == itemName)
            {
                return resource.ownedQuantity > 0;
            }
        }
        return false;
    }

    public void SetSavedResources(List<SavedResourceItem> savedResources)
    {
        foreach (SavedResourceItem savedResource in savedResources)
        {
            ResourceItem existingResource = storeItems.Find(x => x.itemName == savedResource.itemName);
            if (existingResource != null)
            {
                existingResource.ownedQuantity = savedResource.quantity;
            }
        }
        UpdateShopText();
    }


}
