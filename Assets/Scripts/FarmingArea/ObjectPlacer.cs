using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public GameObject[] placeableObjects;
    public GameObject selectedObjectPrefab;
    private GameObject previewObject;

    void Update()
    {
        if (selectedObjectPrefab != null)
        {
            Vector3 gridPosition = GridSystem.Instance.GetNearestGridPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (previewObject == null)
            {
                previewObject = Instantiate(selectedObjectPrefab, gridPosition, Quaternion.identity);
                previewObject.GetComponent<Renderer>().material.color = new Color(1, 1, 1, 0.5f); // Transparan
            }
            else
            {
                previewObject.transform.position = gridPosition;
            }

            if (Input.GetMouseButtonDown(0))
            {
                // if (PlacementValidator.IsPlacementValid(gridPosition, selectedObjectPrefab))
                // {
                //     Instantiate(selectedObjectPrefab, gridPosition, Quaternion.identity);
                //     InventorySystem.Instance.DeductResource("Wood", 50); // Contoh biaya
                // }
                // else
                // {
                //     ErrorPanel errorPanel = FindObjectOfType<ErrorPanel>();
                //     errorPanel.ShowError("Placement not valid!", Input.mousePosition);
                // }
                Instantiate(selectedObjectPrefab, gridPosition, Quaternion.identity);
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && previewObject != null)
        {
            previewObject.transform.Rotate(0, 45, 0);
        }
    }

    public void SelectObject(int index)
    {
        selectedObjectPrefab = placeableObjects[index];
    }

    public void SelectObjectPrefab(GameObject prefab)
    {
        selectedObjectPrefab = prefab;
    }

}
