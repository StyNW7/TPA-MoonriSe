using UnityEngine;

public class UnlockedCursor : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;  // Membebaskan cursor
        Cursor.visible = true;  // Menampilkan cursor
    }

    void Update(){
        Cursor.lockState = CursorLockMode.None;  // Membebaskan cursor
        Cursor.visible = true;  // Menampilkan cursor
    }

}
