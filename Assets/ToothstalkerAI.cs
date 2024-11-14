using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ToothstalkerState {
    Patrolling,
    Pausing,
    Alert,
    Prowling,
    Blinded,
    Attacking,
    Occupied,
    None
}

public class ToothstalkerAI : MonoBehaviour {
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float pauseDuration = 2f; // time in seconds to pause
    [SerializeField] private float alertRadius = 3f; // detection radius for player
    [SerializeField] private GameObject eyes;
    private readonly float offsetY = 0.37f;

    private ToothstalkerState currentState;
    private Action currentBehavior;
    private float pauseTimer;
    private Vector3 currentTarget;

    private Dictionary<string, Transform> patrolPoints;
    private string currentPatrolPoint;


    private Transform player;  // Reference to the player
    private SpriteRenderer spriteRenderer;
    private ToothstalkerAttack toothstalkerAttack;
    private ToothstalkerAnimation toothstalkerAnimation;

    public void SetStats(float baseSpeed, float pauseDuration, float alertRadius) {
        this.baseSpeed = baseSpeed;
        this.pauseDuration = pauseDuration;
        this.alertRadius = alertRadius;
    }
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();
        toothstalkerAttack = GetComponent<ToothstalkerAttack>();
        toothstalkerAnimation = GetComponent<ToothstalkerAnimation>();

        patrolPoints = new Dictionary<string, Transform>();
        Transform patrolPointsParent = GameObject.Find("PatrolPoints").transform;
        
        foreach (Transform patrolPoint in patrolPointsParent) {
            patrolPoints[patrolPoint.name] = patrolPoint;
        }

