using UnityEngine;
using TMPro;

public class GrowingPanelManager : MonoBehaviour
{
    public static GrowingPanelManager Instance;

    public GameObject interactionPanel;
    public TMP_Text interactionText;

    private Transform currentTarget;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowInteractionUI(Transform target, string message)
    {
        if (interactionPanel != null)
        {
            interactionPanel.SetActive(true);
            interactionText.text = message;
            currentTarget = target;
        }
    }

    public void HideInteractionUI(Transform target)
    {
        if (interactionPanel != null && currentTarget == target)
        {
            interactionPanel.SetActive(false);
            currentTarget = null;
        }
    }
}
