using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton : MonoBehaviour
{
    private static PersistentSingleton instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }
}
