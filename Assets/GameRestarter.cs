using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
    private ToothTracker toothTracker;
    private CameraFollow cameraFollow;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private LevelSelector levelSelector;

    private void Awake() {
        toothTracker = FindAnyObjectByType<ToothTracker>();
        cameraFollow = FindAnyObjectByType<CameraFollow>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
    }
    public void RestartGame() {
        toothTracker.ResetTeethCount();
        levelSelector.LoadFirstLevel();
    }

    void Update() {
        if (toothTracker.GetTeethCount() <= 0) {
            GameOver();
        }
    }

    private void GameOver() {
        // cameraFollow.ResetZoom();
        if (playerMovement != null) {
            playerMovement.enabled = false;
        }
        gameOverScreen.SetActive(true);
    }
}