        currentPatrolPoint = "11";
        currentTarget = patrolPoints[currentPatrolPoint].position;
    }

    private void Start(){
        SetState(ToothstalkerState.Patrolling);
    }

    private void Update() {
        currentBehavior?.Invoke();
    }

    private void SetOpacity(float alpha) {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }

    private void SetState(ToothstalkerState newState) {
        toothstalkerAnimation.TriggerAnimation(newState);

        eyes.SetActive(newState == ToothstalkerState.Alert);
        SetOpacity(newState == ToothstalkerState.Blinded ? 0.2f : 1f);

        currentState = newState;
        switch (newState) {
            case ToothstalkerState.Patrolling:
                currentBehavior = Patrol;
                currentTarget = GetNewPatrolTarget();
                break;
                
            case ToothstalkerState.Pausing:
                currentBehavior = Pause;
                pauseTimer = pauseDuration; // Start the timer
                break;

            case ToothstalkerState.Alert:
                currentBehavior = Alert;
                currentTarget = player.position;
                break;

            case ToothstalkerState.Attacking:
                currentBehavior = Attack;
                toothstalkerAttack.AttachToPlayer();
                break;
            
            case ToothstalkerState.Occupied:
                currentBehavior = Occupied;
                break;

            case ToothstalkerState.Blinded:
                currentTarget = GetRetreatTarget();
                currentBehavior = Blinded;
                break;
        }
    }

    public bool IsAttacking() {
        return currentState == ToothstalkerState.Attacking;
    }
        
    private void Patrol() {
        // Move towards the patrol target
        MoveTowards(currentTarget, baseSpeed);

        PauseIfReachesCurrentTarget();

        // Check for player within alert radius
        if (DistanceFrom(player.position) <= alertRadius)
        {
            SetState(ToothstalkerState.Pausing); // Pause briefly before alert
        }
    }

    private void PauseIfReachesCurrentTarget() {
        if (DistanceFrom(currentTarget) < 0.1f) {
            SetState(ToothstalkerState.Pausing);
        }
    }

    private Vector3 GetNewPatrolTarget() {
        // Parse the current patrol point name to get x, y indices
        int currentX = int.Parse(currentPatrolPoint[0].ToString());
        int currentY = int.Parse(currentPatrolPoint[1].ToString());
        List<string> adjacentPoints = GetAdjacentPatrolPoints(currentX, currentY);
        currentPatrolPoint = ChooseRandom(adjacentPoints);
        return patrolPoints[currentPatrolPoint].position;
    }

    private string ChooseRandom(List<string> list) {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    private List<string> GetAdjacentPatrolPoints(int currentX, int currentY) {
        List<string> adjacentPoints = new();
        List<int[]> deltas = new() {
            new int[] { 0, 1 },  // Right
            new int[] { 0, -1 }, // Left
            new int[] { 1, 0 },  // Down
            new int[] { -1, 0 }  // Up
        };

        foreach (int[] delta in deltas) {
            int newX = currentX + delta[0];
            int newY = currentY + delta[1];

            string newPoint = $"{newX}{newY}";

            // Check if the new point exists in the patrol grid
            if (patrolPoints.ContainsKey(newPoint)) {
                adjacentPoints.Add(newPoint);
            }
        }

        return adjacentPoints;
    }

    private void MoveTowards(Vector3 target, float speed) {
        Vector3 adjustedTarget = target + new Vector3(0, offsetY, 0);
        Vector3 direction = (adjustedTarget - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, adjustedTarget, speed * Time.deltaTime);

        if (direction.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1)*3; // Flip horizontally
        } else if (direction.x > 0) {
            transform.localScale = new Vector3(1, 1, 1)*3; // Default orientation
        }
    }

    private float DistanceFrom(Vector3 target) {
        Vector3 adjustedTarget = target + new Vector3(0, offsetY, 0);
        return Vector3.Distance(transform.position, adjustedTarget);
    }

    private void Pause() {
        // Count down the pause timer
        pauseTimer -= Time.deltaTime;

        // When timer is done, transition based on currentState
        if (pauseTimer <= 0) {
            if (DistanceFrom(player.position) <= alertRadius) {
                SetState(ToothstalkerState.Alert); // Transition to Alert if player is nearby
            } else {
                SetState(ToothstalkerState.Patrolling); // Otherwise, go back to Patrolling
            }
        }
    }


    private void Alert() {
        MoveTowards(currentTarget, baseSpeed*2.5f);
        PauseIfReachesCurrentTarget();
    }

    private Vector3 GetRetreatTarget() {
        // Calculate the retreat direction away from the player
        Vector3 retreatDirection = (transform.position - player.position).normalized;

        // Calculate the perpendicular directions (rotated by 90 degrees)
        Vector3 retreatRight = new Vector3(-retreatDirection.y, retreatDirection.x, 0).normalized; // 90 degrees right
        Vector3 retreatLeft = new Vector3(retreatDirection.y, -retreatDirection.x, 0).normalized;  // 90 degrees left

        // Determine which perpendicular direction is closer to the center (0, 0)
        Vector3 rightDestination = transform.position + retreatRight;
        Vector3 leftDestination = transform.position + retreatLeft;

        // Choose the direction that gets closer to (0, 0)
        Vector3 chosenDirection = (rightDestination.magnitude < leftDestination.magnitude) ? retreatRight : retreatLeft;
        return transform.position + chosenDirection * 2f;
    }

    private void Blinded() {
        // Move in the chosen retreat direction
        MoveTowards(currentTarget, baseSpeed * 6);

        // Schedule return to patrol after retreating
        Invoke(nameof(ReturnToPatrol), 0.5f);
    }

    private void ReturnToPatrol()
    {
        SetState(ToothstalkerState.Patrolling);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && currentState != ToothstalkerState.Attacking){
            SetState(ToothstalkerState.Attacking);
        } else if (other.CompareTag("FlashlightEdges")
        && (currentState == ToothstalkerState.Pausing || currentState == ToothstalkerState.Patrolling)) {
            SetState(ToothstalkerState.Blinded);
        }
    }

    private void Attack() {
        if (toothstalkerAttack.JustFinished()) {
            SetState(ToothstalkerState.Occupied);
        }
    }

    private void Prowl() {
        // TODO
        Patrol();
    }

    private void Occupied() {
        // TODO
        Invoke(nameof(EndOccupied), 2f);
    }

    private void EndOccupied() {
        SetState(ToothstalkerState.Pausing);
    }
}
