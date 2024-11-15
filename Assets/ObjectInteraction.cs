using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteraction : MonoBehaviour, IInitializable
{
    private float interactionTime;
    private bool isPlayerNearby = false;
    private float holdTime = 0f;
    private bool isInteracting = false;
    private Transform player;
    private PlayerMovement playerMovement;
    private ProgressBarController progressBarController;
    private CameraFollow cameraFollow;
    private ToothstalkerAI toothstalkerAI;
    protected AudioManager audioManager;
    private bool hasInteracted = false;
    private string clip;

    public void Initialize() {
        ResetState();
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMovement = player.GetComponent<PlayerMovement>();
        progressBarController = GameObject.FindGameObjectWithTag("Loading").GetComponent<ProgressBarController>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        toothstalkerAI = FindAnyObjectByType<ToothstalkerAI>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        clip = GetSFX();
        CustomAwake();
    }

    // This method can be overridden by child classes if they need additional setup.
    protected virtual void CustomAwake() { }

    protected virtual void Update() {
        if (hasInteracted) return;
        if (PlayerCanInteract()) {
            FixLoadingBarPosition();

            if (!isInteracting) {
                isInteracting = true;
                toothstalkerAI.SetPlayerIsInteracting(true);
                holdTime = 0f; // Reset hold time
                playerMovement.TurnOffLight();
                ActivateLoadingBar();
                cameraFollow.ZoomInToTarget();
                audioManager.PlaySFX(clip);
            }

            // Update the hold time and fill the loading bar
            holdTime += Time.deltaTime;
            progressBarController.SetProgress(GetConcaveDownProgress(holdTime / interactionTime));

            // Check if fully filled
            if (holdTime >= interactionTime) {
                CompleteInteraction();
            }
        }
        else if (isInteracting) {
            HaltInteraction();
        }

        if (isInteracting && toothstalkerAI.IsAlert()) {
            // and toothStalker is within view of the camera
            audioManager.PlayOnce("horror hit");
        }
    }

    protected virtual bool PlayerCanInteract() {
        return isPlayerNearby && PlayerIsActing() && PlayerIsFacingObject(player, transform, playerMovement.GetPlayerDirection()) && !toothstalkerAI.IsAttacking() && playerMovement.enabled;
    }

    private bool PlayerIsActing()
    {
        return playerMovement.IsActing();
    }

    // This method can be overridden by child classes if they need different facing logic.
    protected virtual bool PlayerIsFacingObject(Transform player, Transform transform, PlayerDirection playerDirection) {
        return true;
    }

    protected float GetConcaveDownProgress(float percentDone)
    {
        return 1 - Mathf.Pow(1 - percentDone, 2);
    }

    protected virtual void CompleteInteraction() {
        HaltInteraction();
        // never allow to be interacted again
        hasInteracted = true;
    }

    protected virtual void ResetState() {
        hasInteracted = false;
    }

    protected virtual void HaltInteraction() {
        // always do this, even if interrupted
        isInteracting = false;
        toothstalkerAI.SetPlayerIsInteracting(false);
        holdTime = 0f;
        progressBarController?.TurnOff();
        playerMovement?.TurnOnLight();
        cameraFollow?.ResetZoom();
        audioManager?.StopSFX(clip);
    }

    protected void FixLoadingBarPosition() {
        Vector3 position = transform.position + GetLoadingBarOffset();
        progressBarController.SetPositionInWorld(position);
    }

    protected virtual Vector3 GetLoadingBarOffset() { return default; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            ActivateLoadingBar();
        }
    }

    protected abstract string GetSFX();

    private void ActivateLoadingBar()
    {
        progressBarController.TurnOn();
        FixLoadingBarPosition();
        interactionTime = GetInteractionTime();
    }

    protected virtual float GetInteractionTime() { return 1f; }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            HaltInteraction();
        }
    }
}
