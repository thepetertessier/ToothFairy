using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour {
    [SerializeField] private float minInteractionTime = 3f;
    [SerializeField] private float maxInteractionTime = 5f;
    [SerializeField] private Image loadingBarImage;
    private float interactionTime;
    private bool isPlayerNearby = false;
    private float holdTime = 0f;
    private bool isInteracting = false;
    private bool isSearched = false;
    private PlayerControls playerControls;
    private Transform player;
    private PlayerMovement playerMovement;
    private GoodiePlacer goodiePlacer;
    private string bedName;
    SpriteRenderer bedSpriteRenderer;


    private void Awake() {
        playerControls = new PlayerControls();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        loadingBarImage = GameObject.FindGameObjectWithTag("Loading").GetComponent<Image>();
        goodiePlacer = GameObject.FindGameObjectWithTag("Logic").GetComponent<GoodiePlacer>();
        Transform parentTransform = transform.parent;
        bedSpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        bedName = parentTransform.name;
    }

    private void Start() {
        loadingBarImage.gameObject.SetActive(false);
        loadingBarImage.fillAmount = 0f;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private bool PlayerIsActing() {
        return playerControls.Interaction.Act.ReadValue<float>() > 0;
    }

    private bool PlayerIsFacingBed() {
        bool playerIsLeftOfBed = player.position.x > transform.position.x;
        bool playerIsFacingRight = playerMovement.movement.x >= 0;
        bool playerIsFacingLeft = playerMovement.movement.x <= 0;
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
            }

            // Update the hold time and fill the loading bar
            holdTime += Time.deltaTime;
            loadingBarImage.fillAmount = getConcaveDownProgress(holdTime / interactionTime);

            // Check if fully filled
            if (holdTime >= interactionTime) {
                CompleteInteraction();
            }
        }
        else if (isInteracting) // Stop filling when E is released
        {
            isInteracting = false;
            holdTime = 0f;
            loadingBarImage.fillAmount = 0f;
        }
    }

    private void CompleteInteraction() {
        isSearched = true;
        loadingBarImage.gameObject.SetActive(false);

        // darken to show it's been searched
        bedSpriteRenderer.color = new Color(0.688f, 0.523f, 0.523f, 1f);

        if (goodiePlacer.HasKey(bedName)) {
            Debug.Log("Found key!");
        }
        if (goodiePlacer.HasTooth(bedName)) {
            Debug.Log("Found tooth!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = true;
            ActivateLoadingBar();
        }
    }

    private void ActivateLoadingBar() {
        loadingBarImage.gameObject.SetActive(true);
        FixLoadingBarPosition();
        interactionTime = Random.Range(minInteractionTime, maxInteractionTime);
    }

    private void FixLoadingBarPosition() {
        UnityEngine.Vector3 bedScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        loadingBarImage.transform.position = bedScreenPosition;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerNearby = false;
            isInteracting = false;
            holdTime = 0f;
            loadingBarImage.fillAmount = 0f;
            loadingBarImage.gameObject.SetActive(false);
        }
    }
}
