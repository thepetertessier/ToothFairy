using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ToothstalkerState
{
    Patrolling,
    Pausing,
    Alert,
    Prowling,
    Blinded,
    Attacking,
    Occupied
}

public class ToothstalkerAI : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 1f;
    [SerializeField] private float pauseDuration = 2f; // time in seconds to pause
    [SerializeField] private float attackDuration = 3f; // time in seconds to attack

    [SerializeField] private float alertRadius = 1f; // detection radius for player
    private readonly float offsetY = 0.37f;

    private ToothstalkerState currentState;
    private Action currentBehavior;
    private float pauseTimer;
    private float attackTimer;
    private Vector3 currentTarget;  // Target location for patrolling

    private Dictionary<string, Transform> patrolPoints;
    private string currentPatrolPoint;


    private Transform player;  // Reference to the player
    private Animator animator;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        patrolPoints = new Dictionary<string, Transform>();
        Transform patrolPointsParent = GameObject.Find("PatrolPoints").transform;
        
        foreach (Transform patrolPoint in patrolPointsParent) {
            patrolPoints[patrolPoint.name] = patrolPoint;
        }

        currentPatrolPoint = "11";
        currentTarget = patrolPoints[currentPatrolPoint].position;

        SetState(ToothstalkerState.Patrolling);
    }

    private void Update()
    {
        currentBehavior?.Invoke();        
        animator.SetInteger("State", GetAnimatorStateValue(currentState));
    }

    // private Action GetStateAction(ToothstalkerState state) {
    //     return state switch
    //     {
    //         ToothstalkerState.Patrolling => Patrol,
    //         ToothstalkerState.Pausing => Pause,
    //         ToothstalkerState.Alert => Alert,
    //         ToothstalkerState.Prowling => Prowl,
    //         ToothstalkerState.Blinded => Blinded,
    //         ToothstalkerState.Attacking => Attack,
    //         ToothstalkerState.Occupied => Occupied,
    //         _ => Patrol,
    //     };
    // }

    /*
     * 0 standing
     * 1 walking
     * 2 attacking
     * 3 dying (blinded)
     */
    private int GetAnimatorStateValue(ToothstalkerState state) {
        return state switch
        {
            ToothstalkerState.Patrolling => 1,
            ToothstalkerState.Pausing => 0,
            ToothstalkerState.Alert => 1,
            ToothstalkerState.Prowling => 1,
            ToothstalkerState.Blinded => 3,
            ToothstalkerState.Attacking => 2,
            ToothstalkerState.Occupied => 2,
            _ => 0,
        };
    }

    private void SetState(ToothstalkerState newState) {
        // if (currentState == newState) {
        //     return; // without reseting anything
        // }

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
                attackTimer = attackDuration;
                break;
            
            case ToothstalkerState.Occupied:
                currentBehavior = Occupied;
                break;
        }

        Debug.Log($"Set state: {currentState}");
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

    private void Blinded()
    {
        // Add logic for moving away or hiding
        Vector3 retreatDirection = (transform.position - player.position).normalized;
        MoveTowards(transform.position + retreatDirection * 2f, baseSpeed*3);  // Move back a bit
        Invoke(nameof(ReturnToPatrol), 0.5f);
    }

    private void ReturnToPatrol()
    {
        SetState(ToothstalkerState.Patrolling);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player") && currentState != ToothstalkerState.Attacking){
            SetState(ToothstalkerState.Attacking);
        }
    }

    private void Attack() {
        // Logic for damaging the player and waiting for the attack animation or time limit to end
        // For example, reduce health over time, or trigger a “break free” mechanic
        // attackTimer -= Time.deltaTime;

        // if (attackTimer <= 0) {
        //     EndAttack();
        // }

        Invoke(nameof(EndAttack), 0.1f);
    }

    private void EndAttack()
    {
        SetState(ToothstalkerState.Occupied);
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
