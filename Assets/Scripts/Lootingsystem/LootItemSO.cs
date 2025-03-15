using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Item", menuName = "Loot System/Loot Item")]
public class LootItemSO : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public int minAmount;
    public int maxAmount;
}
