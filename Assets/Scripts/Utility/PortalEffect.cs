using System.Collections;
using UnityEngine;

public class PortalEffect : MonoBehaviour
{
    public float rotationSpeed = 50f;  // Kecepatan rotasi
    public float pulseSpeed = 2f;      // Kecepatan denyutan
    public float pulseIntensity = 0.1f; // Seberapa besar efek denyutan

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(PulseEffect());
    }

    void Update()
    {
        // Portal terus berputar
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    IEnumerator PulseEffect()
    {
        while (true)
        {
            float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseIntensity;
            transform.localScale = originalScale * scaleFactor;
            yield return null;
        }
    }
}
