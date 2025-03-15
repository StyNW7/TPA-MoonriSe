using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Animator playerAnimator;
    public float interactionDistance = 4f; // Jarak maksimal untuk menebang
    private TreeCutting nearestTree;

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        FindNearestTree(); // Selalu cari pohon terdekat

        // Cek apakah player memiliki Axe dan menekan F
        if (EquipmentManager.Instance.GetEquipmentWeapon() == "Axe" && Input.GetKeyDown(KeyCode.F))
        {
            TryCutTree();
        }
    }

    private void FindNearestTree()
    {
        TreeCutting[] trees = FindObjectsOfType<TreeCutting>();
        float closestDistance = interactionDistance;
        nearestTree = null;

        foreach (TreeCutting tree in trees)
        {
            float distance = Vector3.Distance(transform.position, tree.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                nearestTree = tree;
            }
        }
    }

    private void TryCutTree()
    {
        if (nearestTree == null) return; // Jika tidak ada pohon dalam jarak yang cukup, abaikan
        if (EquipmentManager.Instance.GetEquipmentWeapon() != "Axe") return; // Pastikan menggunakan Axe

        playerAnimator.SetBool("Cut", true);
        StartCoroutine(WaitForAnimation(nearestTree));
    }

    private IEnumerator WaitForAnimation(TreeCutting tree)
    {
        yield return new WaitForSeconds(1f); // Sesuai dengan durasi animasi menebang
        tree.CutTree();
        playerAnimator.SetBool("Cut", false);
    }

}
