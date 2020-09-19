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

    // Start is called before the first frame update
    void Start()
    {
        Health = maxHealth;
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

            if (Health <= 0)
            {
                // Tell all your friends you're about to die, so they'll avenge you
                Collider2D[] enemyBros = Physics2D.OverlapCircleAll(transform.position,
                                            detectionRadius, enemyLayers);

                foreach (Collider2D enemy in enemyBros)
                {
                    enemy.GetComponent<EnemySight>().avengeMyBro = true;
                    //if (enemyBros.Length > 1)
                    //    Debug.Log(gameObject.name + "has " + (enemyBros.Length - 1) + " bros near it.");
                }
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
        Debug.Log("Killed " + gameObject.name);

        // spawns a weapon at the enemy position right before destroying the enemy
        GetComponent<EnemyDrop>().OnDeath();

        Particles.Spawn(gameObject, Particles.Type.BloodHeavy, Vector3.zero);
        this.enabled = false;
        // gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
