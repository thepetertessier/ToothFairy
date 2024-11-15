using UnityEngine;

public class GameQuitter : MonoBehaviour
{
    // This function will be called when the Quit button is clicked
    public void QuitGame()
    {
        #if UNITY_EDITOR
        // If we're running in the Unity editor
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        // If we're running as a standalone build
        Application.Quit();
        #endif
    }
}
