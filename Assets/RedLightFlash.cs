using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RedLightFlash : MonoBehaviour
{
    [SerializeField] private Light2D redLight;
    public float flashDuration = 0.1f; // How quickly the light flashes
    public float maxIntensity = 2f;
    public float minIntensity = 0.2f;
    private bool isFlashing = false;

    public void StartFlashing() {
        if (!isFlashing) {
            StartCoroutine(FlashCoroutine());
        }
    }

    public void StopFlashing() {
        StopAllCoroutines();
        redLight.intensity = 0f; // Reset to normal intensity
    }

    private IEnumerator FlashCoroutine() {
        isFlashing = true;
        while (true) {
            redLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PingPong(Time.time / flashDuration, 1));
            yield return null;
        }
    }
}
