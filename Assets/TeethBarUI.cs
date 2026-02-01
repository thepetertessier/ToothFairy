using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TeethBarUI : MonoBehaviour {
    [SerializeField] private Image[] toothImages; // Array to hold references to the 8 tooth images

    public void Initialize(int startingTeethCount, int maxTeethCount) {
        toothImages = new Image[maxTeethCount];
        for (int i = 0; i < toothImages.Length; i++) {
            toothImages[i] = transform.GetChild(i).GetComponent<Image>();
        }
        UpdateTeethBar(startingTeethCount);
    }

    public void UpdateTeethBar(int currentTeeth) {        
        // Update each tooth icon based on the number of collected teeth
        for (int i = 0; i < toothImages.Length; i++) {
            if (i < currentTeeth) {
                toothImages[i].color = Color.white; // Normal color for filled tooth
            } else {
                toothImages[i].color = new Color(0.5157232f, 0.1856927f, 0.1856927f); // Darken the tooth if it's missing
            }
        }
    }
}
