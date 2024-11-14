using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : ObjectInteraction
{
    [SerializeField] private float unlockDuration = 6f;
    [SerializeField] private Sprite unlockedSprite;
    private Sprite defaultSprite;
    private KeyTracker keyTracker;
    private SpriteRenderer doorRenderer;
    private BoxCollider2D doorCollider;

    protected override void CustomAwake() {
        keyTracker = GameObject.FindGameObjectWithTag("Logic").GetComponent<KeyTracker>();
        Transform parentTransform = transform.parent;
        doorRenderer = parentTransform.GetComponent<SpriteRenderer>();
        doorCollider = parentTransform.GetComponent<BoxCollider2D>();
        defaultSprite = doorRenderer.sprite;
    }

    protected override void CompleteInteraction() {
        doorRenderer.sprite = unlockedSprite;
        doorCollider.enabled = false;
        base.CompleteInteraction();
    }

    protected override bool PlayerIsFacingObject(Transform player, Transform transform, PlayerDirection playerDirection) {
        return playerDirection == PlayerDirection.Up;
    }

    protected override float GetInteractionTime() {
        return unlockDuration;
    }

    // protected override Vector3 GetLoadingBarOffset() {
    //     return base.GetLoadingBarOffset() + new Vector3(0,0);
    // }

    protected override bool PlayerCanInteract() {
        return base.PlayerCanInteract() && keyTracker.PlayerHasKey();
    }

    protected override string GetSFX()
    {
        return "key turning";
    }

    protected override void ResetState()
    {
        base.ResetState();
        doorRenderer.sprite = defaultSprite;
        doorCollider.enabled = true;
    }
}
