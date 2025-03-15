//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class InventoryManager : MonoBehaviour
//{
//    public static InventoryManager Instance;

//    public GameObject inventoryPanel;
//    public GameObject equipmentTab, resourceTab;
//    public Button equipmentButton, resourceButton;
//    public Transform equipmentContainer, resourceContainer;
//    public GameObject itemPrefab;
//    public Camera mainCamera;
//    public Camera inventoryCamera;
//    //public Transform frontViewPosition;

//    private bool isOpen = false;
//    private Dictionary<string, InventoryItem> inventory = new Dictionary<string, InventoryItem>();

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//    }

//    void Start()
//    {
//        inventoryPanel.SetActive(false);
//        equipmentButton.onClick.AddListener(() => SwitchTab(true));
//        resourceButton.onClick.AddListener(() => SwitchTab(false));
//        inventoryCamera.gameObject.SetActive(false);
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.B))
//        {
//            ToggleInventory();
//        }
//    }

//    IEnumerator SwitchCamera(Camera from, Camera to, float duration)
//    {
//        float elapsedTime = 0f;
//        Vector3 startPosition = from.transform.position;
//        Quaternion startRotation = from.transform.rotation;

//        while (elapsedTime < duration)
//        {
//            to.transform.position = Vector3.Lerp(startPosition, to.transform.position, elapsedTime / duration);
//            to.transform.rotation = Quaternion.Lerp(startRotation, to.transform.rotation, elapsedTime / duration);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        from.enabled = false;
//        to.enabled = true;
//    }

//    void ToggleInventory()
//    {
//        isOpen = !isOpen;
//        inventoryPanel.SetActive(isOpen);

//        if (isOpen)
//        {
//            mainCamera.enabled = false;
//            inventoryCamera.enabled = true;
//            //StartCoroutine(SwitchCamera(mainCamera, inventoryCamera, 1f));
//            Time.timeScale = 0f;
//        }
//        else
//        {
//            mainCamera.enabled = true;
//            inventoryCamera.enabled = false;
//            //StartCoroutine(SwitchCamera(inventoryCamera, mainCamera, 1f));
//            Time.timeScale = 1f;
//        }

//        //if (isOpen)
//        //{
//        //    mainCamera.gameObject.SetActive(false);  // Matikan kamera utama
//        //    inventoryCamera.gameObject.SetActive(true);  // Aktifkan kamera inventory
//        //    Time.timeScale = 0f;
//        //}
//        //else
//        //{
//        //    mainCamera.gameObject.SetActive(true);  // Aktifkan kembali kamera utama
//        //    inventoryCamera.gameObject.SetActive(false);  // Matikan kamera inventory
//        //    Time.timeScale = 1f;
//        //}

//        RefreshUI();
//    }


//    void SwitchTab(bool isEquipment)
//    {
//        equipmentTab.SetActive(isEquipment);
//        resourceTab.SetActive(!isEquipment);
//    }

//    public void AddItem(string name, int quantity, ItemType type, float healthRestore = 0)
//    {
//        if (inventory.ContainsKey(name))
//        {
//            inventory[name].quantity += quantity;
//        }
//        else
//        {
//            inventory[name] = new InventoryItem { name = name, quantity = quantity, type = type, healthRestore = healthRestore };
//        }

//        RefreshUI();
//    }

//    public void RemoveItem(string name)
//    {
//        if (inventory.ContainsKey(name))
//        {
//            inventory[name].quantity--;
//            if (inventory[name].quantity <= 0)
//            {
//                inventory.Remove(name);
//            }
//        }

//        RefreshUI();
//    }

//    public void RefreshUI()
//    {
//        foreach (Transform child in equipmentContainer) Destroy(child.gameObject);
//        foreach (Transform child in resourceContainer) Destroy(child.gameObject);

//        foreach (var item in inventory)
//        {
//            if (item.Value.type == ItemType.Equipment)
//            {
//                GameObject newItem = Instantiate(itemPrefab, equipmentContainer);
//                newItem.GetComponent<ItemUI>().Setup(item.Value, true);
//            }
//            else if (item.Value.type == ItemType.Resource && item.Value.quantity > 0)
//            {
//                GameObject newItem = Instantiate(itemPrefab, resourceContainer);
//                newItem.GetComponent<ItemUI>().Setup(item.Value, false);
//            }
//        }
//    }

//    public void ConsumeItem(string name)
//    {
//        if (inventory.ContainsKey(name) && inventory[name].type == ItemType.Resource)
//        {
//            PlayerHealth.Instance.RestoreHealth(inventory[name].healthRestore);
//            RemoveItem(name);
//        }
//    }
//}

//public enum ItemType { Equipment, Resource }

//public class InventoryItem
//{
//    public string name;
//    public int quantity;
//    public ItemType type;
//    public float healthRestore;
//}
