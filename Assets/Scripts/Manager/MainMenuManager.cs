using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 3f;

    void Start()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0;
            StartCoroutine(FadeIn());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FadeAndLoadScene("MainScene"));
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        fadeCanvas.alpha = 1;
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        if (fadeCanvas != null)
        {
            float timer = 0f;
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
                yield return null;
            }
            fadeCanvas.alpha = 0;
        }
        SceneManager.LoadScene(sceneName);
    }
}
