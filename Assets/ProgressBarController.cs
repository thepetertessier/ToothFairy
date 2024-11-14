using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    private Image image;

    public void SetProgress(float progress) {
        image.fillAmount = progress;
    }

    public void TurnOff() {
        if (image != null) {
            image.enabled = false;
        }
        SetProgress(0f);
    }

    public void TurnOn() {
        image.enabled = true;
        SetProgress(0f);
    }

    public void SetPositionInWorld(UnityEngine.Vector3 position) {
        UnityEngine.Vector3 adjustedPosition = Camera.main.WorldToScreenPoint(position);
        transform.position = adjustedPosition;
    }

    void Awake() {
        image = GetComponent<Image>();
    }
    
    private void Start() {
        TurnOff();
    }
}
