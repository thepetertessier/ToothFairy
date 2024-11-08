using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

public enum PlayerDirection {
    Up,
    Right,
    Down,
    Left
}

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    public Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerDirection direction = PlayerDirection.Down;

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
        UpdateDirection();
        // animator.SetFloat("dy", movement.y);
        // animator.SetFloat("dx", movement.x);
    }

    private void UpdateDirection() {
        Array directions = Enum.GetValues(typeof(PlayerDirection));
        foreach (PlayerDirection dir in directions) {
            if (!GetDirectionCondition(dir)) {
                foreach (PlayerDirection dir2 in directions) {
                    SetDirectionBasedOn(dir2);
                }
            }
        }
        animator.SetInteger("Direction", GetDirectionInt(direction));
    }

    private bool GetDirectionCondition(PlayerDirection dir) {
        return dir switch {
            PlayerDirection.Down => movement.y < 0,
            PlayerDirection.Up => movement.y > 0,
            PlayerDirection.Left => movement.x < 0,
            PlayerDirection.Right => movement.x > 0,
            _ => false,
        };
    }

    private void SetDirectionBasedOn(PlayerDirection dir) {
        if (GetDirectionCondition(dir)) {
            direction = dir;
        }
    }

    private int GetDirectionInt(PlayerDirection dir) {
        return dir switch {
            PlayerDirection.Up => 0,
            PlayerDirection.Right => 1,
            PlayerDirection.Down => 2,
            PlayerDirection.Left => 3,
            _ => 2,
        };
    }

    public PlayerDirection GetPlayerDirection() {
        return direction;
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
