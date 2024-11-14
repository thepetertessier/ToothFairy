using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class SourcedClip : MonoBehaviour {
//     private AudioClip clip;
//     private AudioSource source;
// }

using System;

[Serializable]
public class AudioData
{
    public string name; // A friendly name for reference
    public AudioClip clip; // The actual audio clip
    public AudioSource source; // The associated audio source
    [Range(0f, 1f)] public float volumeScale = 1f; // Volume scale (default to 1)
}


public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource musicSource;

    [SerializeField] private List<AudioData> audioDataList; // List to hold all audio data
    private Dictionary<AudioSource, Coroutine> fadeOutCoroutines = new();


    private Dictionary<string, AudioData> dataFromName = new();
    public AudioClip background;

    private AudioData GetDataFromName(string name) {
        if (dataFromName.TryGetValue(name, out AudioData data)) {
                return data;
        } else {
            Debug.LogError($"Name of audio data '{name}' not found.");
            return default;
        }
    }

    private void Awake() {
        foreach (var audioData in audioDataList) {
            if (audioData.name != null) {
                dataFromName[audioData.name] = audioData;
            }
        }
    }

    private void Start() {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(string name, float volumeScale = 1f) {
        AudioData data = GetDataFromName(name);
        StopExistingFadeOut(data.source, data.volumeScale);
        data.source.PlayOneShot(data.clip, volumeScale * data.volumeScale);
    }

    public void StopSFX(string name, float fadeOutDuration = 1f) {
        AudioData data = GetDataFromName(name);
        if (data.source == null) return;

        if (data.source.isPlaying) {
            if (fadeOutDuration > 0f) {
                StopExistingFadeOut(data.source);
                fadeOutCoroutines[data.source] = StartCoroutine(FadeOutAndStop(data.source, fadeOutDuration, data.volumeScale));
            } else {
                data.source.Stop();
            }
        }
    }

    private void StopExistingFadeOut(AudioSource source, float volumeScale = -1f) {
        if (fadeOutCoroutines.ContainsKey(source) && fadeOutCoroutines[source] != null) {
            StopCoroutine(fadeOutCoroutines[source]);
            fadeOutCoroutines[source] = null;
            if (volumeScale != -1f) {
                source.volume = volumeScale;
            }
        }
    }

    private IEnumerator FadeOutAndStop(AudioSource source, float duration, float defaultVolume)
    {
        float startVolume = source.volume;
        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        source.Stop();
        source.volume = defaultVolume; // Reset the volume for the next play

        // Clear the coroutine reference after finishing
        fadeOutCoroutines[source] = null;
    }


    public string GetRandomRufflingSound() {
        string[] rufflingSounds = { "ruffling1", "ruffling2", "ruffling3", "ruffling4" };
        return rufflingSounds[UnityEngine.Random.Range(0, rufflingSounds.Length)];
    }
}
