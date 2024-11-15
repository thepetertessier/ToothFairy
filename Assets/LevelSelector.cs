using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public interface IInitializable {
    void Initialize();
}

public class LevelSelector : MonoBehaviour {
    [SerializeField] private LevelConfig currentLevel;
    [SerializeField] private LevelConfig[] levelConfigs;
    [SerializeField] private FloorController floorController;
    [SerializeField] private ToothstalkerAI toothstalkerAI;
    [SerializeField] private GameObject toothstalker;
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
        // currentLevel = levelConfigs[0];
        LoadLevel(currentLevel);
    }

    public void InitializeAll() {
        foreach (IInitializable initializable in initializables.Cast<IInitializable>()) {
            initializable.Initialize();
        }
    }

    private void ApplyLevelConfig(LevelConfig levelConfig) {
        floorController.SetColor(levelConfig.floorColor);

        toothstalker.SetActive(levelConfig.toothstalkerExists);
        toothstalker.GetComponent<ToothstalkerAudio>().maxVolume = levelConfig.toothstalkerExists ? 1.0f : 0.2f;

        // Set Toothstalker properties
        toothstalkerAI.SetStats(
            levelConfig.toothstalkerSpeed,
            levelConfig.toothstalkerPauseDuration,
            levelConfig.toothstalkerDetectionRadius);
    }

    public string GetCurrentLevelName() {
        return currentLevel.name;
    }

    [ExecuteAlways]
    private void OnValidate()
    {
        AutoPopulateInitializables();
    }
}
