using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToothstalkerAttack : MonoBehaviour {
    [SerializeField] private float biteInterval = 2.0f; // Time between bites
    [SerializeField] private float progressDecreaseRate = 0.2f; // Rate at which progress decreases
    [SerializeField] private float bitePenalty = 0.3f; // Penalty on progress bar after each bite
    private PlayerMovement playerMovement;
    private ToothTracker toothTracker;
    private bool isAttaching = false;
    private ProgressBarController progressBarController;
    private float progress = 0f;
    private float nextBiteTime;
    private bool justFinished = false;
    private bool canPressAgain = true;
    private ToothstalkerAnimation toothstalkerAnimation;

    public bool JustFinished() {
        return justFinished;
    }

    private void Awake() {
        progressBarController = GameObject.FindGameObjectWithTag("Loading").GetComponent<ProgressBarController>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        toothTracker = FindAnyObjectByType<ToothTracker>();
        toothstalkerAnimation = GetComponent<ToothstalkerAnimation>();
    }

    private void Update()
    {
        if (isAttaching)
        {
            UpdateProgress();
        }
    }

    public void AttachToPlayer()
    {
        isAttaching = true;
        progress = 0f;
        nextBiteTime = Time.time;
        progressBarController.TurnOn();
        playerMovement.SetCanMove(false);
        playerMovement.TurnOffLight();
    }

    private void UpdateProgress()
    {
        // Decrease the progress bar over time
        progress -= progressDecreaseRate * Time.deltaTime;
        progress = Mathf.Clamp01(progress);
        progressBarController.SetProgress(progress);
        progressBarController.SetPositionInWorld(transform.position);

        if (playerMovement.IsActing()) {
            if (canPressAgain) {
                progress += 0.05f;
                canPressAgain = false;
            }
        } else {
            canPressAgain = true;
        }

        // Check for bite interval
        if (Time.time >= nextBiteTime) {
            BeginBite();
            nextBiteTime = Time.time + biteInterval;
        }

        // Check if the player escaped
        if (progress >= 1f) {
            ReleasePlayer();
        }
    }

    private void BeginBite() {
        toothstalkerAnimation.TriggerAnimation(ToothstalkerState.Attacking);
        Invoke(nameof(BitePlayer), 0.2f);
    }

    private void BitePlayer() {
        toothTracker.RemoveTooth();

        // Reset progress as penalty
        progress -= bitePenalty;
        progress = Mathf.Clamp01(progress);
    }

    private void ReleasePlayer() {
        isAttaching = false;
        justFinished = true;
        progressBarController.TurnOff();

        playerMovement.SetCanMove(true);
        playerMovement.TurnOnLight();

        // Reset justFinished to false in a bit
        Invoke(nameof(RestartJustFinished), 0.5f);
    }

    private void RestartJustFinished() {
        justFinished = false;
    }
}