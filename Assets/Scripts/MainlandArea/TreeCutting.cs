using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCutting : MonoBehaviour
{
    public int requiredHits = 3; // Hit yang dibutuhkan sebelum tumbang
    private int currentHits = 0;
    private bool isFalling = false;

    public GameObject treeModel; // Model pohon berdiri
    public GameObject fallenTreeModel; // Model pohon tumbang (nonaktif default)
    public GameObject woodLootPrefab; // Prefab loot kayu

    private Transform playerTransform; // Referensi ke transform player
    public float interactionDistance = 3f; // Jarak interaksi maksimal

    public LootSpawner lootSpawner;
    private static List<TreeCutting> treesInRange = new List<TreeCutting>();
    
    private void Start()
    {
        fallenTreeModel.SetActive(false); // Pohon tumbang disembunyikan
        woodLootPrefab.SetActive(false);

        // Cari player otomatis jika belum di-set
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        lootSpawner = FindObjectOfType<LootSpawner>();
        if (player != null)
            playerTransform = player.transform;
    }

    private void Update()
    {
        if (playerTransform == null || isFalling) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);
        if (distance <= interactionDistance)
        {
            if (!treesInRange.Contains(this))
            {
                treesInRange.Add(this);
            }
        }
        else
        {
            treesInRange.Remove(this);
        }

        UpdateInteractionPanel();
    }

    private void UpdateInteractionPanel()
    {
        if (treesInRange.Count > 0)
        {
            TreeCutting nearestTree = GetNearestTree();
            if (nearestTree != null && EquipmentManager.Instance.GetEquipmentWeapon() == "Axe")
            {
                InteractionManager.Instance.ShowInteractionPanel(nearestTree.transform.position, "Cut Tree");
            }
        }
        else
        {
            InteractionManager.Instance.HideInteractionPanel();
        }
    }

    private TreeCutting GetNearestTree()
    {
        TreeCutting nearestTree = null;
        float minDistance = float.MaxValue;

        foreach (TreeCutting tree in treesInRange)
        {
            float distance = Vector3.Distance(tree.transform.position, playerTransform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestTree = tree;
            }
        }
        return nearestTree;
    }

    public void CutTree()
    {
        if (isFalling) return;

        currentHits++;
        if (currentHits >= requiredHits)
        {
            StartCoroutine(FallTree());
        }
    }

    private IEnumerator FallTree()
    {
        isFalling = true;
        treesInRange.Remove(this);
        UpdateInteractionPanel();

        yield return new WaitForSeconds(0.5f); // Delay sebelum pohon tumbang

        treeModel.SetActive(false);
        fallenTreeModel.SetActive(true); // Aktifkan model pohon tumbang

        Rigidbody rb = fallenTreeModel.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            rb.AddForce(randomDirection * 5f, ForceMode.Impulse);
        }

        lootSpawner.SpawnWood(transform.position);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
