using UnityEngine;

public class PlayerPlantSystem : MonoBehaviour
{
    public GrowingPlant[] plantPrefabs; // Tomato, Berries, Bamboo
    private int selectedPlantIndex = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedPlantIndex = 0; // Pilih Tomato
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedPlantIndex = 1; // Pilih Berries
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedPlantIndex = 2; // Pilih Bamboo

        if (Input.GetKeyDown(KeyCode.E))
        {
            DirtTile nearestDirt = SimpleFarmGrid.Instance.GetNearestDirt(transform.position, 2f);
            if (nearestDirt != null)
            {
                nearestDirt.Plant(plantPrefabs[selectedPlantIndex]);
            }
        }
    }
}
