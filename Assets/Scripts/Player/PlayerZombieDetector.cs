using UnityEngine;

public class PlayerZombieDetector : MonoBehaviour
{

    public float detectionRadius = 15f; // Radius deteksi zombie
    public LayerMask zombieLayer; // Layer untuk filter hanya zombie
    void Update()
    {
        //IsZombieNearby();
    }

    public bool IsZombieNearby()
    {
        // Cari semua objek di dalam radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, zombieLayer);

        // Cek apakah ada zombie dengan script ZombieAI di dalam radius
        foreach (var collider in hitColliders)
        {
            if (collider.GetComponent<ZombieAI>() != null)
            {
                return true; // Jika ada zombie, return true
            }
        }

        return false; // Jika tidak ada zombie, return false
    }

    //void OnDrawGizmos()
    //{
    //    // Visualisasi radius deteksi saat di editor
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, detectionRadius);
    //}

}
