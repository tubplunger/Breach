using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int currentHealth;

    [Header("Debug")]
    public bool logDamage = true;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        HitFlash hitFlash = GetComponent<HitFlash>();
        if (hitFlash != null)
        {
            hitFlash.Flash();
        }

        if (logDamage)
        {
            Debug.Log(gameObject.name + " took " + amount + " damage. Health: " + currentHealth + "/" + maxHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (CompareTag("Player"))
        {
            GameManager.Instance.PlayerDied();
        }
        else if (CompareTag("Enemy"))
        {
            GameManager.Instance.EnemyDied();
        }

        Destroy(gameObject);
    }
}
