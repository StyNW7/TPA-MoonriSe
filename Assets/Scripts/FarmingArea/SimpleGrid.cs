using UnityEngine;
using System.Collections.Generic;
using System;
using TMPro;

public class SimpleGrid : MonoBehaviour
{
    public GameObject objectToPlace;
    public float gridSize = 1f;
    private GameObject ghostObject;
    private HashSet<Vector3> occupiedPositions = new HashSet<Vector3>();
    private Quaternion objectRotation = Quaternion.identity;
    private bool isPlacing;
    public GameObject errorPanel;
    public string itemPrefabType;
    public string itemPrefabName;
    public bool cancelCostPanel = false;

    private List<GrowingPlant> growingPlants = new List<GrowingPlant>();
    
    public LayerMask obstacleLayer;
    public LayerMask farmAreaLayer;
    public LayerMask dirtLayer;
    public LayerMask fenceLayer;
    public LayerMask flowerLayer;
    public LayerMask saplingLayer;

    public NPCInteraction2 npc2;
    public TMP_Text errorText;

    public PlayerManager playerManager;
    public bool cancelPlacement;

    public NavMeshUpdater navMeshUpdater;

    public void setCancelCostPanel(bool yes){
        cancelCostPanel = yes;
    }

    public void setCancelPlacement(bool yes){
        cancelPlacement = yes;
    }

    public void Start()
    {
        isPlacing = false;
        npc2 = FindObjectOfType<NPCInteraction2>();
        playerManager = FindObjectOfType<PlayerManager>();
        navMeshUpdater = FindObjectOfType<NavMeshUpdater>();
        cancelPlacement = false;
    }

    private void Update()
    {
        if (isPlacing)
        {
            StartPlacing();
        }
    }

    public void StartPlacing()
    {
        UpdateGhostPosition();
        // if (Input.GetMouseButtonDown(0)){
        //     if (cancelPlacement) {
        //         CancelPlacement();
        //         cancelPlacement = false;
        //         return;
        //     }
        // }
        if (Input.GetMouseButtonDown(0) && IsPlacementValid())
        {
            PlaceObject();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateObject();
        }
    }

    public void SetCalled(GameObject prefab, bool yes, string typeName)
    {
        itemPrefabName = typeName;
        Debug.Log(itemPrefabName);
        if (yes && prefab != null)
        {
            objectToPlace = prefab;
            CreateGhostObject();
            isPlacing = true;

            if (typeName.Contains("Dirt"))
            {
                itemPrefabType = "Dirt";
            }
            else if (typeName.Contains("Sapling"))
            {
                itemPrefabType = "Sapling";
            }
            else if (typeName.Contains("Farm"))
            {
                itemPrefabType = "FarmArea";
            }
            else
            {
                itemPrefabType = "Fence";
            }
        }
        else
        {
            isPlacing = false;
            itemPrefabType = "null";
        }
    }

    private void CreateGhostObject()
    {
        if (ghostObject != null)
        {
            Destroy(ghostObject);
        }

        ghostObject = Instantiate(objectToPlace);
        UpdateGhostPosition();
        DisableColliders(ghostObject);

        Renderer[] renderers = GetAllRenderers(ghostObject);
        if (renderers.Length == 0)
        {
            Debug.LogWarning("Ghost Object tidak memiliki Renderer! Pastikan ada objek anak dengan MeshRenderer.");
            return;
        }

        foreach (Renderer rend in renderers)
        {
            Material mat = new Material(rend.material);
            Color color = mat.color;
            color.a = 0.5f;
            mat.color = color;
            rend.material = mat;
        }

        AdjustGhostPosition();
    }

