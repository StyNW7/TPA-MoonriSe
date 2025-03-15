using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SavedResourceItem
{
    public string itemName;
    public int quantity;

    public SavedResourceItem(string name, int itemQuantity)
    {
        itemName = name;
        quantity = itemQuantity;
    }

}