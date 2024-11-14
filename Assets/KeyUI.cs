using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyUI : MonoBehaviour, IInitializable {
    private Image image;
    public void Initialize() {
        image.enabled = false;
    }

    private void Awake() {
        image = GetComponent<Image>();
    }
    public void MakeKeyVisible() {
        image.enabled = true;
    }
}
