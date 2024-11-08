using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {
    [SerializeField] private Sprite lockedSprite; // The sprite for the locked door
    [SerializeField] private Sprite openSprite;   // The sprite for the open door
    private SpriteRenderer spriteRenderer;
    private bool isOpen = false;

    private void Awake()
    {
        // Get the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set the initial door sprite to locked
        spriteRenderer.sprite = lockedSprite;
    }

    // Call this method when the player interacts with the door
    public void OpenDoor()
    {
        if (PlayerHasKey() && !isOpen)
        {
            // Change the door sprite to the open one
            spriteRenderer.sprite = openSprite;
            isOpen = true;
            Debug.Log("Door opened!");
        }
    }

    // Mockup function to check if the player has the key
    private bool PlayerHasKey()
    {
        // Check with your actual player key logic
        // Replace this with your method for checking if the player has the key
        return FindObjectOfType<KeyTracker>().PlayerHasKey();
    }

    // Optional: Detect player collision to automatically open the door
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor(); 
        }
    }
}
