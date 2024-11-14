using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlourescentAudio : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake() {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            audioManager.PlaySFX("flourescent");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            audioManager.StopSFX("flourescent");
        }
    }
}
