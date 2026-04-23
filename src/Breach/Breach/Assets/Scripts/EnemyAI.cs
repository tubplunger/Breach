using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform[] patrolPoints;
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Patrol")]
    public float patrolSpeed = 2f;
    public float pointReachDistance = 0.5f;

    [Header("Combat")]
    public float detectionRange = 12f;
    public float attackRange = 10f;
    public float fireCooldown = 1.2f;
    public float turnSpeed = 8f;

    private int currentPatrolIndex;
    private float fireTimer;

    void Update()
    {
        if (player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            EngagePlayer(distanceToPlayer);
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z),
            patrolSpeed * Time.deltaTime
        );

        float distanceToPoint = Vector3.Distance(
            new Vector3(transform.position.x, 0f, transform.position.z),
            new Vector3(targetPoint.position.x, 0f, targetPoint.position.z)
        );

        if (distanceToPoint <= pointReachDistance)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }
    }

    void EngagePlayer(float distanceToPlayer)
    {
        Vector3 lookDirection = (player.position - transform.position).normalized;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        if (distanceToPlayer <= attackRange)
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireCooldown)
            {
                FireProjectile();
                fireTimer = 0f;
            }
        }
    }

    void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
            return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.owner = gameObject;
        }

        Debug.Log(gameObject.name + " fired at player.");
    }
}
