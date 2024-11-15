using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    [SerializeField] private TextSeries opening;
    [SerializeField] private TextSeries afterGettingTooth;
    [SerializeField] private TextSeries afterGettingKey;
    private FloorTextController floorTextController;
    private LevelSelector levelSelector;
    private ToothTracker toothTracker;
    private Dictionary<TextSeries, bool> hasDelivered = new();
    private KeyTracker keyTracker;

    private void Awake() {
        levelSelector = FindAnyObjectByType<LevelSelector>();
        floorTextController = FindAnyObjectByType<FloorTextController>();
        toothTracker = FindAnyObjectByType<ToothTracker>();
        keyTracker = FindAnyObjectByType<KeyTracker>();
        hasDelivered[afterGettingKey] = false;
        hasDelivered[afterGettingTooth] = false;
    }

    private void Start()
    {
        if (levelSelector.GetCurrentLevelName() != "Room0") {
            this.enabled = false;
        }
        floorTextController.DisplayTextSeries(opening);
    }

    private void Update()
    {
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
