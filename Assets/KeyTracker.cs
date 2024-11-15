using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTracker : MonoBehaviour, IInitializable
{
    public bool hasKey = false;
    private KeyUI keyUI;
    private AudioManager audioManager;
    private void Awake() {
        keyUI = GameObject.FindGameObjectWithTag("KeyUI").GetComponent<KeyUI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    public bool PlayerHasKey() {
        return hasKey;
    }

    public void CollectKey() {
        hasKey = true;
        keyUI.MakeKeyVisible();
        audioManager.PlaySFX("key collected"); 
    }

    public void Initialize() {
        hasKey = false;
    }
}
