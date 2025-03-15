using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ZombieBar : MonoBehaviour
{

    public Slider healthSlider;
    public Image healthFillImage;

    private float maxHealth;
    private ZombieAI zombieManager;
    public float updateSpeed = 0.5f;

    private void Start()
    {
        zombieManager = FindObjectOfType<ZombieAI>();
        if (zombieManager != null)
        {
            InitializeBars(zombieManager.GetHealth());
            healthFillImage.color = Color.green;
        }
    }

    public void InitializeBars(float maxHealth)
    {
        this.maxHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        StartCoroutine(SmoothHealthUpdate(currentHealth));
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

}
