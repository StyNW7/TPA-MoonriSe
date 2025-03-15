using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FarmButtons", menuName = "FarmSelectionPanel/FarmButtonList")]
public class FarmButtonTable : ScriptableObject
{
    public List<FarmButtons> farmButtonList;
}
