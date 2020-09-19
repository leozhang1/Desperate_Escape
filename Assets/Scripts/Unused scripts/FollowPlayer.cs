using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// PLEASE DISABLE THIS SCRIPT WHEN USING FOLLOW BEHAVIOR

// Follows player until a certain distance, retreats until a certain distance is
// large enough, shoots player at all times
public class FollowPlayer : MonoBehaviour
{
    public float speed;
    public float stoppingDist;
    public float retreatDist;
    public float startTimeBetweenShots;
    public GameObject projectile;

    private Rigidbody2D rb;

    private Transform target;
    private float timeBetweenShots;


    // TODO: make sure projectiles shot dont go thru walls 'DONE'

    // Start is called before the first frame update
    void Start()
    {
        // find the player this way
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timeBetweenShots = startTimeBetweenShots;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, target.position);

        // keep moving towards player until Vector2.Distance() == distance
        if (distance > stoppingDist)
        {
            // track where the player is at all times but stop when it gets close enough
            AIBehavior(1);
        }
        else if (distance > stoppingDist && distance > retreatDist)
        {
            transform.position = transform.position; // don't move
        }
        else if (distance < retreatDist) // player is too close to enemy
        {
            AIBehavior(-1);
        }

        // add a check for being able to see the player or not in this if-block
        if (timeBetweenShots <= 0)
        {

            if (projectile != null)
                Instantiate(projectile, transform.position, Quaternion.identity);
            timeBetweenShots = startTimeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        // move from current position to randomly assignmed waypoint position
        Transform playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Vector2 lookDir = new Vector2(playerPos.position.x, playerPos.position.y) - rb.position;

        float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = lookAngle;
    }

    // Enter positive 1 for follow or negative 1 for retreat
    private void AIBehavior(int val)
    {
        if (val != -1 && val != 1)
        {
            Debug.LogError("Please enter either -1 or 1 as input to this method");
            return;
        }
        transform.position = Vector2.MoveTowards(
                                        transform.position,
                                        target.position,
                                        speed * Time.deltaTime * val);
    }
}
