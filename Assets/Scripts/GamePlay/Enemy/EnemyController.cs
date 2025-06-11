
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public RoutePatrol patrolRoute;
    public float moveSpeed = 35f;
    public float reachThreshold = 0.1f;
    public float patrolSideDistance = 10f;
    public float patrolSideSpeed = 15f; // tốc độ qua lại
    public float waitAtEdgeTime = 1f;
    private int currentWaypointIndex = 0;
    private Vector3 sideOrigin;
    private Animator animator;
    private bool movingRight = true;
    private EnemyState currentState = EnemyState.Idle;
    private float waitTimer = 0f;
    private enum EnemyState
    {
        Idle,
        MoveForward,
        MoveRight,
        MoveLeft,
        WaitAtEdge
    }
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        SetState(EnemyState.Idle);
        if (patrolRoute != null && patrolRoute.GetLength() >= 2)
        {
            currentWaypointIndex = 1;
        }
    }
    void Update()
    {
        if (patrolRoute == null || patrolRoute.GetLength() < 2)
        {
            SetState(EnemyState.Idle);
            return;
        }
        if (currentWaypointIndex >= patrolRoute.GetLength())
        {
            MoveSideToSide();
            return;
        }

        Transform target = patrolRoute.GetWaypoint(currentWaypointIndex);
        if (target == null)
        {
            SetState(EnemyState.Idle);
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, direction, Time.deltaTime * 5f);
        }

        if (Vector3.Distance(transform.position, target.position) < reachThreshold)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= patrolRoute.GetLength())
            {
                sideOrigin = transform.position;
                SetState(movingRight ? EnemyState.MoveRight : EnemyState.MoveLeft);
            }
            else
            {
                SetState(EnemyState.MoveForward);
            }
        }
        else
        {
            if (currentState != EnemyState.MoveForward)
            {
                SetState(EnemyState.MoveForward);
            }
        }
    }
    void MoveSideToSide()
    {
        if (currentState == EnemyState.WaitAtEdge)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitAtEdgeTime)
            {
                movingRight = !movingRight;
                SetState(movingRight ? EnemyState.MoveRight : EnemyState.MoveLeft);
                waitTimer = 0f;
            }
            return;
        }
        float direction = movingRight ? 1f : -1f;
        Vector3 targetPos = sideOrigin + transform.right * direction * patrolSideDistance;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, patrolSideSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            SetState(EnemyState.WaitAtEdge);
            waitTimer = 0f;
        }

    }
    void SetState(EnemyState newState)
    {
        if (currentState == newState) return;

        currentState = newState;


        if (animator != null)
        {
            Debug.Log("Enemy State: " + currentState);
            animator.SetBool("isIdle", false);
            animator.SetBool("isMovingForward", false);
            animator.SetBool("isMovingLeft", false);
            animator.SetBool("isMovingRight", false);
            animator.SetBool("isIdleRightLeft", false);

            switch (newState)
            {
                case EnemyState.Idle:
                    animator.SetBool("isIdle", true);
                    animator.speed = 1f;
                    break;
                case EnemyState.MoveForward:
                    animator.SetBool("isMovingForward", true);
                    animator.speed = 1f;
                    break;
                case EnemyState.MoveLeft:
                    animator.SetBool("isMovingLeft", true);
                    animator.speed = 1f;
                    break;
                case EnemyState.MoveRight:
                    animator.SetBool("isMovingRight", true);
                    animator.speed = 0.6f;
                    break;
                //ll
                case EnemyState.WaitAtEdge:
                    animator.SetBool("isIdleRightLeft", true);
                    animator.speed = 1f;
                    Debug.Log(animator.speed);

                    break;
            }
        }
    }


}

