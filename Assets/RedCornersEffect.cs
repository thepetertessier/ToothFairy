using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class RedCornersEffect : MonoBehaviour
{
    private PostProcessVolume postProcessVolume;
    [SerializeField] private Vignette vignetteEffect;
    private bool isActive = false;

    private void Start()
    {
        // Get the PostProcessVolume in the scene
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        
        if (postProcessVolume != null && postProcessVolume.profile != null)
        {
            // Try to get the Vignette effect from the profile
            postProcessVolume.profile.TryGetSettings(out vignetteEffect);
        }

        // if (vignetteEffect == null)
        // {
        //     Debug.LogError("Vignette effect is missing in PostProcessing profile.");
        // }
    }

    public void ActivateRedCorners()
    {
        if (vignetteEffect != null)
        {
            isActive = true;
            vignetteEffect.intensity.value = 0.5f; // Adjust intensity of the vignette
            vignetteEffect.color.value = Color.red; // Set vignette color to red
            vignetteEffect.smoothness.value = 0.2f; // Controls the smoothness of the vignette (how much it affects the screen)
        }
    }

    public void DeactivateRedCorners()
    {
        if (vignetteEffect != null)
        {
            isActive = false;
            vignetteEffect.intensity.value = 0f; // Reset intensity to 0 (no vignette)
        }
    }

    private void Update()
    {
        // Optionally, you can animate the vignette effect if you want it to pulse or change over time
        if (isActive)
        {
            // Example of pulsing the vignette effect intensity (make it stronger over time)
            vignetteEffect.intensity.value = Mathf.PingPong(Time.time * 0.5f, 0.5f); // Pulsing effect for more drama
        }
    }
}
