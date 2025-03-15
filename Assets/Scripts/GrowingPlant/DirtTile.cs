using UnityEngine;

public class DirtTile : MonoBehaviour
{

    private GrowingPlant plantedPlant = null;

    private void Start()
    {
        SimpleFarmGrid.Instance.RegisterDirt(this);
    }

    public bool IsEmpty()
    {
        return plantedPlant == null;
    }

    public void Plant(GrowingPlant plantPrefab)
    {
        if (IsEmpty())
        {
            GrowingPlant newPlant = Instantiate(plantPrefab, transform.position, Quaternion.identity);
            plantedPlant = newPlant;
        }
    }

}
