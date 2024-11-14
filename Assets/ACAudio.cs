using System.Collections;
using UnityEngine;

public class ACAudio : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float turnOnChance = 0.9f;
    [SerializeField] private float checkInterval = 5f; // How often to check (in seconds)
    private AudioManager audioManager;

    private void Awake() {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnEnable() {
        // Start the coroutine to check less frequently
        StartCoroutine(CheckForACNoise());
    }

    private void OnDisable() {
        // Stop the coroutine if this object is disabled
        StopCoroutine(CheckForACNoise());
    }

    private IEnumerator CheckForACNoise() {
        while (true) {
            // Wait for the specified interval
            yield return new WaitForSeconds(checkInterval);

            // Perform the random check
            if (Random.Range(0f, 1f) < turnOnChance) {
                audioManager.PlayOnce("AC");
            }
        }
    }
}
