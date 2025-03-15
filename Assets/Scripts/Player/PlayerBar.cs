using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Slider healthSlider;
    public Image healthFillImage;

    public Slider xpSlider;
    public Image xpFillImage;

    private float maxHealth;
    private int maxXP;
    private PlayerManager playerManager;
    public float updateSpeed = 0.5f; // Kecepatan animasi perubahan bar

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (playerManager != null)
        {
            InitializeBars(playerManager.GetHealth(), 1000 + (playerManager.GetLevel() * 200));
            healthFillImage.color = Color.green;
        }
    }

    public void InitializeBars(float maxHealth, int maxXP)
    {
        this.maxHealth = maxHealth;
        this.maxXP = maxXP;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;

        xpSlider.maxValue = maxXP;
        xpSlider.value = 0;
    }

    public void UpdateHealth(float currentHealth)
    {
        StartCoroutine(SmoothHealthUpdate(currentHealth));
    }

    public void UpdateXP(int currentXP, int nextLevelXP)
    {
        xpSlider.maxValue = nextLevelXP;
        StartCoroutine(SmoothXPUpdate(currentXP));
    }

    private IEnumerator SmoothHealthUpdate(float targetHealth)
    {
        float startHealth = healthSlider.value;
        float elapsedTime = 0f;

        while (elapsedTime < updateSpeed)
        {
            elapsedTime += Time.deltaTime;
            healthSlider.value = Mathf.Lerp(startHealth, targetHealth, elapsedTime / updateSpeed);
            UpdateHealthBarColor();
            yield return null;
        }

        healthSlider.value = targetHealth;
        UpdateHealthBarColor();
    }

    private IEnumerator SmoothXPUpdate(float targetXP)
    {
        float startXP = xpSlider.value;
        float elapsedTime = 0f;

        while (elapsedTime < updateSpeed)
        {
            elapsedTime += Time.deltaTime;
            xpSlider.value = Mathf.Lerp(startXP, targetXP, elapsedTime / updateSpeed);
            UpdateXPBarColor();
            yield return null;
        }

        xpSlider.value = targetXP;
        UpdateXPBarColor();
    }

    private void UpdateHealthBarColor()
    {
        float healthPercentage = healthSlider.value / healthSlider.maxValue;
        if (healthPercentage > 0.5f)
            healthFillImage.color = Color.green;
        else if (healthPercentage > 0.2f)
            healthFillImage.color = Color.yellow;
        else
            healthFillImage.color = Color.red;
    }

    private void UpdateXPBarColor()
    {
        float xpPercentage = xpSlider.value / xpSlider.maxValue;
        xpFillImage.color = Color.Lerp(Color.red, Color.blue, xpPercentage);
    }
}
