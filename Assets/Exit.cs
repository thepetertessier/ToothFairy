using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private LevelSelector levelSelector;

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("111");
        if (other.CompareTag("Player")) {
            levelSelector.LoadNextLevel();
        }
    }
}
