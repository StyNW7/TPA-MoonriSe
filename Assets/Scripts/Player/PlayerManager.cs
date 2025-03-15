using StarterAssets;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{

    private int worldLevel = 0;
    private int level = 1;
    private int money = 100;
    private int experience = 0;
    private float health;
    private float max_health;

    private PlayerBar playerBar;
    public TMP_Text LevelText;
    public TMP_Text CoinText;

    public bool StatsPanelOpen = false;
    public GameObject PlayerStatsPanel;
    public TMP_Text CoinStats;
    public TMP_Text LevelStats;
    public TMP_Text ExpStats;
    public TMP_Text HealthStats;

    public GameObject PlayerBody;

    public GameObject damagePopUpPrefab;

    public static PlayerManager Instance;
    public ThirdPersonController tpc;

    public NPCInteraction npc1;
    public NPCInteraction2 npc2;

    [SerializeField] private Animator animator;

    public GameObject deathPanel;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    public GameObject damageTextPrefab; // Prefab DamageText
    public Transform damageTextSpawnPoint; // Posisi spawn damage text

    public CharacterController characterController;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        Debug.Log("Player Manager starttttt");

        max_health = CalculateHealth();
        health = max_health;
        playerBar = FindObjectOfType<PlayerBar>();

        playerController = FindObjectOfType<ThirdPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();

        // if (playerController != null) playerController.enabled = false;
        // if (playerInput != null) playerInput.DeactivateInput();

        if (playerBar != null)
        {
            Debug.Log("Update UI");
            playerBar.InitializeBars(max_health, GetNextLevelXP());
            playerBar.UpdateHealth(health);
        }

        UpdateUI();
        PlayerStatsPanel.SetActive(false);

        if (playerController != null) playerController.enabled = true;
        if (playerInput != null) playerInput.ActivateInput();

        characterController = GetComponent<CharacterController>();

    }

    void Update()
    {

        if (experience >= GetNextLevelXP())
        {
            LevelUp();
        }

        if (playerBar != null)
        {
            playerBar.UpdateXP(experience, GetNextLevelXP());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ShowPlayerStats();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            tpc.SetCtrlKeyPressed();
        }

        //if (Input.GetMouseButtonDown(0) && EquipmentManager.Instance.GetEquipmentWeapon() == "Sword")
        //{

        //}

        //if (Input.GetKey(KeyCode.T))
        //{
        //    ShowDamageText(10);
        //}

    }

    float CalculateHealth(int updatedLevel)
    {
        return Mathf.FloorToInt(150 * Mathf.Pow(updatedLevel, 1.5f) * updatedLevel);
    }

    float CalculateHealth()
    {
        return Mathf.FloorToInt(150 * Mathf.Pow(level, 1.5f) * level);
    }

    public int GetNextLevelXP(int updatedLevel)
    {
        return 1000 + (updatedLevel * 200);
    }

    public int GetNextLevelXP()
    {
        return 1000 + (level * 200);
    }

    public void GainXP(int amount)
    {
        experience += amount;
    }

    public void PlayerLostHealth(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            playerBar.UpdateHealth(0);
            Die();
            return;
        }

        if (playerBar != null)
        {
            playerBar.UpdateHealth(health);
        }
    }

    public void PlayerGetHealth(int amount)
    {
        health += amount;
        if (health > max_health) health = max_health;
        if (playerBar != null)
        {
            playerBar.UpdateHealth(health);
        }
    }

    public void RestoreHealth(float percentage)
    {
        health += max_health * (percentage / 100);
        health = Mathf.Clamp(health, 0, max_health);
        Debug.Log("Health Restored: " + health);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateUI();
    }

    public void DeductMoney(int amount)
    {
        money -= amount;
        UpdateUI();
    }

    public void LevelUp()
    {
        level++;
        experience -= GetNextLevelXP();

        // Update baru
        // health = CalculateHealth();
        max_health = CalculateHealth();

        Debug.Log("Level Up! Level: " + level);

        if (playerBar != null)
        {
            playerBar.InitializeBars(health, GetNextLevelXP());
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (LevelText != null)
        {
            Debug.Log("WorldLevel" + worldLevel);
            // LevelText.text = $"{level}";
            LevelText.text = $"{worldLevel}";
        }

        if (CoinText != null)
        {
            CoinText.text = FormatMoney(money);
        }
    }

    private string FormatMoney(int amount)
    {
        if (amount >= 1_000_000_000)
            return (amount / 1_000_000_000f).ToString("0.##") + "B+";
        else if (amount >= 1_000_000)
            return (amount / 1_000_000f).ToString("0.##") + "M";
        else if (amount >= 1_000)
            return (amount / 1_000f).ToString("0.##") + "K";
        else
            return amount.ToString();
    }

    public void ShowPlayerStats()
    {

        if (StatsPanelOpen == false)
        {

            CoinStats.text = FormatMoney(money);
            LevelStats.text = $"LV. {level}";
            ExpStats.text = $"{FormatMoney(experience)}Exp";
            HealthStats.text = $"{FormatMoney((int)health)}/{FormatMoney((int)max_health)}";

            PlayerStatsPanel.SetActive(true);
            StatsPanelOpen = true;
            //PlayerBody.SetActive(false);
            Time.timeScale = 0f;

            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.DeactivateInput();
        }

        else
        {
            PlayerStatsPanel.SetActive(false);
            StatsPanelOpen = false;
            //PlayerBody.SetActive(true);
            Time.timeScale = 1f;

            if (playerController != null) playerController.enabled = true;
            if (playerInput != null) playerInput.ActivateInput();
        }
            
    }


    public void TakeDamage(float damage, bool isCritical = false)
    {

        health -= damage;
        if (health <= 0) StartCoroutine(Die());

        playerBar.UpdateHealth(health);

        //ShowDamagePopUp(damage, isCritical);

        ShowDamageText(damage);

    }

    private void ShowDamageText(float damage)
    {
        if (damageTextPrefab)
        {
            GameObject dmgText = Instantiate(damageTextPrefab, damageTextSpawnPoint.position, Quaternion.identity);
            dmgText.GetComponent<DamageText>().SetDamage(damage);
        }
    }

    private void ShowDamagePopUp(float damage, bool isCritical)
    {
        GameObject popUp = Instantiate(damagePopUpPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
        popUp.GetComponent<DamagePopUp>().Setup(damage, isCritical);
    }

    private IEnumerator Die()
    {
        // Debug.Log("Player has died!");

        // Animate the player dead animation

        animator.SetBool("isDie", true);

        Rigidbody rb = GetComponent<Rigidbody>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        if (rb != null)
        {
            rb.isKinematic = false; // Biarkan physics bekerja
            rb.useGravity = true;
        }

        if (animator != null)
        {
            animator.applyRootMotion = true;
        }
        
        yield return new WaitForSeconds(2.0f);

        deathPanel.SetActive(true);

        if (playerController != null) playerController.enabled = false;
        if (playerInput != null) playerInput.DeactivateInput();

        // Start the coroutine to delay the scene transition
        StartCoroutine(WaitBeforeReset());
    }

    // Coroutine that waits for a few seconds before resetting everything
    private IEnumerator WaitBeforeReset()
    {
        yield return new WaitForSeconds(4f);

        // Reset everything
        ResetEverything();

        // Load MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }


    private void ResetEverything()
    {

        level = 1;
        money = 100;
        experience = 0;
        worldLevel = 0;

        // Every equipment, farm, item etc Reset

        npc1.ResetItemOwnership();
        npc2.ResetResource();

        List<StoreItem> ownedItems = npc1.GetAllItems();
        List<ResourceItem> ownedResources = npc2.GetAllResources();

        DataManager.Instance.SaveGame(150, 150, 1, 100,
                0, 0, ownedItems, ownedResources);

    }

    public bool GetStatsPanelOpen() => StatsPanelOpen;

    public int GetLevel() => level;

    public int GetWorldLevel() => worldLevel;
    public int GetMoney() => money;
    public int GetXP() => experience;
    public float GetHealth() => health;

    public void SetWorldLevel(int saveLevel) => worldLevel += saveLevel;
    public void SetLevel(int saveLevel) => level = saveLevel;
    public void SetMoney(int saveLevel) => money = saveLevel;
    public void SetXP(int saveLevel) => experience = saveLevel;
    public void SetHealth(float saveHealth) => health = saveHealth;

    public float GetMaxHealth() => max_health;
    public void SetMaxHealth(float saveHealth) => max_health = saveHealth;

}
