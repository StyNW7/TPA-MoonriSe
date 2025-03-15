using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Loot Table", menuName = "Loot System/Loot Table")]
public class LootTableSO : ScriptableObject
{
    public List<LootItemSO> lootItems;
}
