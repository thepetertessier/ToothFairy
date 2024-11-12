using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player’s transform
    [SerializeField] private Vector2 minBounds; // Bottom-left boundary
    [SerializeField] private Vector2 maxBounds; // Top-right boundary
    [SerializeField] private float smoothing = 0.3f; // How smoothly the camera follows the player

    [Header("Zoom Settings")]
    [SerializeField] private float normalSize = 5f; // Default orthographic size of the camera
    [SerializeField] private float zoomedSize = 3f;  // Smaller orthographic size for zoomed-in effect
    [SerializeField] private float zoomSpeed = 2f; // Speed of zooming

    private Vector3 velocity = Vector3.zero; // For smooth damping
    private bool isZoomingIn = false;
    private bool isZoomingOut = false;

    private Camera cam; // Reference to the camera component

    private void Awake()
    {
        // Get the camera component on the GameObject
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            // Target position based on player's position (excluding z)
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

        // Handle continuous zooming in or out
        HandleZoom();
    }

    /// <summary>
    /// Smoothly zooms the camera in by adjusting its orthographic size.
    /// </summary>
    public void ZoomInToTarget()
    {
        isZoomingIn = true;
        isZoomingOut = false;
    }

    /// <summary>
    /// Smoothly zooms the camera out to its default orthographic size.
    /// </summary>
    public void ResetZoom()
    {
        isZoomingIn = false;
        isZoomingOut = true;
    }

    /// <summary>
    /// Continuously handles the zoom in/out effect by adjusting the orthographic size.
    /// </summary>
    private void HandleZoom()
    {
        if (isZoomingIn)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, zoomedSize, zoomSpeed * Time.deltaTime);

            // Stop zooming in when we reach the zoomed target
            if (Mathf.Approximately(cam.orthographicSize, zoomedSize))
            {
                isZoomingIn = false;
            }
        }
        else if (isZoomingOut)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, normalSize, zoomSpeed * Time.deltaTime);

            // Stop zooming out when we reach the default size
            if (Mathf.Approximately(cam.orthographicSize, normalSize))
            {
                isZoomingOut = false;
            }
        }
    }
}
