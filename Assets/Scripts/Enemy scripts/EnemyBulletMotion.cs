using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control projectile speed and roughly where it's going
public class EnemyBulletMotion : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    // capture player position
    private Transform player;

    // projectile will target the player
    private Vector2 target;

    float bulletForce = 10f;
    public Transform firePoint;


    // Start is called before the first frame update
    //void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").transform;

    //    // get the position of the player upon instantiation (AKA shot out by the terrorist)
    //    target = new Vector2(player.position.x, player.position.y);

    //    rb = GetComponent<Rigidbody2D>();

    //}

    // Update is called once per frame
    //void Update()
    //{
    //    // stubbornly move towards that target position captured upon instantiation
    //    //transform.position =
    //    //    Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

    //    Vector2 lookDir = (Vector2) player.position - rb.position;

    //    float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

    //    rb.rotation = lookAngle;

    //    rb.AddForce(bulletForce * firePoint.up, ForceMode2D.Impulse);
    //    Debug.Log("instantiated a bullet");
    //    //if (transform.position.x == target.x &&
    //    //    transform.position.y == target.y)
    //    //{
    //    //    Destroy(gameObject);
    //    //}
    //}


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // damage the player
            // Debug.Log("Player damaged");
            collision.gameObject.GetComponent<PlayerStats>().RestoreHealth(-1);

            Destroy(gameObject);
        }
        else /*if (collision.gameObject.CompareTag("wall"))*/
        {
            Particles.Spawn(gameObject, Particles.Type.Sparks);
            Destroy(gameObject);
        }
    }

}
