using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{

    public static InteractionManager Instance { get; private set; }

    public GameObject interactionPanel;
    public TMP_Text interactionText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    public void ShowInteractionPanel(Vector3 position, string message)
    {
        if (interactionPanel != null)
        {
            //interactionPanel.transform.position = Camera.main.WorldToScreenPoint(position + Vector3.up * 1.5f);
            interactionPanel.SetActive(true);
            if (interactionText != null)
                interactionText.text = message;
        }
    }

    public void HideInteractionPanel()
    {
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

}
