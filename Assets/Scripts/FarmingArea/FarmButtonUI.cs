using UnityEngine;
using UnityEngine.UI;

public class FarmButtonUI : MonoBehaviour
{

    public RawImage buttonImage;
    public PlacementSystem placementSystem;
    public SimpleGrid farmAreaGrid;
    public ObjectPlacer objectPlacer;
    private FarmButtons farmData;
    private CostPanel costPanel;

    private void Start()
    {
        farmAreaGrid = FindObjectOfType<SimpleGrid>();
        objectPlacer = FindObjectOfType<ObjectPlacer>();
    }

    public void SetupButton(FarmButtons farmButton, CostPanel costPanel)
    {
        this.farmData = farmButton;
        this.costPanel = costPanel;

        buttonImage.texture = farmButton.farmTexture;

        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        // Debug.Log(Input.mousePosition);
        costPanel.ShowCostPanel(farmData);
        // placementSystem.StartPlacing(farmData.farmPrefab);
        farmAreaGrid.SetCalled(farmData.farmPrefab ,true, farmData.farmName);
        // farmAreaGrid.StartPlacing(farmData.farmPrefab);
        // objectPlacer.SelectObjectPrefab(farmData.farmPrefab);
    }

    public void OnClick()
    {
        
    }

}

