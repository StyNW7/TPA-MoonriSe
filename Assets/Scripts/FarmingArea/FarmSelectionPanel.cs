using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmSelectionPanel : MonoBehaviour
{
    public FarmButtonTable farmButtonTable; // Referensi ke daftar item
    public GameObject buttonPrefab; // Prefab button untuk item
    public Transform contentPanel; // Panel tempat button ditampilkan
    public CostPanel costPanel; // Referensi ke Cost Panel
    public NPCInteraction2 npc2; // Referensi ke sistem inventory

    private Dictionary<string, GameObject> farmButtons = new Dictionary<string, GameObject>();
    private HashSet<string> saplingNames = new HashSet<string>(); // Menyimpan daftar nama sapling
    private HashSet<string> farmAreaNames = new HashSet<string>(); // Menyimpan daftar nama farm area

    private void Start()
    {
        PopulateSelectionPanel();
    }

    private void PopulateSelectionPanel()
    {
        foreach (FarmButtons farmButton in farmButtonTable.farmButtonList)
        {
            string itemType = farmButton.farmCostType;
            string itemName = farmButton.farmName;

            // Validasi Sapling
            if (itemType.Contains("Sapling"))
            {
                saplingNames.Add(farmButton.farmName);
                if (!npc2.HasSaplings(farmButton.farmName)) continue;
            }

            // Validasi Farm Area (Wood Requirement)
            else if (itemName.Contains("Farm"))
            {

                // Debug.Log("Farm");
                farmAreaNames.Add(farmButton.farmName);
                if (npc2.HasEnoughWood(farmButton.farmCostValue)) continue;

            }

            CreateButton(farmButton);
        }
    }

    private void Update()
    {
        // Update Sapling Buttons
        foreach (string saplingName in saplingNames)
        {
            bool shouldShow = npc2.HasSaplings(saplingName);

            if (!farmButtons.ContainsKey(saplingName) && shouldShow)
            {
                FarmButtons farmButton = farmButtonTable.farmButtonList.Find(item => item.farmName == saplingName);
                if (farmButton != null)
                {
                    CreateButton(farmButton);
                }
            }

            if (farmButtons.ContainsKey(saplingName))
            {
                farmButtons[saplingName].SetActive(shouldShow);
            }
        }

        foreach (string farmName in farmAreaNames)
        {
            FarmButtons farmButton = farmButtonTable.farmButtonList.Find(item => item.farmName == farmName);
            bool shouldShow = npc2.HasEnoughWood(farmButton.farmCostValue);

            if (!farmButtons.ContainsKey(farmName) && shouldShow)
            {
                CreateButton(farmButton);
            }

            if (farmButtons.ContainsKey(farmName))
            {
                farmButtons[farmName].SetActive(shouldShow);
            }
        }

    }

    private void CreateButton(FarmButtons farmButton)
    {
        GameObject newButton = Instantiate(buttonPrefab, contentPanel);
        newButton.GetComponent<FarmButtonUI>().SetupButton(farmButton, costPanel);
        farmButtons[farmButton.farmName] = newButton;
    }
}
