using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player’s transform
    [SerializeField] private Vector2 minBounds; // Bottom-left boundary
    [SerializeField] private Vector2 maxBounds; // Top-right boundary
    [SerializeField] private float smoothing = 0.3f; // How smoothly the camera follows the player

    private Vector3 velocity = Vector3.zero; // For smooth damping

    private void LateUpdate()
    {
        if (player != null)
        {
            // Target position based on player's position
            Vector3 targetPosition = new Vector3(player.position.x, player.position.y, transform.position.z);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothing);

            // Clamp camera position to stay within bounds
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
                Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y),
                transform.position.z
            );
        }
    }
}
