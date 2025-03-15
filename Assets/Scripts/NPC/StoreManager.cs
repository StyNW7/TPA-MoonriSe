//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;
//using TMPro;

//[System.Serializable]
//public class ToolItem
//{
//    public string toolName;
//    public Sprite toolIcon;
//    public string description;
//    public int price;
//    public bool isOwned;
//}

//public class StoreManager : MonoBehaviour
//{
//    public List<ToolItem> availableTools;
//    public TMP_Text toolNameText, descriptionText, priceText;
//    public Image toolIcon;
//    public Button buyButton;
//    private ToolItem selectedTool;
//    private int playerMoney = 1000; // Contoh saldo awal

//    void Start()
//    {
//        DisplayTools();
//        buyButton.onClick.AddListener(BuyTool);
//    }

//    void DisplayTools()
//    {
//        foreach (ToolItem tool in availableTools)
//        {
//            GameObject toolButton = new GameObject(tool.toolName);
//            toolButton.AddComponent<Button>().onClick.AddListener(() => SelectTool(tool));
//        }
//    }

//    void SelectTool(ToolItem tool)
//    {
//        selectedTool = tool;
//        toolNameText.text = tool.toolName;
//        descriptionText.text = tool.description;
//        priceText.text = tool.price.ToString();
//        toolIcon.sprite = tool.toolIcon;

//        if (tool.isOwned)
//        {
//            buyButton.interactable = false;
//            buyButton.GetComponentInChildren<Text>().text = "Owned";
//        }
//        else
//        {
//            buyButton.interactable = true;
//            buyButton.GetComponentInChildren<Text>().text = "Buy";

//            if (playerMoney >= tool.price)
//                priceText.color = Color.yellow;
//            else
//                priceText.color = Color.red;
//        }
//    }

//    public void BuyTool()
//    {
//        if (selectedTool != null && playerMoney >= selectedTool.price && !selectedTool.isOwned)
//        {
//            playerMoney -= selectedTool.price;
//            selectedTool.isOwned = true;
//            buyButton.GetComponentInChildren<Text>().text = "Owned";
//            buyButton.interactable = false;
//        }
//    }
//}
