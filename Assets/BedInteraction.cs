using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteraction : ObjectInteraction
{
    [SerializeField] protected float minInteractionTime = 3f;
    [SerializeField] protected float maxInteractionTime = 5f;

    private bool isSearched = false;
    private GoodiePlacer goodiePlacer;
    private ToothTracker toothTracker;
    private KeyTracker keyTracker;
    private string bedName;
    private SpriteRenderer bedSpriteRenderer;

    protected override void CustomAwake()
    {
        GameObject logic = GameObject.FindGameObjectWithTag("Logic");
        goodiePlacer = logic.GetComponent<GoodiePlacer>();
        toothTracker = FindAnyObjectByType<ToothTracker>();
        keyTracker = logic.GetComponent<KeyTracker>();
        Transform parentTransform = transform.parent;
        bedSpriteRenderer = parentTransform.GetComponent<SpriteRenderer>();
        bedName = parentTransform.name;
    }

    protected override void Update() {
        if (isSearched) return;
        base.Update();
    }

    protected override bool PlayerIsFacingObject(Transform player, Transform transform, PlayerDirection playerDirection) {
        bool playerIsLeftOfBed = player.position.x < transform.position.x;
        bool playerIsFacingRight = playerDirection == PlayerDirection.Right;
        bool playerIsFacingLeft = playerDirection == PlayerDirection.Left;
        return (playerIsLeftOfBed && playerIsFacingRight) || (!playerIsLeftOfBed && playerIsFacingLeft);
    }

    protected override void CompleteInteraction()
    {
        base.CompleteInteraction();
        isSearched = true;
        bedSpriteRenderer.color = new Color(0.688f, 0.523f, 0.523f, 1f);

        if (goodiePlacer.HasKey(bedName))
        {
            keyTracker.CollectKey();
        }
        if (goodiePlacer.HasTooth(bedName))
        {
            toothTracker.CollectTooth();
        }

        this.enabled = false;
    }

    protected override float GetInteractionTime() {
        return Random.Range(minInteractionTime, maxInteractionTime);
    }

    protected override string GetSFX() {
        return audioManager.GetRandomRufflingSound();
    }
}
