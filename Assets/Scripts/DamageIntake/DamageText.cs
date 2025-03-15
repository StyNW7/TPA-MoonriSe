using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    public TMP_Text textMesh;   
    public float moveSpeed = 1.5f;
    public float fadeSpeed = 2f;
    public float bounceFrequencyMin = 4f;
    public float bounceFrequencyMax = 8f;
    public float bounceAmplitudeMin = 0.15f;
    public float bounceAmplitudeMax = 0.35f;
    public float lifetime = 1.2f;

    private Vector3 startPosition;
    private float elapsedTime = 0f;
    private Vector3 randomOffset;
    private Color textColor;

    private float bounceFrequency;
    private float bounceAmplitude;

    void Start()
    {
        startPosition = transform.position;

        // Acak posisi awal agar damage text tidak bertumpuk
        randomOffset = new Vector3(Random.Range(-0.3f, 0.3f), 0, Random.Range(-0.3f, 0.3f));
        transform.position += randomOffset;

        // Simpan warna awal untuk fade out
        textColor = textMesh.color;

        // Acak bouncing agar tidak semua teks bergerak sama
        bounceFrequency = Random.Range(bounceFrequencyMin, bounceFrequencyMax);
        bounceAmplitude = Random.Range(bounceAmplitudeMin, bounceAmplitudeMax);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Efek bouncing dengan kurva lebih smooth
        float bounce = Mathf.Sin(elapsedTime * bounceFrequency) * bounceAmplitude * Mathf.Exp(-elapsedTime * 2);

        // Pergerakan ke atas + bouncing
        transform.position = startPosition + new Vector3(randomOffset.x, elapsedTime * moveSpeed + bounce, randomOffset.z);

        // Pastikan damage text selalu menghadap kamera
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0); // Membalik agar text tidak terbalik
        }

        // Fade out dengan SmoothStep agar lebih halus
        textColor.a = Mathf.SmoothStep(1, 0, elapsedTime / lifetime);
        textMesh.color = textColor;

        // Hapus objek setelah animasi selesai
        if (elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float damage)
    {
        textMesh.text = damage.ToString("F0"); // Format angka tanpa desimal
    }

    public void SetColor(Color color)
    {
        textMesh.color = color;
    }
}
