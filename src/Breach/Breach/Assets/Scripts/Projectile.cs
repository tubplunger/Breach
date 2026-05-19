using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile")]
    public float speed = 20f;
    public int damage = 1;
    public float lifetime = 5f;

    [Header("Ownership")]
    public GameObject owner;

    private float lifeTimer;
    private PooledObject pooledObject;

    void Awake()
    {
        pooledObject = GetComponent<PooledObject>();
    }

    void OnEnable()
    {
        lifeTimer = 0f;
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        lifeTimer += Time.deltaTime;

        if (lifeTimer >= lifetime)
        {
            Despawn();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == owner)
            return;

        Health health = other.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        Despawn();
    }

    void Despawn()
    {
        if (pooledObject != null)
        {
            pooledObject.ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
