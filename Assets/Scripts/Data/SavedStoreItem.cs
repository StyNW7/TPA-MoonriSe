using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SavedStoreItem
{
    public string itemName;
    public bool isOwned;

    public SavedStoreItem(string name, bool owned)
    {
        itemName = name;
        isOwned = owned;
    }

}