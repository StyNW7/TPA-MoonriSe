using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class PlayerAttackZombie : MonoBehaviour
{
    private Animator playerAnimator;
    public float attackDistance = 2f; // Jarak maksimal untuk menyerang zombie
    public int swordDamage = 20; // Damage per hit
    private ZombieAI nearestZombie;

    private int attackCombo = 0; // Untuk tracking combo (0 atau 1)
    private bool canAttack = true; // Untuk memastikan pemain tidak menyerang terlalu cepat
    private float comboResetTime = 1.5f; // Waktu reset combo jika tidak menekan attack lagi

    public PlayerManager playerManager;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerInput = FindObjectOfType<PlayerInput>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        FindNearestZombie(); // Cari zombie terdekat setiap frame

        // Jika player memiliki sword dan menekan tombol serangan
        if (EquipmentManager.Instance.GetEquipmentWeapon() == "Sword" && Input.GetMouseButtonDown(0) && canAttack)
        {
            if (playerController != null) playerController.enabled = false;
            if (playerInput != null) playerInput.DeactivateInput();
            StopAllCoroutines(); // Hentikan timer reset combo jika ada serangan baru
            StartCoroutine(SwordAttackZombie());
        }
        else {
            if (playerController != null) playerController.enabled = true;
            if (playerInput != null) playerInput.ActivateInput();
        }
    }

    private void FindNearestZombie()
    {
        ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
        float closestDistance = attackDistance;
        nearestZombie = null;

        foreach (ZombieAI zombie in zombies)
        {
            float distance = Vector3.Distance(transform.position, zombie.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                nearestZombie = zombie;
            }
        }
    }

    private IEnumerator SwordAttackZombie()
    {
        // if (nearestZombie == null) yield break;
        // if (EquipmentManager.Instance.GetEquipmentWeapon() != "Sword") yield break;

        canAttack = false; // Mencegah spam attack

        // Gunakan AttackIndex untuk menentukan animasi
        playerAnimator.SetInteger("AttackIndex", attackCombo);
        playerAnimator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(0.5f); // Tunggu animasi hit pertama

        // Berikan damage
        if (nearestZombie != null) nearestZombie.TakeDamage(swordDamage);

        yield return new WaitForSeconds(0.3f); // Beri delay untuk combo berikutnya
        playerAnimator.SetBool("isAttacking", false);

        // Pindah ke attack combo selanjutnya
        attackCombo = (attackCombo + 1) % 2;

        yield return new WaitForSeconds(0.3f); // Delay sebelum bisa menyerang lagi
        canAttack = true;

        // Mulai timer reset combo jika tidak ada input tambahan
        StartCoroutine(ResetComboAfterDelay());
    }

    private IEnumerator ResetComboAfterDelay()
    {
        yield return new WaitForSeconds(comboResetTime);
        attackCombo = 0;
        playerAnimator.SetInteger("AttackIndex", 0);
    }

    // Fungsi untuk dipanggil dari Animation Event
    public void ResetToIdle()
    {
        attackCombo = 0;
        playerAnimator.SetBool("isAttacking", false);
        playerAnimator.SetInteger("AttackIndex", 0);
    }

}



// using System.Collections;
// using UnityEngine;

// public class PlayerAttackZombie : MonoBehaviour
// {
//     private Animator playerAnimator;
//     public float attackDistance = 2f; // Jarak maksimal untuk menyerang zombie
//     public int swordDamage = 20; // Damage per hit
//     private ZombieAI nearestZombie;

//     private int attackCombo = 0; // Untuk tracking combo (0 atau 1)
//     private bool canAttack = true; // Untuk memastikan pemain tidak menyerang terlalu cepat

//     private void Start()
//     {
//         playerAnimator = GetComponent<Animator>();
//     }

//     private void Update()
//     {
//         FindNearestZombie(); // Cari zombie terdekat setiap frame

//         // Jika player memiliki sword dan menekan Q
//         if (EquipmentManager.Instance.GetEquipmentWeapon() == "Sword" && Input.GetMouseButtonDown(0) && canAttack)
//         {
//             StartCoroutine(SwordAttackZombie());
//         }
//         // else if (EquipmentManager.Instance.GetEquipmentWeapon() == "Sword" && Input.GetMouseButtonDown(0)){
//         //     StartCoroutine(SwordAttackZombie());
//         // }
//     }

//     private void FindNearestZombie()
//     {
//         ZombieAI[] zombies = FindObjectsOfType<ZombieAI>();
//         float closestDistance = attackDistance;
//         nearestZombie = null;

//         foreach (ZombieAI zombie in zombies)
//         {
//             float distance = Vector3.Distance(transform.position, zombie.transform.position);
//             if (distance <= closestDistance)
//             {
//                 closestDistance = distance;
//                 nearestZombie = zombie;
//             }
//         }
//     }

//     private IEnumerator SwordAttackZombie()
//     {
//         if (nearestZombie == null) yield break; // Tidak ada zombie di dekatnya
//         if (EquipmentManager.Instance.GetEquipmentWeapon() != "Sword") yield break;

//         canAttack = false; // Mencegah spam attack

//         // Gunakan AttackIndex untuk menentukan animasi
//         playerAnimator.SetInteger("AttackIndex", attackCombo);
//         playerAnimator.SetBool("isAttacking", true);

//         yield return new WaitForSeconds(0.5f); // Tunggu animasi hit pertama

//         // Berikan damage
//         nearestZombie.TakeDamage(swordDamage);

//         yield return new WaitForSeconds(0.3f); // Beri delay untuk combo berikutnya
//         playerAnimator.SetBool("isAttacking", false);

//         // Pindah ke attack combo selanjutnya
//         attackCombo = (attackCombo + 1) % 2;

//         yield return new WaitForSeconds(0.3f); // Delay sebelum bisa menyerang lagi
//         canAttack = true;
//     }


//     public void ResetToIdle()
//     {
//         attackCombo = 0;
//         playerAnimator.SetBool("isAttacking", false);
//         playerAnimator.SetInteger("AttackIndex", 0);
//     }


// }