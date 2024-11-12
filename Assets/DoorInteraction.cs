using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour {
    [SerializeField] private Sprite openSprite;
    private SpriteRenderer spriteRenderer;
    private KeyTracker keyTracker;

    private void Awake()
    {
        Transform parent = transform.parent;
        spriteRenderer = parent.GetComponent<SpriteRenderer>();
        keyTracker = FindAnyObjectByType<KeyTracker>();
    }

    // Call this method when the player interacts with the door
    public void OpenDoor() {
        spriteRenderer.sprite = openSprite;
        Debug.Log("Door opened!");
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && keyTracker.PlayerHasKey()) {
            OpenDoor();
        }
    }
}
