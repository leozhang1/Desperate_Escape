using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();

        rb = gameObject.GetComponent<Rigidbody2D>();
        animator.speed = rb.velocity.magnitude;
        // Debug.Log(rb.rotation);

        StartCoroutine(Explode(DataDriven.GrenadeTimer));
    }

    private void Update()
    {
        GameObject player = GameObject.Find("Player");
        Debug.DrawRay(transform.position, player.transform.position - transform.position);
    }

    public static void Spawn(GameObject player)
    {
        GameObject grenade = Instantiate(Resources.Load<GameObject>("Prefabs/Grenade Thrown"), player.transform.position, player.transform.rotation);
        grenade.name = DataDriven.thrownObjName;
        grenade.transform.Rotate(0, 0, 90);
        grenade.GetComponent<Rigidbody2D>().AddForce(player.transform.up * DataDriven.GrenadeInitVelocity, ForceMode2D.Impulse);
    }

    private IEnumerator Explode(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        Particles.Spawn(gameObject, Particles.Type.Explosion);

        float damage;
        RaycastHit2D raycast;

        //Damage the player.
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            raycast = Physics2D.Raycast(gameObject.transform.position, player.transform.position - gameObject.transform.position, DataDriven.GrenadeRadius, LayerMask.GetMask("Player", "Obstacle"));
            //Layer 13 is Obstacle.
            if (raycast.collider != null)
                // Debug.Log(player.name + " : " + raycast.collider.gameObject.name);
            if (raycast.collider == null || raycast.collider.gameObject.layer != 13)
            {
                damage = CalculateDamage(player);
                GameObject.Find("Player").GetComponent<PlayerStats>().RestoreHealth((int)(-1 * damage));
            }
        }


        //Damage terrorists.
        GameObject[] terrorists = GameObject.FindGameObjectsWithTag("Terrorist");
        foreach (GameObject terrorist in terrorists)
        {
            raycast = Physics2D.Raycast(gameObject.transform.position, terrorist.transform.position - gameObject.transform.position, DataDriven.GrenadeRadius, LayerMask.GetMask("Terrorists", "Obstacle"));
            //Layer 13 is Obstacle.
            if (raycast.collider != null)
                // Debug.Log(terrorist.name + " : " + raycast.collider.gameObject.name);
            if (raycast.collider == null || raycast.collider.gameObject.layer != 13)
            {
                damage = CalculateDamage(terrorist);
                terrorist.GetComponent<EnemyHealth>().TakeDamage((int)damage);
            }
        }

        Destroy(gameObject);
    }

    private float CalculateDamage(GameObject entity)
    {
        return DataDriven.GrenadeDamage * Mathf.Pow(DataDriven.GrenadeRadius / MathUtils.DistanceBetweenGameObjects(gameObject, entity), Mathf.Pow(DataDriven.GrenadeDamage, 7f/11f));
    }
}
