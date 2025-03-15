using UnityEngine;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour
{

    public static EquipmentManager Instance;
    public GameObject smokeEffectPrefab;
    public Transform playerTransform;
    private string equippedItem;

    // PlayerEquipment
    public GameObject bowEquipment;
    public GameObject axeEquipment;
    public GameObject fishingRodEquipment;
    public GameObject swordEquipment;
    public GameObject arrowEquipment;

    public RawImage[] usePanelItem;
    public Texture okTexture;
    public Texture cancelTexture;

    private bool useTheSameEquipment = false;
    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        UnequipEverything();
        equippedItem = "NULL";
    }

    public void EquipItem(string itemName)
    {
        // Buat efek asap saat mengganti senjata
        GameObject smokeInstance = Instantiate(smokeEffectPrefab, playerTransform.position, Quaternion.identity);
        Destroy(smokeInstance, 1.5f);

        // Cek apakah item yang sama sedang dipakai, jika ya maka unequip
        if (equippedItem == itemName)
        {
            UnequipEverything();
            ResetUsePanel();
            equippedItem = "NULL"; // Set sebagai tidak ada equip
            return;
        }

        // Jika item berbeda, unequip semua dulu
        UnequipEverything();
        ResetUsePanel();

        // Aktifkan senjata sesuai dengan item yang dipilih
        equippedItem = itemName;

        switch (itemName)
        {
            case "Axe":
                axeEquipment.SetActive(true);
                usePanelItem[2].texture = okTexture;
                break;
            case "Fishing Rod":
                fishingRodEquipment.SetActive(true);
                usePanelItem[1].texture = okTexture;
                break;
            case "Sword":
                swordEquipment.SetActive(true);
                usePanelItem[0].texture = okTexture;
                break;
            case "Bow":
                bowEquipment.SetActive(true);
                arrowEquipment.SetActive(true);
                usePanelItem[3].texture = okTexture;
                break;
        }
    }


    public void ResetUsePanel()
    {
        usePanelItem[0].texture = cancelTexture;
        usePanelItem[1].texture = cancelTexture;
        usePanelItem[2].texture = cancelTexture;
        usePanelItem[3].texture = cancelTexture;
    }

    public void UnequipEverything()
    {
        bowEquipment.SetActive(false);
        axeEquipment.SetActive(false);
        fishingRodEquipment.SetActive(false);
        swordEquipment.SetActive(false);
        arrowEquipment.SetActive(false);
    }

    public string GetEquipmentWeapon() { return equippedItem; }

}
