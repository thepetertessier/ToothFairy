using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class TextData
{
    [TextArea] public string text;
    public float? customFadeDuration;
    public float? customDuration;
    public float? customShakeIntensity;
}

public class FloorTextController : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshPro tmpText;

    [Header("Display Settings")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float defaultDuration = 1f;
    [SerializeField] private float shakeIntensity = 0.05f;
    [SerializeField] private float flickerFrequency = 0.1f;
    [SerializeField] private float flickerIntensity = 0.2f;

    private Coroutine displayCoroutine;
    private AudioManager audioManager;

    private void Awake()
    {
        tmpText.alpha = 0f;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    /// <summary>
    /// Displays text using a TextData object.
    /// </summary>
    public void DisplayText(TextData textData)
    {
        if (displayCoroutine != null) StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(DisplayRoutine(
            textData.text,
            textData.customFadeDuration ?? fadeDuration,
            textData.customDuration ?? defaultDuration,
            textData.customShakeIntensity ?? shakeIntensity
        ));
    }

    /// <summary>
    /// Displays a series of texts from a TextSeries Scriptable Object.
    /// </summary>
    public void DisplayTextSeries(TextSeries textSeries)
    {
        if (displayCoroutine != null) StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(DisplaySeriesRoutine(textSeries.textDataList));
        audioManager.PlaySFX("text reveal");
    }

    private IEnumerator DisplayRoutine(string text, float fadeTime, float displayTime, float shakeAmount)
    {
        tmpText.text = text;
        tmpText.alpha = 0f;

        // Fade In
        yield return FadeText(0f, 1f, fadeTime);

        // Shake and Flicker while displaying
        float timer = 0f;
        while (timer < displayTime)
        {
            timer += Time.deltaTime;

            // Shake Effect
            tmpText.transform.localPosition = Random.insideUnitCircle * shakeAmount;

            // Flicker Effect
            tmpText.alpha = 1f - Random.Range(0, flickerIntensity);

            yield return new WaitForSeconds(flickerFrequency);
        }

        // Reset position and alpha before fading out
        tmpText.transform.localPosition = Vector3.zero;
        tmpText.alpha = 1f;

        // Fade Out
        yield return FadeText(1f, 0f, fadeTime);

        tmpText.text = "";
    }

    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            tmpText.alpha = alpha;
            yield return null;
        }
    }

    private IEnumerator DisplaySeriesRoutine(List<TextData> textDataList)
    {
        foreach (TextData textData in textDataList)
        {
            audioManager.PlaySFX("text reveal");
            // yield return StartCoroutine(DisplayRoutine(
            //     textData.text,
            //     textData.customFadeDuration ?? fadeDuration,
            //     textData.customDuration ?? defaultDuration,
            //     textData.customShakeIntensity ?? shakeIntensity
            // ));
            yield return StartCoroutine(DisplayRoutine(
                textData.text,
                fadeDuration,
                defaultDuration,
                shakeIntensity
            ));
        }
    }

    public void DisplayPressE() {
        if (displayCoroutine != null) StopCoroutine(displayCoroutine);
        tmpText.text = "PRESS E";
        tmpText.color = Color.red;
        tmpText.fontSizeMax = 4;
        tmpText.alpha = 1f;
    }

    public void StopDisplay() {
        if (displayCoroutine != null) StopCoroutine(displayCoroutine);
        tmpText.color = Color.white;
        tmpText.fontSizeMax = 40;
        tmpText.alpha = 0f;
    }
}
