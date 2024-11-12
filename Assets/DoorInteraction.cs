using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : ObjectInteraction
{
    [SerializeField] private float unlockDuration = 6f;
    [SerializeField] private Sprite unlockedSprite;
    private KeyTracker keyTracker;
    private SpriteRenderer doorRenderer;

    protected override void CustomAwake() {
        keyTracker = GameObject.FindGameObjectWithTag("Logic").GetComponent<KeyTracker>();
        Transform parentTransform = transform.parent;
        doorRenderer = parentTransform.GetComponent<SpriteRenderer>();
    }

    protected override void CompleteInteraction() {
        base.CompleteInteraction();
        {
            Debug.Log("Door unlocked!");
            doorRenderer.sprite = unlockedSprite;

            // Add your logic to load the next room/scene here
            // For example:
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
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
}
