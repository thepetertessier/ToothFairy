using UnityEngine;

public class ToothstalkerVisibility : MonoBehaviour
{
    private Camera mainCamera;
    
    private void Awake() {
        mainCamera = Camera.main; // Assumes the main camera is tagged as "MainCamera"
    }

    public bool IsInView() {
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the object is within the camera's viewport (0 to 1 for both x and y)
        return viewportPos.x >= 0f && viewportPos.x <= 1f &&
               viewportPos.y >= 0f && viewportPos.y <= 1f;
    }
}
