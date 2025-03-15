using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false; // Mencegah panah terus bergerak setelah mengenai objek
    private int arrowDamage = 25; // Damage default ke zombie

    [SerializeField] private LayerMask zombieLayer; // Layer khusus untuk zombie
    [SerializeField] private float destroyTime = 10f; // Waktu sebelum panah hancur

    private ZombieAI nearestZombie;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; // Mencegah bug physics
        Destroy(gameObject, destroyTime); // Hancurkan panah setelah 10 detik jika tidak mengenai objek
    }

    // private void FindNearestZombie()
    // {
    //     ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
    //     float closestDistance = attackDistance;
    //     nearestZombie = null;

    //     foreach (ZombieAI zombie in zombies)
    //     {
    //         float distance = Vector3.Distance(transform.position, zombie.transform.position);
    //         if (distance <= closestDistance)
    //         {
    //             closestDistance = distance;
    //             nearestZombie = zombie;
    //         }
    //     }
    // }

    private void FixedUpdate()
    {
        if (!hasHit)
        {
            // Pastikan panah tetap menghadap arah pergerakan
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return; // Pastikan hanya satu kali hit

    
        if (other.GetComponent<ZombieAI>() != null) // Jika objek memiliki komponen ZombieAI
        {
            StickToZombie(other);
        }
        else
        {
            Debug.Log("Arrow terkena: " + other.gameObject.name);
            StopArrow();
        }
    }


    private void StickToZombie(Collider zombie)
    {
        hasHit = true;
        rb.isKinematic = true; // Matikan physics agar tidak memantul
        rb.linearVelocity = Vector3.zero; // Hentikan kecepatan panah
        
        // Atur posisi dan rotasi agar menempel dengan baik
        transform.SetParent(zombie.transform);
        transform.position = zombie.ClosestPoint(transform.position); // Menempel ke titik terdekat
        transform.rotation = Quaternion.LookRotation(-zombie.transform.forward); // Sesuaikan arah panah

        // transform.SetParent(null);

        Debug.Log("Zombie terkena damage: " + zombie.gameObject);
        ApplyDamage(zombie.gameObject); // Berikan damage ke zombie
    }

    private void StopArrow()
    {
        hasHit = true;
        rb.isKinematic = true; // Matikan physics agar panah tidak jatuh
        rb.linearVelocity = Vector3.zero;
        Destroy(gameObject, destroyTime); // Hancurkan panah setelah waktu tertentu
    }

    private void ApplyDamage(GameObject target)
    {
        ZombieAI zombieHealth = target.GetComponent<ZombieAI>();
        if (zombieHealth != null)
        {
            zombieHealth.TakeDamage(arrowDamage);
        }
    }

}
