using UnityEngine;
using Cinemachine;

public class FreeLookCamera : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;
    public Transform playerTransform;

    [Header("Camera Settings")]
    public float cameraRotationSpeed = 200f;
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 1.5f;
    public bool invertYAxis = false;

    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;
    private bool isCameraLocked = false;

    void Start()
    {
        if (freeLookCamera == null)
        {
            freeLookCamera = GetComponent<CinemachineFreeLook>();
        }

        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.m_InputAxisName = "";
            freeLookCamera.m_YAxis.m_InputAxisName = "";
        }
    }

    void Update()
    {
        if (!isCameraLocked)
        {
            HandleMouseCameraRotation();
            AlignCameraWithPlayer();
        }
    }

    void HandleMouseCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * (invertYAxis ? -1 : 1);

        currentCameraRotationX += mouseX;
        currentCameraRotationY -= mouseY;
        currentCameraRotationY = Mathf.Clamp(currentCameraRotationY, -40f, 70f);

        if (freeLookCamera != null)
        {
            freeLookCamera.m_XAxis.Value = currentCameraRotationX;
            freeLookCamera.m_YAxis.Value = Mathf.InverseLerp(-40f, 70f, currentCameraRotationY);
        }
    }

    void AlignCameraWithPlayer()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Bisa diganti dengan tombol tengah mouse
        {
            currentCameraRotationX = playerTransform.eulerAngles.y;
            if (freeLookCamera != null)
            {
                freeLookCamera.m_XAxis.Value = currentCameraRotationX;
            }
        }
    }

    // 🔹 Fungsi untuk mengunci/membuka kamera
    public void LockCamera(bool state)
    {
        isCameraLocked = state;
    }

}
