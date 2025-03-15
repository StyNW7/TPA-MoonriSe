using UnityEngine;
using UnityEngine.UI;

public class PlacementSystem : MonoBehaviour
{
    public GridSystem gridSystem;
    public GameObject errorPanel;
    public Transform farmingArea;
    public LayerMask collisionLayer;

    private GameObject previewObject;
    private GameObject selectedObject;
    private bool isPlacing = false;
    private Vector3 lastValidPosition;
    private Quaternion objectRotation = Quaternion.identity;
    private bool isPlacementValid = false;

    void Update()
    {
        if (!isPlacing || previewObject == null) return;

        UpdatePreviewPosition();

        if (Input.GetMouseButtonDown(0) && isPlacementValid) // Klik kiri untuk menempatkan
        {
            PlaceObject();
        }
        else if (Input.GetKeyDown(KeyCode.R)) // Rotasi 45°
        {
            RotateObject();
        }
        else if (Input.GetKeyDown(KeyCode.X)) // Batal
        {
            CancelPlacement();
        }
    }

    public void StartPlacing(GameObject objectPrefab)
    {
        if (previewObject != null) Destroy(previewObject);

        previewObject = Instantiate(objectPrefab);
        previewObject.GetComponent<Collider>().enabled = false;
        SetPreviewMaterial(previewObject, new Color(1, 1, 1, 1f)); // Bayangan transparan
        isPlacing = true;
    }

    private void UpdatePreviewPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snappedPosition = gridSystem.GetSnappedPosition(hit.point);
            isPlacementValid = IsPlacementValid(snappedPosition);

            if (isPlacementValid)
            {
                lastValidPosition = snappedPosition;
                previewObject.transform.position = lastValidPosition;
                previewObject.transform.rotation = objectRotation;
                SetPreviewMaterial(previewObject, new Color(1, 1, 1, 0.5f)); // Warna normal
                errorPanel.SetActive(false);
            }
            else
            {
                SetPreviewMaterial(previewObject, new Color(1, 0, 0, 0.5f)); // Warna merah jika tidak valid
                errorPanel.SetActive(true);
                errorPanel.transform.position = Input.mousePosition + new Vector3(50, -50, 0);
            }
        }
    }

    private bool IsPlacementValid(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, new Vector3(0.5f, 0.5f, 0.5f), objectRotation, collisionLayer);
        return colliders.Length == 0 && gridSystem.IsPositionInsideGrid(position);
    }

    private void PlaceObject()
    {
        if (!isPlacementValid) return;

        selectedObject = Instantiate(previewObject, lastValidPosition, objectRotation);
        selectedObject.GetComponent<Collider>().enabled = true;
        previewObject.SetActive(false);
        isPlacing = false;
    }

    private void RotateObject()
    {
        objectRotation *= Quaternion.Euler(0, 45, 0);
        previewObject.transform.rotation = objectRotation;
    }

    private void CancelPlacement()
    {
        if (previewObject != null) Destroy(previewObject);
        isPlacing = false;
        errorPanel.SetActive(false);
    }

    private void SetPreviewMaterial(GameObject obj, Color color)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            rend.material.color = color;
        }
    }

}
