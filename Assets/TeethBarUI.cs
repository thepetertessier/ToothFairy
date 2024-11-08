using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeethBarUI : MonoBehaviour {
    [SerializeField] private ToothTracker toothTracker;
    [SerializeField] private Image[] toothImages; // Array to hold references to the 10 tooth images

    private void Awake() {
        toothTracker = GameObject.FindGameObjectWithTag("Logic").GetComponent<ToothTracker>();

        toothImages = new Image[toothTracker.GetMaxTeethCount()];
        for (int i = 0; i < toothImages.Length; i++) {
            toothImages[i] = transform.GetChild(i).GetComponent<Image>();
        }
    }

    private void Start() {
        UpdateTeethBar();
    }

    public void UpdateTeethBar() {
        int currentTeeth = toothTracker.GetTeethCount();
        
        // Update each tooth icon based on the number of collected teeth
        for (int i = 0; i < toothImages.Length; i++) {
            if (i < currentTeeth) {
                toothImages[i].color = Color.white; // Normal color for filled tooth
            } else {
                toothImages[i].color = Color.black; // Darken the tooth if it's missing
            }
        }
    }
}
