using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // need an animator to access the desire animations
    // public Animator animator;

    // Set the total amount of health the terrorist will have
    private int maxHealth = DataDriven.TerroristMaxHealth;

    [HideInInspector]
    public int Health { get; private set; }

    // Ally detection variables
    public float detectionRadius = 20f;
    public LayerMask enemyLayers;

    void Start()
    {
        Health = maxHealth;
        Physics2D.queriesStartInColliders = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (transform == null) return;

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void TakeDamage(int damage)
    {
        if (damage > 0)
        {
            Health -= damage;

            // Tell all your friends you're about to die, so they'll avenge you
            Collider2D[] enemyBros = Physics2D.OverlapCircleAll(transform.position,
                                        detectionRadius, enemyLayers);

            foreach (Collider2D enemy in enemyBros)
            {
                enemy.GetComponent<EnemyBrains>().avengeMyBro = true;
                print(enemy.name);
            }

            if (Health <= 0)
            {
                Die();
            }
            else
            {
                Particles.Spawn(gameObject, Particles.Type.BloodLight, 0.1f, -0.25f, -1f, gameObject.transform);
            }
        }
    }

    private void Die()
    {
        // Debug.Log("Killed " + gameObject.name);

        // spawns a weapon at the enemy position right before destroying the enemy
        GetComponent<EnemyDrop>().OnDeath();

        Particles.Spawn(gameObject, Particles.Type.BloodHeavy, Vector3.zero);
        this.enabled = false;
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
