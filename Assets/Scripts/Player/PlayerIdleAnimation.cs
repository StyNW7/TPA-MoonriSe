using UnityEngine;

public class PlayerIdleAnimation : MonoBehaviour
{

    private Animator animator;
    private float idleTimer = 0f;
    private float idleThreshold = 15f; // Waktu idle sebelum animasi unik dipicu
    private bool isIdleTriggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Cek apakah player sedang bergerak atau tidak
        bool isMoving = Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0;

        if (isMoving)
        {
            ResetIdleTimer();
        }
        else
        {
            idleTimer += Time.deltaTime;

            // Jika idle lebih dari 15 detik dan belum memicu animasi unik
            if (idleTimer >= idleThreshold && !isIdleTriggered)
            {
                TriggerIdleAnimation();
            }
        }
    }

    void ResetIdleTimer()
    {
        idleTimer = 0f;
        isIdleTriggered = false;
        animator.SetBool("isIdleSpecial", false); // Reset kondisi animasi unik
    }

    void TriggerIdleAnimation()
    {

        isIdleTriggered = true;

        // Pilih salah satu dari dua animasi idle spesial secara acak

        animator.SetBool("isIdleSpecial", true);

        int randomIdle = Random.Range(0, 2);

        if (randomIdle == 0)
        {
            Debug.Log("Player look around");
            animator.SetTrigger("LookAround"); // Animasi melihat sekeliling
        }
        else
        {
            Debug.Log("Player backpain");
            animator.SetTrigger("BackPain"); // Animasi pegang punggung
        }

        
    }

}
