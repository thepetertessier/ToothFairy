using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectInteraction : MonoBehaviour
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

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMovement = player.GetComponent<PlayerMovement>();
        progressBarController = GameObject.FindGameObjectWithTag("Loading").GetComponent<ProgressBarController>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        toothstalkerAI = FindAnyObjectByType<ToothstalkerAI>();
        CustomAwake();
    }

    // This method can be overridden by child classes if they need additional setup.
    protected virtual void CustomAwake() { }

    protected virtual void Update() {
        if (PlayerCanInteract()) {
            FixLoadingBarPosition();

            if (!isInteracting) {
                isInteracting = true;
                holdTime = 0f; // Reset hold time
                playerMovement.TurnOffLight();
                ActivateLoadingBar();
                cameraFollow.ZoomInToTarget();
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
    }

    protected virtual bool PlayerCanInteract() {
        return isPlayerNearby && PlayerIsActing() && PlayerIsFacingObject(player, transform, playerMovement.GetPlayerDirection()) && !toothstalkerAI.IsAttacking();
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
    }

    protected virtual void HaltInteraction() {
        // always do this, even if interrupted
        isInteracting = false;
        holdTime = 0f;
        progressBarController.TurnOff();
        playerMovement.TurnOnLight();
        cameraFollow.ResetZoom();
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
            isInteracting = false;
            holdTime = 0f;
            progressBarController.TurnOff();
        }
    }
}
