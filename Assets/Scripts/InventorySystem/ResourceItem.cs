using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ResourceItem
{
    public string itemName;
    public int price;
    public int ownedQuantity;

    public string spriteName; // Simpan nama sprite

    // [System.NonSerialized]
    public Texture itemSprite;
}