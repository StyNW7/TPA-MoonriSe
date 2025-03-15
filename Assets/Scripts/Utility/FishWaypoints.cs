using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    public float speed = 2f;  // Kecepatan ikan
    public float rotationSpeed = 5f; // Kecepatan rotasi ikan

    [Header("Waypoints")]
    public Transform[] waypoints;  // Array untuk menyimpan titik tujuan
    private int currentWaypointIndex = 0; // Indeks waypoint yang sedang dituju

    void Update()
    {
        if (waypoints.Length == 0) return; // Jika tidak ada waypoint, keluar dari fungsi

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex]; // Ambil target waypoint saat ini

        // Gerakkan ikan menuju waypoint menggunakan Lerp agar pergerakan halus
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotasi ikan agar menghadap ke arah target
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Jika sudah dekat dengan waypoint, ganti ke waypoint berikutnya
        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // Loop waypoint secara berulang
        }
    }

}
