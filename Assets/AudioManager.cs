using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioClip background;
    public AudioClip bedSearch;
    public AudioClip keyTurning;
    public AudioClip toothstalkerAttack;
    public AudioClip walking;
    public AudioClip death;
    public AudioClip breathing1;
    public AudioClip breathing2;

    private void Start() {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        SFXSource.PlayOneShot(clip);
    }
}
