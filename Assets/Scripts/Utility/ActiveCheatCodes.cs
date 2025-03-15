using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class ActiveCheatCodes : MonoBehaviour
{

    public GameObject cheatPanel;
    public CanvasGroup cheatCanvasGroup;
    public TMP_Text cheatTextTitle;
    public TMP_Text cheatTextDescription;
    private string currentInput = "";
    private PlayerManager playerManager;

    public GameObject StatsPanel;
    public PlayerManager PlayerManager;

    public GameObject PlayerBody;
    private CharacterController characterController;
    private Rigidbody playerRigidbody;

    // Ruins and Dock Location
    public GameObject RuinsLocation;
    public GameObject DockLocation;

    public GameObject VillageLocation;

    public GameObject TeleportRoom;

    public NPCInteraction npc1;
    public NPCInteraction2 npc2;

    public ThirdPersonController thirdPersonController;

    private Dictionary<string, string> cheatCodes = new Dictionary<string, string>()
    {
        { "duaempatsatuu", "Own All Equipment" },
        { "makemixyuey", "Gain a Large Money" },
        { "poksupremacy", "Rich in Resources" },
        { "subcoandsbadut", "Gain a Large Experience" },
        { "casemakernangis", "Only Take 10s to Grow Plants" },
        { "secrein", "Teleport to The Ruins Area" },
        { "secreout", "Teleport to The Exit Room (in the dungeon)" },
        { "fishingmania", "Teleport to the Dock" },
        { "dualimasatuu", "Back to the Home" },
        { "die", "Delete Everything :)" }
    };

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        if (PlayerBody != null)
        {
            characterController = PlayerBody.GetComponent<CharacterController>();
            playerRigidbody = PlayerBody.GetComponent<Rigidbody>();
        }

        if (cheatCanvasGroup == null)
        {
            cheatCanvasGroup = cheatPanel.GetComponent<CanvasGroup>();
        }
        thirdPersonController = FindObjectOfType<ThirdPersonController>();
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                currentInput += char.ToLower(c);
            }
            else if (c == '\b' && currentInput.Length > 0) // Handle backspace
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r') // Enter untuk eksekusi
            {
                CheckCheatCode();
            }
        }

        if (currentInput.Length > 20)
        {
            currentInput = currentInput.Substring(currentInput.Length - 20);
        }

        CheckCheatCode(); // Cek cheat code secara real-time tanpa harus menekan Enter
    }

    void CheckCheatCode()
    {
        foreach (var cheat in cheatCodes.Keys)
        {
            if (currentInput.Contains(cheat))
            {
                ActivateCheat(cheat);
                currentInput = ""; // Reset input setelah cheat aktif
                return;
            }
        }
    }

    void ActivateCheat(string cheat)
    {
        if (PlayerManager.GetStatsPanelOpen())
        {
            PlayerManager.ShowPlayerStats();
        }

        string cheatDescription = cheatCodes[cheat];

        cheatPanel.SetActive(true);
        cheatTextTitle.text = $"{cheat}";
        cheatTextDescription.text = $"{cheatDescription}";

        StopAllCoroutines();
        StartCoroutine(FadeInCheatPanel());

        switch (cheat)
        {
            case "duaempatsatuu":
                OwnAllEquipment();
                break;
            case "makemixyuey":
                GainLargeMoney();
                break;
            case "poksupremacy":
                RichInResources();
                break;
            case "subcoandsbadut":
                GainLargeExperience();
                break;
            case "casemakernangis":
                FastGrowPlants();
                break;
            case "secrein":
                TeleportToRuins();
                break;
            case "secreout":
                TeleportToExitRoom();
                break;
            case "fishingmania":
                TeleportToDock();
                break;
            case "dualimasatuu":
                BackToVillage();
                break;
            case "die":
                DeleteVEverything();
                break;
        }

        StartCoroutine(FadeOutCheatPanel());
    }

    IEnumerator FadeInCheatPanel()
    {
        cheatCanvasGroup.alpha = 0;
        cheatPanel.SetActive(true);

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cheatCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cheatCanvasGroup.alpha = 1;
    }

    IEnumerator FadeOutCheatPanel()
    {
        yield return new WaitForSeconds(3f); // Tunggu sebelum fade out

        float duration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cheatCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cheatCanvasGroup.alpha = 0;
        cheatPanel.SetActive(false);
    }

    void DeleteVEverything()
    {
        npc1.ResetItemOwnership();
        npc2.ResetResource();
    }

    void OwnAllEquipment()
    {
        Debug.Log("All Equipment Unlocked!");
        npc1.GetItemOwnership();
    }

    void GainLargeMoney()
    {
        Debug.Log("Gained a large amount of money!");
        playerManager.AddMoney(10000000);
    }

    void RichInResources()
    {
        Debug.Log("Resources have been maxed out!");
        npc2.GetAllResource(300);
    }

    void GainLargeExperience()
    {
        Debug.Log("Player gained a large amount of experience!");
        playerManager.GainXP(10000);
    }

    void FastGrowPlants()
    {

        Debug.Log("Grow plants in 10s!");

        GrowingPlant[] plants = FindObjectsOfType<GrowingPlant>();
        foreach (GrowingPlant plant in plants)
        {
            StartCoroutine(FastGrowCoroutine(plant));
        }
    }

    IEnumerator FastGrowCoroutine(GrowingPlant plant)
    {
        while (plant.currentStage < plant.growthStages.Length - 1)
        {
            yield return new WaitForSeconds(10f / plant.growthStages.Length);
            if (plant.currentPlantObject != null)
                Destroy(plant.currentPlantObject);
            
            plant.currentStage++;
            plant.currentPlantObject = Instantiate(plant.growthStages[plant.currentStage].prefab, plant.transform.position, Quaternion.identity, plant.transform);
        }
        plant.isMatured = true;
    }

    void TeleportToRuins()
    {
        if (PlayerBody == null || RuinsLocation == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody or RuinsLocation is not assigned!");
            return;
        }

        Debug.Log("Teleported to The Ruins Area!");

        if (characterController != null) {
            characterController.enabled = false;
            thirdPersonController.CannotMove(true);
        }
        if (playerRigidbody != null)
            playerRigidbody.MovePosition(RuinsLocation.transform.position);
        else
            PlayerBody.transform.position = RuinsLocation.transform.position;

        if (characterController != null)
            StartCoroutine(EnableCharacterController());
    }

    void TeleportToExitRoom()
    {
        if (PlayerBody == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody is not assigned!");
            return;
        }

        // Ambil posisi Exit Room dari DungeonGenerator
        Vector3 exitPosition = DungeonGenerator.ExitRoomPosition;

        Debug.Log("Teleported to The Exit Area!");

        if (characterController != null)
        {
            characterController.enabled = false;
            thirdPersonController.CannotMove(true);
        }
        
        if (playerRigidbody != null)
            playerRigidbody.MovePosition(exitPosition);
        else
            PlayerBody.transform.position = exitPosition;

        if (characterController != null)
            StartCoroutine(EnableCharacterController());
    }


    void TeleportToDock()
    {
        if (PlayerBody == null || DockLocation == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody or DockLocation is not assigned!");
            return;
        }

        Debug.Log("Teleported to The Dock!");

        if (characterController != null) {
            characterController.enabled = false;
            thirdPersonController.CannotMove(true);
        }
        if (playerRigidbody != null)
            playerRigidbody.MovePosition(DockLocation.transform.position);
        else
            PlayerBody.transform.position = DockLocation.transform.position;

        if (characterController != null)
            StartCoroutine(EnableCharacterController());
    }

    void BackToVillage()
    {
        if (PlayerBody == null || VillageLocation == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody or DockLocation is not assigned!");
            return;
        }

        Debug.Log("Teleported to The Village!");

        if (characterController != null) {
            characterController.enabled = false;
            thirdPersonController.CannotMove(true);
        }
        if (playerRigidbody != null)
            playerRigidbody.MovePosition(VillageLocation.transform.position);
        else
            PlayerBody.transform.position = VillageLocation.transform.position;

        if (characterController != null)
            StartCoroutine(EnableCharacterController());
    }

    IEnumerator EnableCharacterController()
    {
        yield return new WaitForSeconds(0.1f);
        characterController.enabled = true;
        thirdPersonController.CannotMove(false);
    }

}
