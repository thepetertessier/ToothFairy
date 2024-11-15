using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
    private ToothTracker toothTracker;
    private CameraFollow cameraFollow;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject gameOverScreen;
    private AudioManager audioManager;

    private void Awake() {
        toothTracker = FindAnyObjectByType<ToothTracker>();
        cameraFollow = FindAnyObjectByType<CameraFollow>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        audioManager.PlaySFX("roar");
    }
}
