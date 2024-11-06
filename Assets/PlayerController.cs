using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    public Vector2 movement;
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
}
