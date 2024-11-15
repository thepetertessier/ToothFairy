using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour, IInitializable
{
    [SerializeField] private TextSeries opening;
    [SerializeField] private TextSeries afterGettingTooth;
    [SerializeField] private TextSeries afterGettingKey;
    [SerializeField] private TextSeries ending;
    private FloorTextController floorTextController;
    private LevelSelector levelSelector;
    private ToothTracker toothTracker;
    private Dictionary<TextSeries, bool> hasDelivered = new();
    private KeyTracker keyTracker;
    private string levelName;

    private void Awake() {
        levelSelector = FindAnyObjectByType<LevelSelector>();
        floorTextController = FindAnyObjectByType<FloorTextController>();
        toothTracker = FindAnyObjectByType<ToothTracker>();
        keyTracker = FindAnyObjectByType<KeyTracker>();
        hasDelivered[afterGettingKey] = false;
        hasDelivered[afterGettingTooth] = false;
    }

    public void Initialize()
    {
        levelName = levelSelector.GetCurrentLevelName();
        if (levelName == "Room0") {
            floorTextController.DisplayTextSeries(opening);
        } else if (levelName == "TheEnd") {
            floorTextController.DisplayTextSeries(ending);
        }
    }

    private void Update()
    {
        if (levelName == "Room0") {
            if (!hasDelivered[afterGettingTooth] && toothTracker.anyTeethCollected) {
                floorTextController.DisplayTextSeries(afterGettingTooth);
                hasDelivered[afterGettingTooth] = true;
            }

            if (!hasDelivered[afterGettingKey] && keyTracker.PlayerHasKey()) {
                floorTextController.DisplayTextSeries(afterGettingKey);
                hasDelivered[afterGettingKey] = true;
            }
        }
    }
}
