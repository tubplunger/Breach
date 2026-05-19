using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("State")]
    public EnemyState currentState;

    [Header("References")]
    public Transform player;
    public Transform[] patrolPoints;
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float pointReachDistance = 0.5f;
    public float turnSpeed = 8f;

    [Header("Detection")]
    public float detectionRange = 15f;
    public float attackRange = 8f;
    public float losePlayerRange = 20f;

    [Header("Combat")]
    public float fireCooldown = 1.2f;

    [Header("Pooling")]
    public ObjectPool projectilePool;

    [Header("Debug")]
    public bool showDebugLogs = false;

    private int currentPatrolIndex;
    private float fireTimer;

    void Update()
    {
        if (player == null)
            return;

        UpdateState();
        RunCurrentState();
    }

    void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (distanceToPlayer <= detectionRange)
                {
                    ChangeState(EnemyState.Chase);
                }
                break;

            case EnemyState.Chase:
                if (distanceToPlayer <= attackRange)
                {
                    ChangeState(EnemyState.Attack);
                }
                else if (distanceToPlayer > losePlayerRange)
                {
                    ChangeState(EnemyState.Patrol);
                }
                break;

            case EnemyState.Attack:
                if (distanceToPlayer > attackRange)
                {
                    ChangeState(EnemyState.Chase);
                }
                break;
        }
    }

    void RunCurrentState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        MoveToward(targetPoint.position, patrolSpeed);

        float distanceToPoint = Vector3.Distance(
            new Vector3(transform.position.x, 0f, transform.position.z),
            new Vector3(targetPoint.position.x, 0f, targetPoint.position.z)
        );

        if (distanceToPoint <= pointReachDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void ChasePlayer()
    {
        MoveToward(player.position, chaseSpeed);
    }

    void AttackPlayer()
    {
        LookAtTarget(player.position);

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireCooldown)
        {
            FireProjectile();
            fireTimer = 0f;
        }
    }

    void MoveToward(Vector3 targetPosition, float speed)
    {
        Vector3 targetFlat = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        LookAtTarget(targetFlat);

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetFlat,
            speed * Time.deltaTime
        );
    }

    void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f;

        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            turnSpeed * Time.deltaTime
        );
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile;

        if (projectilePool != null)
        {
            projectile = projectilePool.GetObject(firePoint.position, firePoint.rotation);
        }
        else
        {
            projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        }
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            projectileScript.owner = gameObject;
        }

        if (showDebugLogs)
        {
            Debug.Log(gameObject.name + " fired at player.");
        }
    }

    void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
            return;

        currentState = newState;

        if (showDebugLogs)
        {
            Debug.Log(gameObject.name + " changed state to " + currentState);
        }
    }
}
