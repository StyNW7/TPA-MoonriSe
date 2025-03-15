using UnityEngine;

public class LootManager : MonoBehaviour
{
    public LootTableSO lootTable;
    public GameObject experiencePrefab;

    public void DropLoot(Vector3 position, int zombieLevel)
    {
        foreach (var loot in lootTable.lootItems)
        {
            int dropCount = Random.Range(0, 4); // 0 - 3
            if (dropCount > 0)
            {
                for (int i = 0; i < dropCount; i++)
                {
                    Vector3 spawnPos = position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
                    Instantiate(loot.prefab, spawnPos, Quaternion.identity);
                }
            }
        }

        // Drop experience berdasarkan formula
        int expAmount = Mathf.RoundToInt(150 * (zombieLevel / 2f));
        for (int i = 0; i < Random.Range(1, 4); i++) // Drop 1 - 3 exp
        {
            Vector3 expPos = position + new Vector3(Random.Range(-1f, 1f), 0.5f, Random.Range(-1f, 1f));
            GameObject exp = Instantiate(experiencePrefab, expPos, Quaternion.identity);
            //exp.GetComponent<ExperienceLoot>().SetExperience(expAmount);
        }
    }
}
