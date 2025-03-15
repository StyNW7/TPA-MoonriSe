//using UnityEngine;
//using UnityEngine.UI;

//public class ItemUI : MonoBehaviour
//{

//    public Text itemName;
//    public Text itemQuantity;
//    public Button itemButton;
//    private InventoryItem currentItem;
//    private bool isEquipment;

//    public void Setup(InventoryItem item, bool equipment)
//    {
//        currentItem = item;
//        isEquipment = equipment;
//        itemName.text = item.name;
//        itemQuantity.text = item.quantity.ToString();

//        itemButton.onClick.AddListener(() => OnItemClicked());
//    }

//    void OnItemClicked()
//    {
//        if (isEquipment)
//        {
//            EquipmentManager.Instance.EquipItem(currentItem.name);
//        }
//        else
//        {
//            InventoryManager.Instance.ConsumeItem(currentItem.name);
//        }
//    }

//}
