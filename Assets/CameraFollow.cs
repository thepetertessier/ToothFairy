using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player; // Reference to the player’s transform
    [SerializeField] private Vector2 bottomLeftCorner; // Bottom-left corner of the map
    [SerializeField] private Vector2 topRightCorner; // Top-right corner of the map
    [SerializeField] private float smoothing = 0.3f; // How smoothly the camera follows the player

    [Header("Zoom Settings")]
    [SerializeField] private float normalSize = 5f; // Default orthographic size of the camera
    [SerializeField] private float zoomedSize = 3f;  // Smaller orthographic size for zoomed-in effect
    [SerializeField] private float zoomDuration = 0.6f; // Speed of zooming

    private Vector3 velocity = Vector3.zero; // For smooth damping
    private float zoomLerpTime = 0f; // Keeps track of the zooming progress
    private bool isZooming = false;
    private float fromSize;
    private float toSize;

    private Camera cam; // Reference to the camera component

    private void Awake()
    {
        cam = GetComponent<Camera>();
        GameObject bounds = GameObject.FindGameObjectWithTag("Bounds");
        Transform lowerLeft = bounds.transform.GetChild(0);
        Transform upperRight = bounds.transform.GetChild(1);
        bottomLeftCorner = lowerLeft.position;
        topRightCorner = upperRight.position;
    }

    private void LateUpdate()
    {
        if (player != null && cam != null)
        {
            // Step 1: Center the camera on the player with smoothing
            Vector3 targetPosition = new(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothing);

            // Step 2: Handle camera zoom
            HandleZoom();

            // Step 3: Ensure camera stays within the map bounds
            ClampCameraWithinBounds();
        }
    }

    /// <summary>
    /// Smoothly zooms the camera in by adjusting its orthographic size.
    /// </summary>
    public void ZoomInToTarget()
    {
        ZoomTo(zoomedSize);
    }

    /// <summary>
    /// Smoothly zooms the camera out to its default orthographic size.
    /// </summary>
    public void ResetZoom()
    {
        ZoomTo(normalSize);
    }

    private void ZoomTo(float size) {
        if (cam == null) return;
        isZooming = true;
        zoomLerpTime = 0f;
        fromSize = cam.orthographicSize;
        toSize = size;
    }

    /// <summary>
    /// Continuously handles the zoom in/out effect by adjusting the orthographic size.
    /// </summary>
    private void HandleZoom() {
        if (isZooming) {
            zoomLerpTime += Time.deltaTime / zoomDuration;
            float smoothStep = Mathf.SmoothStep(fromSize, toSize, zoomLerpTime);
            cam.orthographicSize = smoothStep;

            // Check if the zoom has reached the target size
            if (Mathf.Approximately(cam.orthographicSize, toSize) || zoomLerpTime >= 1f) {
                cam.orthographicSize = toSize;
                isZooming = false;
            }
        }
    }

    /// <summary>
    /// Ensures the camera stays within the map bounds after moving and zooming.
    /// </summary>
    private void ClampCameraWithinBounds()
    {
        // Calculate half extents based on the orthographic size and aspect ratio
        float halfHeight = cam.orthographicSize;
        float halfWidth = cam.orthographicSize * cam.aspect;

        // Clamp the camera's position to ensure its edges stay within the map bounds
        float clampedX = Mathf.Clamp(transform.position.x, bottomLeftCorner.x + halfWidth, topRightCorner.x - halfWidth);
        float clampedY = Mathf.Clamp(transform.position.y, bottomLeftCorner.y + halfHeight, topRightCorner.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
