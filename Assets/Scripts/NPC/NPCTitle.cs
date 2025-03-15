using UnityEngine;

public class NPCTitle : MonoBehaviour
{
    public Transform cameraTransform;

    void Update()
    {
        transform.LookAt(transform.position + cameraTransform.forward);
    }
}
