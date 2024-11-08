using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothTracker : MonoBehaviour {
    [SerializeField] private int initialTeethCount = 8;
    [SerializeField] private int maxTeethCount = 10;
    private int teethCount;
    private TeethBarUI teethBarUI;

    private void Awake() {
        teethCount = initialTeethCount;
        teethBarUI = GameObject.FindGameObjectWithTag("TeethBar").GetComponent<TeethBarUI>();
    }

    public void CollectTooth() {
        if (teethCount < maxTeethCount) {
            teethCount++;
        } //else drop tooth?
        teethBarUI.UpdateTeethBar();
    }

    public void RemoveTooth() {
        if (teethCount > 0) {
            teethCount--;
        }
        teethBarUI.UpdateTeethBar();
    }

    public int GetTeethCount() {
        return teethCount;
    }

    public int GetMaxTeethCount() {
        return maxTeethCount;
    }
}
