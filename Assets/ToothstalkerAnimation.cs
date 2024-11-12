using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToothstalkerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private string GetAnimatorTriggerValue(ToothstalkerState state) {
        return state switch {
            ToothstalkerState.Patrolling => "walk",
            ToothstalkerState.Pausing => "stand",
            ToothstalkerState.Alert => "walk",
            ToothstalkerState.Prowling => "walk",
            ToothstalkerState.Blinded => "die",
            ToothstalkerState.Attacking => "attack",
            ToothstalkerState.Occupied => "die",
            _ => "",
        };
    }

    public void TriggerAnimation(ToothstalkerState state) {
        animator.SetTrigger(GetAnimatorTriggerValue(state));
    }
}
