using System.Collections;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float footstepInterval = 0.5f; // Time between footsteps
    [SerializeField] private float volume = 0.7f;

    private PlayerMovement playerMovement;
    private bool isWalking;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Check if the player is moving
        isWalking = playerMovement != null && playerMovement.movement.magnitude > 0.1f;

        // If the player is moving and not already playing footsteps, start playing footsteps
        if (isWalking && !IsInvoking(nameof(PlayFootstep)))
        {
            InvokeRepeating(nameof(PlayFootstep), 0f, footstepInterval);
        }
        // Stop playing footsteps if the player has stopped moving
        else if (!isWalking)
        {
            CancelInvoke(nameof(PlayFootstep));
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        // Select a random footstep clip
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip, volume);
    }
}
