using UnityEngine;

//lootSpawner.SpawnLoot(transform.position);
public class LootSpawner : MonoBehaviour
{
    public LootTableSO lootTable;

    public void SpawnLoot(Vector3 spawnPosition)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        int randomIndex = Random.Range(0, lootTable.lootItems.Count);
        LootItemSO selectedLoot = lootTable.lootItems[randomIndex];

        int lootAmount = Random.Range(0, 4);

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

    public void SpawnWood(Vector3 spawnPosition)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        int randomIndex = Random.Range(0, lootTable.lootItems.Count);
        LootItemSO selectedLoot = lootTable.lootItems[3];

        int lootAmount = Random.Range(0, 4);

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

    public void SpawnXP(Vector3 spawnPosition, int zombieLevel)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        LootItemSO selectedLoot = lootTable.lootItems[4];

        int lootAmount = Random.Range(0, 4);

        // lootAmount += 150 * (zombieLevel/2);

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

    public void SpawnTomato(Vector3 spawnPosition)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        int randomIndex = Random.Range(0, lootTable.lootItems.Count);
        LootItemSO selectedLoot = lootTable.lootItems[0];

        int lootAmount = Random.Range(0, 4);
        // int lootAmount = 2;

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

    public void SpawnBerries(Vector3 spawnPosition)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        int randomIndex = Random.Range(0, lootTable.lootItems.Count);
        LootItemSO selectedLoot = lootTable.lootItems[1];

        int lootAmount = Random.Range(0, 4);

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

    public void SpawnBamboo(Vector3 spawnPosition)
    {
        if (lootTable == null || lootTable.lootItems.Count == 0)
        {
            Debug.LogWarning("Loot Table kosong atau tidak diassign!");
            return;
        }

        int randomIndex = Random.Range(0, lootTable.lootItems.Count);
        LootItemSO selectedLoot = lootTable.lootItems[2];

        int lootAmount = Random.Range(0, 4);

        for (int i = 0; i < lootAmount; i++)
        {
            Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0.5f, Random.Range(-0.5f, 0.5f));
            GameObject lootObject = Instantiate(selectedLoot.prefab, spawnPosition + randomOffset, Quaternion.identity);

            LootBehavior lootBehavior = lootObject.GetComponent<LootBehavior>();
            if (lootBehavior != null)
            {
                lootBehavior.SetLootItem(selectedLoot);
            }
        }
    }

}
