using UnityEngine;
using System.Collections.Generic;

public interface IInitializable {
    void Initialize();
}

public class LevelSelector : MonoBehaviour {
    [SerializeField] private LevelConfig currentLevel;
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private FloorController floorController;
    [SerializeField] private ToothstalkerAI toothstalkerAI;
    [SerializeField] private List<MonoBehaviour> initializables; // Store scripts that implement IInitializable
    private AudioManager audioManager;

    private void Awake() {
        AutoPopulateInitializables();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void AutoPopulateInitializables() {
        // Clear the list to avoid duplicates
        initializables.Clear();

        // Find all objects that implement IInitializable in the scene
        foreach (var initializable in FindObjectsOfType<MonoBehaviour>()) {
            if (initializable is IInitializable) {
                initializables.Add(initializable);
            }
        }
    }

    private void LoadLevel(LevelConfig levelConfig) {
        ApplyLevelConfig(levelConfig);
        InitializeAll();
        audioManager.PlaySFX("enter");
    }

    public void LoadNextLevel() {
        Debug.Log($"Loading next level: {currentLevel.nextLevel}");
        LoadLevel(currentLevel.nextLevel);
    }

    public void Start() {
        currentLevel = levelConfigs[0];
        LoadLevel(currentLevel);
    }

    public void InitializeAll() {
        foreach (var item in initializables) {
            if (item is IInitializable initializable) {
                initializable.Initialize();
            }
        }
    }

    private void ApplyLevelConfig(LevelConfig levelConfig) {
        floorController.SetColor(levelConfig.floorColor);

        // Set Toothstalker properties
        toothstalkerAI.SetStats(
            levelConfig.toothstalkerSpeed,
            levelConfig.toothstalkerPauseDuration,
            levelConfig.toothstalkerDetectionRadius);
    }

    [ExecuteAlways]
    private void OnValidate()
    {
        AutoPopulateInitializables();
    }
}
