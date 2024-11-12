using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BedInteraction : MonoBehaviour {
    [SerializeField] private float minInteractionTime = 3f;
    [SerializeField] private float maxInteractionTime = 5f;
    private float interactionTime;
    private bool isPlayerNearby = false;
    private float holdTime = 0f;
    private bool isInteracting = false;
    private bool isSearched = false;
    private Transform player;
    private PlayerMovement playerMovement;
    private GoodiePlacer goodiePlacer;
    private ToothTracker toothTracker;
    private KeyTracker keyTracker;
    private string bedName;
    SpriteRenderer bedSpriteRenderer;
    private ProgressBarController progressBarController;


    private void Awake() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        progressBarController = GameObject.FindGameObjectWithTag("Loading").GetComponent<ProgressBarController>();
        GameObject logic = GameObject.FindGameObjectWithTag("Logic");
        goodiePlacer = logic.GetComponent<GoodiePlacer>();
        toothTracker = logic.GetComponent<ToothTracker>();
        keyTracker = logic.GetComponent<KeyTracker>();
        Transform parentTransform = transform.parent;
        bedSpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        bedName = parentTransform.name;
    }

    private bool PlayerIsActing() {
        return playerMovement.IsActing();
    }

    private bool PlayerIsFacingBed() {
        bool playerIsLeftOfBed = player.position.x < transform.position.x;
        bool playerIsFacingRight = playerMovement.GetPlayerDirection() == PlayerDirection.Right;
        bool playerIsFacingLeft = playerMovement.GetPlayerDirection() == PlayerDirection.Left;
        return (playerIsLeftOfBed && playerIsFacingRight) || (!playerIsLeftOfBed && playerIsFacingLeft);
    }

    private float getConcaveDownProgress(float percentDone) {
        return 1 - Mathf.Pow(1 - percentDone, 2);
    }

    private void Update() {
        if (isSearched)
            return;
        if (isPlayerNearby && PlayerIsActing() && PlayerIsFacingBed()) {
            FixLoadingBarPosition();

            if (!isInteracting) {
                isInteracting = true;
                holdTime = 0f; // Reset hold time
                playerMovement.TurnOffLight();
            }

            // Update the hold time and fill the loading bar
            holdTime += Time.deltaTime;
            progressBarController.SetProgress(getConcaveDownProgress(holdTime / interactionTime));

            // Check if fully filled
            if (holdTime >= interactionTime) {
                CompleteInteraction();
            }
        }
        else if (isInteracting) // Stop filling when E is released
        {
            isInteracting = false;
            holdTime = 0f;
            progressBarController.SetProgress(0f);
            playerMovement.TurnOnLight();
        }
    }

    private void CompleteInteraction() {
        isSearched = true;
        progressBarController.TurnOff();
        playerMovement.TurnOnLight();

        // darken to show it's been searched
        bedSpriteRenderer.color = new Color(0.688f, 0.523f, 0.523f, 1f);

        if (goodiePlacer.HasKey(bedName)) {
            Debug.Log("Found key!");
            keyTracker.CollectKey();
        }
        if (goodiePlacer.HasTooth(bedName)) {
            Debug.Log("Found tooth!");
            toothTracker.CollectTooth();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = true;
            ActivateLoadingBar();
        }
    }

    private void ActivateLoadingBar() {
        progressBarController.TurnOn();
        FixLoadingBarPosition();
        interactionTime = Random.Range(minInteractionTime, maxInteractionTime);
    }

    private void FixLoadingBarPosition() {
        progressBarController.SetPositionInWorld(transform.position);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = false;
            isInteracting = false;
            holdTime = 0f;
            progressBarController.TurnOff();
        }
    }
}
