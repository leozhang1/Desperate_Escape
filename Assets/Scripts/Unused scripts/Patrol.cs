using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    // how fast the ai is patrolling
    public float speed;

    private Rigidbody2D rb;

    // store the coordinates of your waypoints (hint: collection of transforms)
    public Transform[] wayPoints;
    private int wayPointIndex = 0;

    // determine how long the enemy ai should wait at any given waypoint
    public float setWaitTime;
    private float waitTime;


    // Start is called before the first frame update
    void Start()
    {
        // initialize waitTime
        waitTime = setWaitTime;

        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        if (GetComponent<EnemyHealth>().enabled)
        {
            if (Vector2.Distance(transform.position, wayPoints[wayPointIndex % wayPoints.Length].position) < .2f)
            {
                // make necessary adjustments when idle in waypoint position
                // long enough
                if (waitTime <= 0)
                {
                    waitTime = setWaitTime;
                    wayPointIndex++;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }

            MoveAI();

        }
    }

    private void MoveAI()
    {
        // move from current position to randomly assignmed waypoint position
        transform.position = Vector2.MoveTowards(transform.position,
                                        wayPoints[wayPointIndex % wayPoints.Length].position,
                                        speed * Time.deltaTime
                                        );

        Vector2 lookDir = new Vector2(transform.position.x, transform.position.y) - rb.position;

        // makes sure that the angle is calculated correctly
        if ((Vector2)transform.position != rb.position)
        {
            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = lookAngle;
        }
    }
}
