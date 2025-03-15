using UnityEngine;

public class LootBehavior : MonoBehaviour
{

    private Transform player;
    private bool isFollowing = false;
    private float speed = 5f;
    private float followDistance = 3f;
    private LootItemSO lootItem; // Loot Item berdasarkan Scriptable Object
    private int lootAmount;
    private TrailRenderer trailRenderer;

    public NPCInteraction2 npcResource;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        npcResource = FindObjectOfType<NPCInteraction2>();
        trailRenderer = GetComponent<TrailRenderer>();

        if (trailRenderer == null)
        {
            trailRenderer = gameObject.AddComponent<TrailRenderer>();
            ConfigureTrailEffect(trailRenderer);
        }

        Invoke("EnableFollow", 1f); // Delay sebelum loot mulai mengikuti player
    }

    //public void SetLootItem(LootItemSO item)
    //{
    //    lootItem = item;
    //    lootAmount = Random.Range(item.minAmount, item.maxAmount + 1);
    //}

    public void SetLootItem(LootItemSO item)
    {
        lootItem = item;
        lootAmount = Random.Range(item.minAmount, item.maxAmount+1);
        Invoke("StartFollowing", 0.5f);
    }

    private void EnableFollow()
    {
        isFollowing = true;
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            if (distance < followDistance)
            {
                transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime * speed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (lootItem == null)
            {
                Debug.LogError("LootItem is null! Make sure SetLootItem() is called before the loot is collected.");
                return;
            }

            if (lootItem.itemName.ToLower().Contains("experience"))
            {
                Debug.Log("This is Loot Experience");
                PlayerManager.Instance.GainXP(lootAmount);
            }
            else
            {
                Debug.Log("You got Resource Item: " + lootItem.itemName );
                npcResource.AddResource(lootItem.itemName, lootAmount);
            }

            Destroy(gameObject); // Hancurkan loot saat menyentuh player
        }
    }

    private void ConfigureTrailEffect(TrailRenderer trail)
    {
        trail.time = 0.5f;
        trail.startWidth = 0.2f;
        trail.endWidth = 0f;
        //trail.material = new Material(Shader.Find("Particles/Standard Unlit"));
        trail.startColor = Color.yellow;
        trail.endColor = new Color(1f, 1f, 0f, 0f); // Warna memudar
    }

}
