using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{

    public TMP_Text textMesh; // Referensi ke TextMeshPro
    private float moveSpeed = 1.5f;
    private float fadeSpeed = 1f;
    private float lifetime = 1.2f;
    private float bounceHeight = 0.5f;

    private void Start()
    {
        Destroy(gameObject, lifetime); // Hapus pop-up setelah beberapa detik
    }

    public void Setup(float damageAmount, bool isCritical)
    {
        textMesh.text = damageAmount.ToString();
        textMesh.color = isCritical ? Color.yellow : Color.red; // Kuning untuk critical hit
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        transform.LookAt(Camera.main.transform); // Selalu menghadap kamera

        // Efek pantulan
        float bounce = Mathf.Sin(Time.time * 10f) * bounceHeight * Time.deltaTime;
        transform.position += new Vector3(0, bounce, 0);

        // Efek fade-out
        textMesh.alpha -= fadeSpeed * Time.deltaTime;
    }

}
