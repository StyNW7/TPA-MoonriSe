using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Untuk UI Interaction Panel
using TMPro;

public class GrowingPlant : MonoBehaviour
{
    [System.Serializable]
    public class GrowthStage
    {
        public GameObject prefab;
        public float timeToNextStage;
    }

    public GrowthStage[] growthStages;
    public string plantName;
    public bool isMatured = false;
    public GameObject lootPrefab;
    public Transform player; // Referensi ke player
    public float interactionDistance = 2f; // Jarak interaksi
    public GameObject interactionPanel; // Panel UI Interaksi
    public TMP_Text interactionText; // Text UI untuk interaksi

    public int currentStage = 0;
    public GameObject currentPlantObject;
    private SimpleGrid gridSystem;
    private bool isPlayerNear = false;

    public LootSpawner lootSpawner;

    void Start()
    {
        gridSystem = FindObjectOfType<SimpleGrid>();
        lootSpawner = FindObjectOfType<LootSpawner>();

        player = GameObject.Find("PlayerArmature").transform;
        //interactionPanel = GameObject.FindWithTag("GrowPanel");
        //interactionText = interactionPanel?.GetComponentInChildren<TMP_Text>();

        //if (interactionPanel != null)
        //    interactionPanel.SetActive(false); // Sembunyikan UI di awal


        // GameObject panelObject = GameObject.Find("PlayerInteractionGrowingPlant");

        // if (panelObject != null)
        // {
        //     interactionPanel = panelObject;
        //     interactionText = interactionPanel.GetComponentInChildren<TMP_Text>();
        // }
        // else
        // {
        //     Debug.LogError("Interaction Panel tidak ditemukan di scene!");
        // }

        // // Pastikan Interaction Panel tetap tersembunyi di awal
        // if (interactionPanel != null)
        // {
        //     interactionPanel.SetActive(false);
        // }


        if (growthStages.Length > 0)
        {
            currentPlantObject = Instantiate(growthStages[currentStage].prefab, transform.position, Quaternion.identity, transform);
            StartCoroutine(Grow());
        }
        // interactionPanel.SetActive(false); // Sembunyikan UI saat awal
    }

    void Update()
    {
        CheckPlayerDistance();
        Interact();
    }

    IEnumerator Grow()
    {
        while (currentStage < growthStages.Length - 1)
        {
            yield return new WaitForSeconds(growthStages[currentStage].timeToNextStage);
            if (currentPlantObject != null)
                Destroy(currentPlantObject);
            
            currentStage++;
            currentPlantObject = Instantiate(growthStages[currentStage].prefab, transform.position, Quaternion.identity, transform);
        }

        isMatured = true;
    }

    void CheckPlayerDistance()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance <= interactionDistance)
        {
            if (!isPlayerNear)
            {
                isPlayerNear = true;
                ShowInteractionUI();
            }
        }
        else
        {
            if (isPlayerNear)
            {
                isPlayerNear = false;
                HideInteractionUI();
            }
        }
    }

    void ShowInteractionUI()
    {
        // interactionPanel.SetActive(true);
        // interactionText.text = isMatured ? "Press [E] to Harvest" : "Press [E] to Destroy";

        if (GrowingPanelManager.Instance != null)
        {
            string message = isMatured ? "Press [F] to Harvest" : "Press [F] to Destroy";
            GrowingPanelManager.Instance.ShowInteractionUI(transform, message);
        }
    }

    void HideInteractionUI()
    {
        // interactionPanel.SetActive(false);
        if (GrowingPanelManager.Instance != null)
        {
            GrowingPanelManager.Instance.HideInteractionUI(transform);
        }
    }

    public void Interact()
    {
        if (isPlayerNear)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (isMatured)
                {
                    Harvest();
                }
                else
                {
                    Remove();
                }
            }
        }
    }

    public void Harvest()
    {
        if (lootPrefab != null)
        {
            // Instantiate(lootPrefab, transform.position, Quaternion.identity);
            if (plantName == "Tomato")
                lootSpawner.SpawnTomato(transform.position);
            else if (plantName == "Berries")
                lootSpawner.SpawnBerries(transform.position);
            else if (plantName == "Bamboo")
                lootSpawner.SpawnBamboo(transform.position);
        }
        Destroy(gameObject);
        HideInteractionUI();
    }

    public void Remove()
    {
        Destroy(gameObject);
        HideInteractionUI();
    }

}
