using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void RestoreHealth(float percentage)
    {
        currentHealth += maxHealth * (percentage / 100);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Health Restored: " + currentHealth);
    }
}
