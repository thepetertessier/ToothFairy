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

public class PlayerMovement : MonoBehaviour, IInitializable
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private GameObject playerLight;

    private PlayerControls playerControls;
    public Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    public PlayerDirection direction;
    private Transform flashlight;
    private bool canMove = true;
    private Vector3 initPosition;

    private void Awake() {
        direction = PlayerDirection.Up;
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        flashlight = transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        initPosition = transform.position;
    }

    public void Initialize() {
        transform.position = initPosition;
        UpdateDirection();
        UpdateFlashlightDirection();
        animator.SetInteger("Direction", GetDirectionInt(direction));
    }

    private void OnEnable() {
        playerControls ??= new PlayerControls();
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }


    private void Update() {
        PlayerInput();
        UpdateDirection();
    }

    public void SetCanMove(bool canMove) {
        this.canMove = canMove;
    }

    private void UpdateDirection() {
        if (GetDirectionCondition(direction) || movement == Vector2.zero)
            // as long as the player keeps holding down direction or presses nothing, nothing updates
            return;
        Array directions = Enum.GetValues(typeof(PlayerDirection));
        foreach (PlayerDirection dir in directions) {
            SetDirectionBasedOn(dir);
        }
        UpdateFlashlightDirection();
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

    private void UpdateFlashlightDirection() {
        float z_rotation = direction switch {
            PlayerDirection.Up => 0,
            PlayerDirection.Right => -90,
            PlayerDirection.Down => 180,
            PlayerDirection.Left => 90,
            _ => -90,
        };
        flashlight.rotation = Quaternion.Euler(0, 0, z_rotation);
    }

    public PlayerDirection GetPlayerDirection() {
        return direction;
    }

    public bool IsActing() {
        return playerControls.Interaction.Act.ReadValue<float>() > 0;
    }

    private void FixedUpdate() {
        if (canMove) {
            Move();
        }
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void Move() {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void TurnOffLight() {
        playerLight.SetActive(false);
    }

    public void TurnOnLight() {
        playerLight.SetActive(true);
    }

    private void OnDestroy() {
        // Ensure that the input actions are disabled to avoid memory leaks
        playerControls.Movement.Disable();
        playerControls.Dispose();
    }
}
