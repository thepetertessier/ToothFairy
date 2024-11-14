using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPulse : MonoBehaviour
{
    private Camera mainCamera;
    public float shakeMagnitude = 0.05f;
    public float shakeSpeed = 0.02f;

    private Vector3 originalPosition;

    private void Start() {
        mainCamera = Camera.main;
        originalPosition = mainCamera.transform.localPosition;
    }

    // Call this method to shake the camera
    public void Shake() {
        StartCoroutine(ShakeCoroutine());
    }

    public void StopShaking() {
        StopAllCoroutines();
        mainCamera.transform.localPosition = originalPosition; // Reset position
    }

    private IEnumerator ShakeCoroutine() {
        float timeElapsed = 0f;
        
        while (true) {
            if (timeElapsed >= shakeSpeed) {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;

                mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);
                timeElapsed = 0;
                yield return null;
            } else {
                timeElapsed += Time.deltaTime;
            }
        }
    }
}
