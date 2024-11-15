using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEventManager : MonoBehaviour
{
    [SerializeField] private TextSeries opening;
    private FloorTextController floorTextController;
    private LevelSelector levelSelector;

    private void Awake() {
        levelSelector = FindAnyObjectByType<LevelSelector>();
        floorTextController = FindAnyObjectByType<FloorTextController>();
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
        
    }
}
