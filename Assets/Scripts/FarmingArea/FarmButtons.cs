using UnityEngine;

[CreateAssetMenu(fileName = "FarmButtons", menuName = "FarmSelectionPanel/FarmButton")]
public class FarmButtons : ScriptableObject
{
    public string farmName;
    public string farmCostType;
    public int farmCostValue;
    public Texture farmTexture;
    public int quantity = 0;
    public GameObject farmPrefab;
}
