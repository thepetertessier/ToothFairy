using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyUI : MonoBehaviour {
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    public void MakeKeyVisible() {
        image.enabled = true;
    }

    private void OnEnable() {
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        // Unsubscribe from the sceneLoaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Awake();
    }
}
