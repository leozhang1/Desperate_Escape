using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    // need an animator to access the desire animations
    public Animator animator;

    // need the transform to get the position of hitbox
    public Transform hitPoint;

    // need the range to determine how far the hitbox can extend
    public float attackRange = 0.5f;

    // need to differentiate between the enemy and non-enemy objects
    public LayerMask enemyLayers;

    public void Attack()
    {
        // play an attack animation
        if (animator != null)
        {
            animator.SetTrigger("Melee");
        }

        // Detect enemies in range of attack
        Collider2D [] enemies =
            Physics2D.OverlapCircleAll(hitPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemies)
        {
            // Debug.Log("Hit " + enemy.name);

            // Modifies the script attached to each enemy captured
            // in this array.
            // No need to check for null because our layer mask
            // guarantees that each enemy captured in this array
            // will have an "EnemyHealth" script
            if (enemy.GetComponent<AdvancedFollowPlayer>().enabled)
                enemy.GetComponent<EnemyHealth>().TakeDamage(DataDriven.PlayerMeleeDamage);
            else if (enemy.GetComponent<AdvancedPatrol>().enabled)
                enemy.GetComponent<EnemyHealth>().TakeDamage(DataDriven.TerroristMaxHealth);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (hitPoint == null) return;

        Gizmos.DrawWireSphere(hitPoint.position, attackRange);
    }
}
