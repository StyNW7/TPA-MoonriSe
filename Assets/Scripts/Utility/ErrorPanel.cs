using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ErrorPanel : MonoBehaviour
{
    public GameObject errorPanel;
    public TMP_Text errorText;

    public void ShowError(string message, Vector3 mousePos)
    {
        errorText.text = message;
        errorPanel.transform.position = mousePos + new Vector3(50, -50, 0);
        errorPanel.SetActive(true);
        Invoke("HideError", 0.5f);
    }

    void HideError()
    {
        errorPanel.SetActive(false);
    }
}
