using UnityEngine;

public class ZombieIdleAnimation : MonoBehaviour
{

    private Animator animator;
    private float idleTimer = 0f;
    private float idleThreshold = 15f; // Waktu idle sebelum animasi unik dipicu
    private bool isIdleTriggered = false;
    public ZombieAI zombieAI;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
        bool isIdle = zombieAI.GetZombieIdle();

        if (isIdle != true)
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
        Debug.Log("Zombie Screaming di Idle Animation");
        animator.SetBool("isIdleSpecial", true);

    }

}
