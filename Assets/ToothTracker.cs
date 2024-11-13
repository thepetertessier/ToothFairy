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
    private ToothTracker instance;
    private AudioManager audioManager;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        teethCount = initialTeethCount;
        teethBarUI = GameObject.FindGameObjectWithTag("TeethBar").GetComponent<TeethBarUI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public void CollectTooth() {
        if (teethCount < maxTeethCount) {
            teethCount++;
        } //else drop tooth?
        teethBarUI.UpdateTeethBar();
        audioManager.PlaySFX("tooth collected");
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

    public void ResetTeethCount() {
        teethCount = initialTeethCount;
    }
}
