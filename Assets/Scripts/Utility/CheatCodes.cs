using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CheatCodeManager : MonoBehaviour
{
    public GameObject cheatPanel;
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

    private Dictionary<string, string> cheatCodes = new Dictionary<string, string>()
    {
        { "duaempatsatuu", "Own All Equipment" },
        { "makemixyuey", "Gain a Large Money" },
        { "poksupremacy", "Rich in Resources" },
        { "subcoandsbadut", "Gain a Large Experience" },
        { "casemakernangis", "Only Take 10s to Grow Plants" },
        { "secrein", "Teleport to The Ruins Area" },
        { "secreout", "Teleport to The Exit Room (in the dungeon)" },
        { "fishingmania", "Teleport to the Dock" }
    };

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();

        if (PlayerBody != null)
        {
            characterController = PlayerBody.GetComponent<CharacterController>();
            playerRigidbody = PlayerBody.GetComponent<Rigidbody>();
        }
    }

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (char.IsLetter(c))
            {
                currentInput += char.ToLower(c);
            }
            else if (c == '\b' && currentInput.Length > 0)
            {
                currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            else if (c == '\n' || c == '\r')
            {
                CheckCheatCode();
            }
        }

        if (currentInput.Length > 20)
        {
            currentInput = currentInput.Substring(currentInput.Length - 20);
        }
    }

    void CheckCheatCode()
    {
        if (cheatCodes.ContainsKey(currentInput))
        {
            ActivateCheat(currentInput);
        }
        currentInput = "";
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

        StartCoroutine(HideCheatPanel());

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
        }
    }

    IEnumerator HideCheatPanel()
    {
        yield return new WaitForSeconds(3f);
        cheatPanel.SetActive(false);
    }

    void OwnAllEquipment()
    {
        Debug.Log("All Equipment Unlocked!");
    }

    void GainLargeMoney()
    {
        Debug.Log("Gained a large amount of money!");
        playerManager.AddMoney(10000000);
    }

    void RichInResources()
    {
        Debug.Log("Resources have been maxed out!");
    }

    void GainLargeExperience()
    {
        Debug.Log("Player gained a large amount of experience!");
        playerManager.GainXP(10000);
    }

    void FastGrowPlants()
    {
        Debug.Log("Plants now grow in 10s!");
    }

    void TeleportToRuins()
    {
        if (PlayerBody == null || RuinsLocation == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody or RuinsLocation is not assigned!");
            return;
        }

        Debug.Log("Teleported to The Ruins Area!");

        // Jika menggunakan CharacterController, nonaktifkan sebelum teleportasi
        if (characterController != null)
        {
            characterController.enabled = false;
        }

        // Jika menggunakan Rigidbody, gunakan MovePosition agar physics tetap bekerja
        if (playerRigidbody != null)
        {
            playerRigidbody.MovePosition(RuinsLocation.transform.position);
        }
        else
        {
            PlayerBody.transform.position = RuinsLocation.transform.position;
        }

        // Reaktifkan CharacterController setelah teleport
        if (characterController != null)
        {
            StartCoroutine(EnableCharacterController());
        }
    }

    void TeleportToExitRoom()
    {
        Debug.Log("Teleported to The Exit Room!");
    }

    void TeleportToDock()
    {
        if (PlayerBody == null || DockLocation == null)
        {
            Debug.LogWarning("Teleport failed: PlayerBody or DockLocation is not assigned!");
            return;
        }

        Debug.Log("Teleported to The Dock!");

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.MovePosition(DockLocation.transform.position);
        }
        else
        {
            PlayerBody.transform.position = DockLocation.transform.position;
        }

        if (characterController != null)
        {
            StartCoroutine(EnableCharacterController());
        }
    }

    IEnumerator EnableCharacterController()
    {
        yield return new WaitForSeconds(0.1f); // Delay agar posisi diperbarui dulu
        characterController.enabled = true;
    }
}
