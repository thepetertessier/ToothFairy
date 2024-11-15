using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestarter : MonoBehaviour {
    private ToothTracker toothTracker;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject gameOverScreen;
    private AudioManager audioManager;
    private ToothstalkerAI toothstalkerAI;
    private ToothstalkerAttack toothstalkerAttack;
    private bool gameIsOver = false;

    private void Awake() {
        toothTracker = FindAnyObjectByType<ToothTracker>();
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        audioManager = FindAnyObjectByType<AudioManager>();
        toothstalkerAI = FindAnyObjectByType<ToothstalkerAI>();
        toothstalkerAttack = FindAnyObjectByType<ToothstalkerAttack>();
    }
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update() {
        if (toothTracker.GetTeethCount() <= 0 && !gameIsOver) {
            GameOver();
        }
    }

    private void GameOver() {
        gameIsOver = true;
        if (playerMovement != null) {
            playerMovement.enabled = false;
        }
        if (toothstalkerAI != null) {
            toothstalkerAI.enabled = false;
        }
        if (toothstalkerAttack != null) {
            toothstalkerAttack.enabled = false;
        }
        gameOverScreen.SetActive(true);
        audioManager.StopSFX("dying");
        audioManager.PlaySFX("roar");
        audioManager.PlaySFX("game over");
    }
}
