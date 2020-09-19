using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // set enemy damage per hit
    private int damage = DataDriven.PistolDamage;

    private void Awake()
    {
        // bad practice for instantiating multiple enemies
        // terrorist = GameObject.FindGameObjectWithTag("Terrorist");
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Terrorist"))
        {
            // print("shot enemy");
            collision.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);

            // chase the player
            // collision.gameObject.GetComponent<AdvancedPatrol>().enabled = false;
            // collision.gameObject.GetComponent<AdvancedFollowPlayer>().enabled = true;
        }
        else
        {
            Particles.Spawn(gameObject, Particles.Type.Sparks);
        }

        Destroy(gameObject);
    }
}
