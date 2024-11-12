using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyUI : MonoBehaviour {
    private Image image;

    private void Awake() {
        image = GetComponent<Image>();
        image.enabled = false;
    }
    public void MakeKeyVisible() {
        image.enabled = true;
    }
}
