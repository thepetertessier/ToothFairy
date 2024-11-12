using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyTracker : MonoBehaviour
{
    public bool hasKey = false;
    private KeyUI keyUI;
    private void Awake() {
        keyUI = GameObject.FindGameObjectWithTag("KeyUI").GetComponent<KeyUI>();
    }

    public bool PlayerHasKey() {
        return hasKey;
    }

    public void CollectKey() {
        hasKey = true;
        keyUI.MakeKeyVisible();
    }
}
