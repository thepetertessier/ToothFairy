using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour
{
    private bool hasKey = false;

    public bool PlayerHasKey() {
        return hasKey;
    }

    public void CollectKey() {
        hasKey = true;
    }
}
