using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToothTracker : MonoBehaviour {
    [SerializeField] private int initialTeethCount = 8;
    [SerializeField] private int maxTeethCount = 10;
    private int teethCount;
    private TeethBarUI teethBarUI;
    private AudioManager audioManager;
    public bool anyTeethCollected = false;

    private void Awake() {
        ResetTeethCount();
        teethBarUI = GameObject.FindGameObjectWithTag("TeethBar").GetComponent<TeethBarUI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        teethBarUI.Initialize(initialTeethCount, maxTeethCount);
    }

    public void CollectTooth() {
        if (teethCount < maxTeethCount) {
            teethCount++;
            audioManager.PlaySFX("tooth collected");
            teethBarUI.UpdateTeethBar(teethCount);
        } //else drop tooth?
        anyTeethCollected = true;
    }

    public void RemoveTooth() {
        if (teethCount > 0) {
            teethCount--;
        }
        teethBarUI.UpdateTeethBar(teethCount);
    }

    public int GetTeethCount() {
        return teethCount;
    }

    public int GetMaxTeethCount() {
        return maxTeethCount;
    }

    public void ResetTeethCount() {
        teethCount = initialTeethCount;
    }
}
