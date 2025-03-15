// #if UNITY_EDITOR
// using UnityEditor;
// #endif
// using UnityEngine;

// public class GameStopHandler : MonoBehaviour
// {
// #if UNITY_EDITOR
//     private void OnEnable()
//     {
//         EditorApplication.playModeStateChanged += OnPlayModeChanged;
//     }

//     private void OnDisable()
//     {
//         EditorApplication.playModeStateChanged -= OnPlayModeChanged;
//     }

//     private void OnPlayModeChanged(PlayModeStateChange state)
//     {
//         if (state == PlayModeStateChange.ExitingPlayMode)
//         {
//             Debug.Log("Game stopped in editor, deleting PlayerPrefs...");
//             PlayerPrefs.DeleteKey("HasPlayedBefore");
//             PlayerPrefs.Save();
//         }
//     }
// #endif

//     private void OnApplicationQuit()
//     {
//         Debug.Log("Game is quitting, deleting PlayerPrefs...");
//         PlayerPrefs.DeleteKey("HasPlayedBefore");
//         PlayerPrefs.Save();
//     }
// }
