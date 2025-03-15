using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StoreItem
{
    public string itemName;
    public string itemDescription;
    public int price;
    public bool isOwned;

    public string spriteName; // Simpan nama sprite

    // [System.NonSerialized]
    public Texture itemSprite;
}