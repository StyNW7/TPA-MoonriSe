using UnityEngine;

public class CloseButton : MonoBehaviour
{

    public GameObject storePanel;

    public void CloseStore()
    {
        storePanel.SetActive(false);
    }

}