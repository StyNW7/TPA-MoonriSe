using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform player;       // Target player
    public Vector3 offset = new Vector3(0, 2, -5); // Offset default kamera
    public float smoothSpeed = 10f; // Kecepatan lerp kamera
    public LayerMask obstacleMask;  // Layer objek yang bisa menghalangi

    private Vector3 currentOffset;  // Offset kamera saat ini

    private void Start()
    {
        currentOffset = offset; // Set offset awal
    }

    private void LateUpdate()
    {
        if (player == null) return;

        Vector3 desiredPosition = player.position + offset; // Posisi kamera seharusnya
        Vector3 direction = desiredPosition - player.position; // Arah dari player ke kamera
        RaycastHit hit;

        // Cek apakah ada objek di antara kamera dan player
        if (Physics.Raycast(player.position, direction.normalized, out hit, offset.magnitude, obstacleMask))
        {
            // Kamera berpindah lebih dekat ke player untuk menghindari objek
            float hitDistance = hit.distance * 0.9f; // Sedikit buffer agar tidak menempel ke objek
            currentOffset = direction.normalized * hitDistance;
        }
        else
        {
            // Tidak ada objek yang menghalangi, kembalikan ke offset normal
            currentOffset = Vector3.Lerp(currentOffset, offset, Time.deltaTime * smoothSpeed);
        }

        // Update posisi kamera dengan offset terbaru
        transform.position = player.position + currentOffset;
        transform.LookAt(player.position); // Kamera selalu menghadap player
    }
}
