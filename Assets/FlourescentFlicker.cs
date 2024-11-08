using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FluorescentFlicker : MonoBehaviour
{
    [SerializeField] private Light2D fluorescentLight; // Reference to the Light2D component
    [SerializeField] private float minIntensity = 0.5f; // Minimum light intensity
    [SerializeField] private float maxIntensity = 1.5f; // Maximum light intensity
    [SerializeField] private float flickerSpeed = 0.1f; // Speed of the flicker (seconds)
    [SerializeField] private float chanceToBlackout = 0.05f; // Chance to completely go out (0 to 1)
    [SerializeField] private float blackoutDuration = 2.0f; // How long the light stays out (seconds)

    private bool isBlackout = false;
    
    private void Start()
    {
        if (fluorescentLight == null)
        {
            fluorescentLight = GetComponent<Light2D>();
        }

        // Start the flickering coroutine
        StartCoroutine(FlickerEffect());
    }

    private IEnumerator FlickerEffect()
    {
        while (true)
        {
            // Check if the light should blackout
            if (!isBlackout && Random.value < chanceToBlackout)
            {
                StartCoroutine(BlackoutEffect());
            }
            
            if (!isBlackout)
            {
                // Randomize the light intensity to simulate flickering
                float newIntensity = Random.Range(minIntensity, maxIntensity);
                fluorescentLight.intensity = newIntensity;
            }

            // Wait for the next flicker update
            yield return new WaitForSeconds(flickerSpeed);
        }
    }

    private IEnumerator BlackoutEffect()
    {
        isBlackout = true;
        
        // Turn off the light
        fluorescentLight.intensity = 0f;
        
        // Wait for the blackout duration
        yield return new WaitForSeconds(blackoutDuration);
        
        // Restore the light to a random intensity
        fluorescentLight.intensity = Random.Range(minIntensity, maxIntensity);
        isBlackout = false;
    }
}
