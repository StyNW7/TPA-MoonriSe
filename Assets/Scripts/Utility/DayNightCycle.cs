using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour
{

    public Light directionalLight;
    public Material daySkybox;
    public Material nightSkybox;
    public GameObject zombiePrefab; // Prefab Zombie
    public Transform player; // Posisi Player

    public float cycleDuration = 60f;
    private float timeElapsed = 0f;
    [SerializeField] private bool isNight = false;

    private float spawnInterval = 10f; // Spawn setiap 10 detik
    private float spawnChance = 0.14f; // 14% peluang spawn
    private bool spawningAllowed = false; // Mencegah spawn ganda

    void Start()
    {
        StartCoroutine(SpawnZombieCoroutine()); // Mulai coroutine spawn zombie
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float cycle = (timeElapsed % cycleDuration) / cycleDuration; // Nilai antara 0 - 1

        // Rotasi Matahari & Bulan
        directionalLight.transform.rotation = Quaternion.Euler(cycle * 360f - 90f, 170f, 0f);

        if (cycle < 0.5f) // Siang (0 - 0.5)
        {
            isNight = false;
            RenderSettings.skybox = daySkybox;
            directionalLight.intensity = Mathf.Lerp(0.3f, 1f, cycle * 2); // Terang
            spawningAllowed = false; // Matikan spawn zombie saat siang
        }
        else // Malam (0.5 - 1)
        {
            isNight = true;
            RenderSettings.skybox = nightSkybox;
            directionalLight.intensity = Mathf.Lerp(1f, 0.7f, (cycle - 0.5f) * 2); // Redup

            // Aktifkan spawn zombie hanya saat malam
            spawningAllowed = true;
        }

        DynamicGI.UpdateEnvironment();
    }

    IEnumerator SpawnZombieCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Cek apakah saat ini malam & belum ada zombie
            if (isNight && spawningAllowed)
            {
                float randomChance = Random.value; // Nilai antara 0 - 1
                if (randomChance <= 1)
                {
                    Debug.Log("Zombie Muncul!");
                    SpawnZombie();
                }
            }
        }
    }

    void SpawnZombie()
    {
        if (zombiePrefab == null || player == null) return;

        // Tentukan posisi random di sekitar player
        Vector3 spawnPosition = player.position + new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        spawnPosition.y = 0; // Pastikan zombie muncul di tanah

        // Spawn zombie
        Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);
        Debug.Log("Zombie spawned!");
    }

    public bool GetIsNight()
    {
        return isNight;
    }

}
