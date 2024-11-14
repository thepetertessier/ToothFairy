using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelConfig levelConfig;
    [SerializeField] private FloorController floorController;
    [SerializeField] private ToothstalkerAI toothstalkerAI;

    private void Start()
    {
        ApplyLevelConfig();
    }

    private void ApplyLevelConfig()
    {
        floorController.SetColor(levelConfig.floorColor);

        // Set Toothstalker properties
        toothstalkerAI.SetStats(
            levelConfig.toothstalkerSpeed,
            levelConfig.toothstalkerPauseDuration,
            levelConfig.toothstalkerDetectionRadius);
    }

    public void SetLevelConfig(LevelConfig config) {
        levelConfig = config;
    }

    public LevelConfig GetLevelConfig() {
        return levelConfig;
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
        // Reacquire references whenever a new scene is loaded
        floorController = FindObjectOfType<FloorController>();
        toothstalkerAI = FindObjectOfType<ToothstalkerAI>();
        
        // Ensure these references are not null
        if (floorController == null) {
            Debug.LogError("FloorController not found in the scene!");
        }
        if (toothstalkerAI == null) {
            Debug.LogError("ToothstalkerAI not found in the scene!");
        }
        ApplyLevelConfig();
    }
}
