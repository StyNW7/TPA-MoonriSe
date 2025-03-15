using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CostPanel : MonoBehaviour
{

    public TMP_Text itemNameText;
    public TMP_Text costText;
    public TMP_Text costTypeText;
    public RawImage itemIcon;

    public Button closeCostPanel;

    public FarmButtons activateButton;
    public SimpleGrid SimpleGrid;

    private void Start()
    {
        gameObject.SetActive(false); // Sembunyikan saat awal
        activateButton = null;
        closeCostPanel.onClick.AddListener(CloseCostPanel);
    }

    public void ShowCostPanel(FarmButtons farmButton)
    {
        activateButton = farmButton;
        itemNameText.text = farmButton.farmName;
        costTypeText.text = farmButton.farmCostType;
        costText.text = farmButton.farmCostValue.ToString();
        itemIcon.texture = farmButton.farmTexture;

        gameObject.SetActive(true); // Tampilkan saat item dipilih
    }

    public void CloseCostPanel()
    {
        gameObject.SetActive(false);
        activateButton = null;
        // SimpleGrid.SetCalled(null, false, "null");
        SimpleGrid.setCancelCostPanel(true);
        SimpleGrid.setCancelPlacement(true);
        if (SimpleGrid != null)
        {
            SimpleGrid.CancelPlacement();
        }
    }

}