    private void UpdateGhostPosition()
    {
        if (ghostObject == null) return;

        Camera cam = Camera.main ?? GameObject.FindWithTag("BuilderCamera")?.GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("No active camera found! Make sure a camera exists with the tag 'MainCamera' or 'BuilderCamera'.");
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 snappedPosition = new Vector3(
                Mathf.Round(hit.point.x / gridSize) * gridSize,
                Mathf.Round(hit.point.y / gridSize) * gridSize,
                Mathf.Round(hit.point.z / gridSize) * gridSize
            );

            ghostObject.transform.position = snappedPosition;

            if (!IsPlacementValid())
            {
                SetGhostColor(Color.red);
                errorPanel.SetActive(true);
                errorPanel.transform.position = Input.mousePosition + new Vector3(50, -50, 0);
            }
            else
            {
                errorPanel.SetActive(false);
                SetGhostColor(new Color(1f, 1f, 1f, 0.5f));
            }
        }
    }

    private void AdjustGhostPosition()
    {
        if (itemPrefabType == "Fence")
        {
            Renderer[] renderers = GetAllRenderers(ghostObject);
            if (renderers.Length > 0)
            {
                Bounds bounds = renderers[0].bounds;
                foreach (Renderer rend in renderers)
                {
                    bounds.Encapsulate(rend.bounds);
                }
                Vector3 centerOffset = bounds.center - ghostObject.transform.position;
                ghostObject.transform.position -= centerOffset;
            }
        }
        UpdateGhostPosition();
    }

    


    private bool IsPlacementValid()
    {
        Vector3 placementPosition = ghostObject.transform.position;

        if (itemPrefabType == "FarmArea")
        {
            bool collidesWithFarmArea = Physics.CheckBox(placementPosition, new Vector3(gridSize, 1f, gridSize), Quaternion.identity, farmAreaLayer);
            bool collidesWithObstacle = Physics.CheckBox(placementPosition, new Vector3(gridSize, 1f, gridSize), Quaternion.identity, obstacleLayer);
            bool collidesWithFence = Physics.CheckBox(placementPosition, new Vector3(gridSize, 1f, gridSize), Quaternion.identity, fenceLayer);

            if (collidesWithFarmArea)
            {
                errorText.text = "Cannot place Farm Area: Overlaps with another Farm Area!";
                SetGhostColor(Color.red);
                return false;
            }
            if (collidesWithObstacle)
            {
                errorText.text = "Cannot place Farm Area: Obstacle is in the way!";
                SetGhostColor(Color.red);
                return false;
            }
            if (collidesWithFence)
            {
                errorText.text = "Cannot place Farm Area: Collides with Fence!";
                SetGhostColor(Color.red);
                return false;
            }

            // Validasi Wood sebelum menempatkan Farm
            bool hasEnoughWood = true;
            if (itemPrefabName.Contains("Small")) hasEnoughWood = npc2.HasEnoughWood(50);
            else if (itemPrefabName.Contains("Medium")) hasEnoughWood = npc2.HasEnoughWood(100);
            else if (itemPrefabName.Contains("Large")) hasEnoughWood = npc2.HasEnoughWood(150);

            if (!hasEnoughWood)
            {
                errorText.text = "Not enough Wood!";
                SetGhostColor(Color.red);
                return false;
            }

            SetGhostColor(Color.white);
            return true;
        }

        if (itemPrefabType == "Dirt")
        {
            bool insideFarmArea = Physics.CheckBox(placementPosition, new Vector3(gridSize * 0.5f, 1f, gridSize * 0.5f), Quaternion.identity, farmAreaLayer);
            bool collidesWithDirt = Physics.CheckBox(placementPosition, new Vector3(gridSize * 0.4f, 1f, gridSize * 0.4f), Quaternion.identity, dirtLayer);
            bool collidesWithFence = Physics.CheckBox(placementPosition, new Vector3(gridSize * 0.4f, 1f, gridSize * 0.4f), Quaternion.identity, fenceLayer);

            if (!insideFarmArea)
            {
                errorText.text = "Dirt must be placed inside a Farm Area!";
                SetGhostColor(Color.red);
                return false;
            }
            if (collidesWithDirt)
            {
                errorText.text = "Dirt already exists in this location!";
                SetGhostColor(Color.red);
                return false;
            }
            if (collidesWithFence)
            {
                errorText.text = "Dirt cannot be placed on a Fence!";
                SetGhostColor(Color.red);
                return false;
            }

            SetGhostColor(Color.white);
            return true;
        }

        if (itemPrefabType == "Sapling")
        {
            bool onDirt = Physics.CheckBox(placementPosition, new Vector3(gridSize * 0.5f, 1f, gridSize * 0.5f), Quaternion.identity, dirtLayer);
            bool collidesWithSapling = Physics.CheckBox(placementPosition, new Vector3(gridSize * 0.4f, 1f, gridSize * 0.4f), Quaternion.identity, saplingLayer);

            if (!onDirt)
            {
                errorText.text = "Sapling must be placed on Dirt!";
                SetGhostColor(Color.red);
                return false;
            }
            if (collidesWithSapling)
            {
                errorText.text = "A Sapling is already planted here!";
                SetGhostColor(Color.red);
                return false;
            }

            // Validasi jumlah Sapling
            bool hasEnoughSapling = true;
            if (itemPrefabName.Contains("Tomato")) hasEnoughSapling = npc2.HasSaplings("Tomato Sapling");
            else if (itemPrefabName.Contains("Berries")) hasEnoughSapling = npc2.HasSaplings("Berries Sapling");
            else if (itemPrefabName.Contains("Bamboo")) hasEnoughSapling = npc2.HasSaplings("Bamboo Sapling");

            if (!hasEnoughSapling)
            {
                errorText.text = "Not enough Saplings!";
                SetGhostColor(Color.red);
                return false;
            }

            SetGhostColor(Color.white);
            return true;
        }

        return true;
    }


    private void PlaceObject()
    {

        if (cancelCostPanel == true) {
            cancelCostPanel = false;
            return;
        }

        Vector3 placementPosition = ghostObject.transform.position;
        Quaternion placementRotation = ghostObject.transform.rotation;

        if (!IsPlacementValid()) return;

        // Debug.Log(itemPrefabName);

        // BAYARRRR

        if (itemPrefabName.Contains("Bamboo")){
            npc2.ReduceResource("Bamboo Sapling", 1);
        }

        else if (itemPrefabName.Contains("Berries")){
            npc2.ReduceResource("Berries Sapling", 1);
        }

        else if (itemPrefabName.Contains("Tomato")){
            npc2.ReduceResource("Tomato Sapling", 1);
        }

        else if (itemPrefabName.Contains("Small")){
            npc2.ReduceResource("Wood", 50);
        }

        else if (itemPrefabName.Contains("Medium")){
            npc2.ReduceResource("Wood", 100);
        }

        else if (itemPrefabName.Contains("Large")){
            npc2.ReduceResource("Wood", 200);
        }

        // Jika yang ingin ditempatkan adalah sapling, periksa apakah ada DirtTile di lokasi ini
        if (itemPrefabType == "Sapling")
        {
            Collider[] colliders = Physics.OverlapSphere(placementPosition, gridSize * 0.5f, dirtLayer);
            DirtTile nearestDirt = null;

            foreach (Collider collider in colliders)
            {
                DirtTile dirtTile = collider.GetComponent<DirtTile>();
                if (dirtTile != null)
                {
                    nearestDirt = dirtTile;
                    break;
                }
            }

            if (nearestDirt == null || !nearestDirt.IsEmpty())
            {
                Debug.LogWarning("Tidak bisa menanam sapling di sini! Dirt sudah memiliki tanaman lain.");
                return;
            }

            // Jika Dirt kosong, tanam sapling
            nearestDirt.Plant(objectToPlace.GetComponent<GrowingPlant>());
            growingPlants.Add(objectToPlace.GetComponent<GrowingPlant>());
            return;
        }

        // Jika bukan sapling, tempatkan objek seperti biasa
        GameObject placedObject = Instantiate(objectToPlace, placementPosition, placementRotation);
        occupiedPositions.Add(placementPosition);

        playerManager.GainXP(7);

        if (itemPrefabType == "FarmArea")
        {
            // navMeshUpdater.BakeNavMesh();
            RemoveFlowers(placementPosition);
        }

        UpdateGhostPosition();

    }


    // private void RemoveFlowers(Vector3 position)
    // {
    //     Debug.Log("Try remove flowers");
    //     Collider[] colliders = Physics.OverlapSphere(position, gridSize);
    //     foreach (Collider collider in colliders)
    //     {
    //         if (collider.CompareTag("Flower"))
    //         {
    //             Debug.Log("Hilanginnn bungaa");
    //             Destroy(collider.gameObject);
    //         }
    //     }
    // }

    // private void RemoveFlowers(Vector3 position)
    // {
    //     Debug.Log("Try remove flowers");

    //     // Layer yang harus diperiksa (FarmArea dan Fence)
    //     LayerMask farmLayerMask = LayerMask.GetMask("FarmArea", "Fence");

    //     // Ambil semua collider yang bersentuhan dengan posisi FarmArea baru
    //     Collider[] farmColliders = Physics.OverlapSphere(position, gridSize, farmLayerMask);
        
    //     foreach (Collider farmCollider in farmColliders)
    //     {
    //         // Cari bunga di sekitar FarmArea atau Fence yang ditemukan
    //         Collider[] flowerColliders = Physics.OverlapSphere(farmCollider.transform.position, gridSize);
            
    //         foreach (Collider flowerCollider in flowerColliders)
    //         {
    //             if (flowerCollider.CompareTag("Flower"))
    //             {
    //                 Debug.Log("Removing flower: " + flowerCollider.gameObject.name);
    //                 Destroy(flowerCollider.gameObject);
    //             }
    //         }
    //     }
    // }


    private void RemoveFlowers(Vector3 position)
    {
        Debug.Log("Try removing flowers...");

        // Layer yang harus diperiksa (FarmArea dan Fence)
        LayerMask farmLayerMask = LayerMask.GetMask("FarmArea", "Fence");

        // Ambil semua collider yang bersentuhan dengan posisi FarmArea baru
        Collider[] farmColliders = Physics.OverlapSphere(position, gridSize, farmLayerMask);

        int flowerCount = 0;

        foreach (Collider farmCollider in farmColliders)
        {
            Bounds farmBounds = farmCollider.bounds; // Gunakan Bounds untuk area yang lebih luas

            // Ambil semua bunga dalam area farmBounds
            Collider[] flowerColliders = Physics.OverlapBox(farmBounds.center, farmBounds.extents, Quaternion.identity);

            foreach (Collider flowerCollider in flowerColliders)
            {
                if (flowerCollider.CompareTag("Flower"))
                {
                    Debug.Log($"Removing flower: {flowerCollider.gameObject.name}");
                    Destroy(flowerCollider.gameObject);
                    flowerCount++;
                }
            }
        }

        Debug.Log($"Removed {flowerCount} flowers.");
    }



    // private void RemoveFlowers(Vector3 position)
    // {
    //     Debug.Log("Try remove flowers");

    //     LayerMask farmLayerMask = LayerMask.GetMask("FarmArea", "Fence");

    //     // Gunakan Box Collider untuk mendeteksi objek dalam area farm
    //     Collider[] farmColliders = Physics.OverlapBox(position, new Vector3(gridSize, 1f, gridSize), Quaternion.identity, farmLayerMask);

    //     HashSet<Collider> flowersToRemove = new HashSet<Collider>();

    //     foreach (Collider farmCollider in farmColliders)
    //     {
    //         Collider[] flowerColliders = Physics.OverlapSphere(farmCollider.transform.position, gridSize * 1.2f); // Tambahkan sedikit toleransi radius
            
    //         foreach (Collider flowerCollider in flowerColliders)
    //         {
    //             if (flowerCollider.CompareTag("Flower"))
    //             {
    //                 flowersToRemove.Add(flowerCollider); // Tambahkan ke HashSet agar tidak ada duplikasi
    //             }
    //         }
    //     }

    //     // Hapus semua bunga yang terdeteksi
    //     foreach (Collider flower in flowersToRemove)
    //     {
    //         Debug.Log("Removing flower: " + flower.gameObject.name);
    //         Destroy(flower.gameObject);
    //     }
    // }



    private void RotateObject()
    {
        objectRotation *= Quaternion.Euler(0, 45, 0);
        ghostObject.transform.rotation = objectRotation;
    }

    private void SetGhostColor(Color color)
    {
        Renderer[] renderers = GetAllRenderers(ghostObject);
        foreach (Renderer rend in renderers)
        {
            // Gunakan material instance agar tidak mempengaruhi material asli
            Material matInstance = new Material(rend.material);
            matInstance.color = color;
            rend.material = matInstance;
        }
    }


    private void DisableColliders(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private Renderer[] GetAllRenderers(GameObject obj)
    {
        List<Renderer> renderers = new List<Renderer>();
        GetRenderersRecursive(obj.transform, renderers);
        return renderers.ToArray();
    }

    private void GetRenderersRecursive(Transform parent, List<Renderer> renderers)
    {
        foreach (Transform child in parent)
        {
            Renderer rend = child.GetComponent<Renderer>();
            if (rend != null)
            {
                renderers.Add(rend);
            }
            GetRenderersRecursive(child, renderers);
        }
    }

    public void CancelPlacement()
    {
        isPlacing = false;  // Hentikan mode placing
        itemPrefabType = "null";  // Reset tipe objek
        objectToPlace = null;  // Hapus objek yang dipilih
        errorPanel.SetActive(false);
        if (ghostObject != null)
        {
            Destroy(ghostObject);  // Hapus ghost preview
        }
    }

}
