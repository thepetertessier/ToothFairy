using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake() {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Update() {
        PlayerInput();
        animator.SetFloat("dy", movement.y);
        animator.SetFloat("dx", movement.x);
    }

    private void FixedUpdate() {
        Move();
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move() {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void UpdateAnimation() {
        // Update the MoveX and MoveY parameters in the Animator
        animator.SetFloat("MoveX", movement.x);
        animator.SetFloat("MoveY", movement.y);

        // Optional: Set a "isMoving" boolean if you want an idle animation when not moving
        animator.SetBool("isMoving", movement != Vector2.zero);
    }
}
