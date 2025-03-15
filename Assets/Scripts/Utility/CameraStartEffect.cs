using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraStartEffect : MonoBehaviour
{
    public CinemachineVirtualCamera startCamera;
    public float zoomDuration = 2f;
    public float targetFOV = 40f; // Nilai FOV yang lebih kecil untuk zoom in

    void Start()
    {
        if (startCamera != null)
        {
            StartCoroutine(CameraZoomEffect());
        }
    }

    IEnumerator CameraZoomEffect()
    {
        float elapsedTime = 0f;
        float initialFOV = startCamera.m_Lens.FieldOfView;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            startCamera.m_Lens.FieldOfView = Mathf.Lerp(initialFOV, targetFOV, elapsedTime / zoomDuration);
            yield return null;
        }

        startCamera.m_Lens.FieldOfView = targetFOV; // Pastikan nilai akhir benar
        startCamera.Priority = 0;
    }
}
