using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using Cinemachine;

public class BuilderMode : MonoBehaviour
{

    public Camera builderCamera;
    public GameObject player;
    public GameObject selectionPanel;
    public GameObject costPanel;
    public GameObject errorPanel;

    private bool isBuilderMode = false;

    public ThirdPersonController playerController;
    public PlayerInput playerInput;

    public CinemachineFreeLook freeLookCamera;
    public Camera mainCamera;

    void Start()
    {
        builderCamera.gameObject.SetActive(false);
        playerController = FindObjectOfType<ThirdPersonController>();
        playerInput = FindObjectOfType<PlayerInput>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleBuilderMode();
        }

        if (isBuilderMode)
        {
            MoveCamera();
            ZoomCamera();
        }

        if (freeLookCamera == null)
        {
            GameObject freeLookObj = GameObject.FindGameObjectWithTag("FreeLookCamera");
            if (freeLookObj != null)
            {
                freeLookCamera = freeLookObj.GetComponent<CinemachineFreeLook>();
            }
        }

        if (mainCamera == null) mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    void ToggleBuilderMode()
    {

        isBuilderMode = !isBuilderMode;
        builderCamera.gameObject.SetActive(isBuilderMode);
        player.SetActive(!isBuilderMode);
        selectionPanel.SetActive(isBuilderMode);

        if (isBuilderMode)
        {
            // 🔹 Matikan FreeLook Camera
            if (freeLookCamera != null)
            {
                freeLookCamera.enabled = false;
            }
            mainCamera.gameObject.SetActive(!isBuilderMode);
        }
        else
        {
            // 🔹 Aktifkan kembali FreeLook Camera
            if (freeLookCamera != null)
            {
                freeLookCamera.enabled = true;
            }
            mainCamera.gameObject.SetActive(!isBuilderMode);
        }
        
        // if (isBuilderMode){
        //     // playerInput.cursorInputForLook = false;
        //     Cursor.lockState = CursorLockMode.None; // Bebaskan kursor
        //     Cursor.visible = true; // Tampilkan kursor
        // }

        // else {
        //     // playerInput.cursorInputForLook = true;
        //     Cursor.lockState = CursorLockMode.Locked; // Kunci kembali kursor ke tengah layar
        //     Cursor.visible = false;
        // }

        // if (playerController != null) playerController.enabled = isBuilderMode;
        // if (playerInput != null && isBuilderMode) playerInput.DeactivateInput();
        // else playerInput.ActivateInput();

        costPanel.SetActive(false);
        errorPanel.SetActive(false);

    }

    void MoveCamera()
    {
        float moveSpeed = 5f;
        if (Input.GetKey(KeyCode.W)) builderCamera.transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) builderCamera.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.A)) builderCamera.transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D)) builderCamera.transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }

    void ZoomCamera()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        builderCamera.orthographicSize -= scroll * 2f;
        builderCamera.orthographicSize = Mathf.Clamp(builderCamera.orthographicSize, 5f, 15f);
    }

}
