using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class SourcedClip : MonoBehaviour {
//     private AudioClip clip;
//     private AudioSource source;
// }

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource objectInteractionSource;
    [SerializeField] private AudioSource toothstalkerSource;
    [SerializeField] private AudioSource playerBreathingSource;
    [SerializeField] private AudioSource playerMovingSource;
    public Dictionary<AudioClip, AudioSource> clipToSource = new();

    public AudioClip background;
    public AudioClip ruffling1;
    public AudioClip ruffling2;
    public AudioClip ruffling3;
    public AudioClip ruffling4;
    public AudioClip keyTurning;
    public AudioClip toothstalkerAttack;
    public AudioClip walking;
    public AudioClip death;
    public AudioClip breathing1;
    public AudioClip breathing2;

    private void Awake() {
        clipToSource[ruffling1] = objectInteractionSource;
        clipToSource[ruffling2] = objectInteractionSource;
        clipToSource[ruffling3] = objectInteractionSource;
        clipToSource[ruffling4] = objectInteractionSource;
        clipToSource[keyTurning] = objectInteractionSource;
        clipToSource[toothstalkerAttack] = toothstalkerSource;
        clipToSource[walking] = playerMovingSource;
        clipToSource[death] = playerBreathingSource;
        clipToSource[breathing1] = playerBreathingSource;
        clipToSource[breathing2] = playerBreathingSource;
    }

    private void Start() {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volumeScale = 1f) {
        AudioSource source = clipToSource[clip];
        source.PlayOneShot(clip, volumeScale);
    }

    public void StopSFX(AudioClip clip) {
        AudioSource source = clipToSource[clip];
        if (source.isPlaying) {
            source.Stop();
        }
    }

    public AudioClip GetRandomRufflingSound() {
        AudioClip[] rufflingSounds = { ruffling1, ruffling2, ruffling3, ruffling4 };
        return rufflingSounds[Random.Range(0, rufflingSounds.Length)];
    }
}
