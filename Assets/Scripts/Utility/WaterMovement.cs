using UnityEngine;

public class WaterMovement : MonoBehaviour
{

    public float waveSpeed = 1f;  // Kecepatan gelombang
    public float waveHeight = 0.5f; // Tinggi gelombang
    public float waveFrequency = 1f; // Frekuensi gelombang

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Gerakan gelombang naik-turun menggunakan sin()
        float wave = Mathf.Sin(Time.time * waveSpeed) * waveHeight;
        transform.position = new Vector3(startPos.x, startPos.y + wave, startPos.z);
    }

}
