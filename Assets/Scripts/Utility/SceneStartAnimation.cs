using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneStartAnimation : MonoBehaviour
{
    public CanvasGroup fadePanel; // Referensi ke FadePanel UI
    public float fadeDuration = 2f; // Durasi fade out dalam detik

    void Start()
    {
        fadePanel.gameObject.SetActive(true);
        if (fadePanel != null)
        {
            StartCoroutine(FadeOutEffect());
        }
    }

    IEnumerator FadeOutEffect()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false); // Matikan panel setelah fade out selesai
    }
}
