using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class AdvancedPatrol : MonoBehaviour
{
    public float speed;

    // array of waypoints
    public Transform[] wayPoints;

    // DONT REALLY KNOW
    public float nextWayPointDist = 3f;

    private Path path;
    private int wayPointIndex = 0;
    private int vectorPathIndex = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    Quaternion spawnRot;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        spawnRot = transform.rotation;

        InvokeRepeating("UpdatePath", .5f,
            Vector2.Distance(rb.position, wayPoints[wayPointIndex % wayPoints.Length].position) / 2f > 2.5f ? 8f : 5f);
    }

    // void OnCollisionEnter2D(Collision2D collisionInfo)
    // {
    //     // if get hit by a bullet, immediately chase the player
    //     if (collisionInfo.collider.CompareTag("bullet"))
    //     {
    //         print("shot enemy!!");
    //         GetComponent<AdvancedFollowPlayer>().enabled = true;
    //         this.enabled = false;
    //         print("should chase");
    //     }
    // }

    void UpdatePath()
    {
        // may need to change this condition to check for how close the enemy is to the current waypoint DONE
        if (seeker.IsDone())
        {
            Debug.Log("wayPointIndex: " + wayPointIndex);

            // wait for couple seconds
            //StartCoroutine(IdleEnemy());
            seeker.StartPath(rb.position, wayPoints[wayPointIndex % wayPoints.Length].position, OnPathComplete);
        }
        else
        {
            Debug.Log(wayPointIndex % wayPoints.Length + " is not reached yet");
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            //Debug.Log("resetting vectorPathIndex to 0 and incrementing wayPoint");
            vectorPathIndex = 0;

            if (wayPoints.Length > 1)
            {
                ++wayPointIndex;
            }
        }
        else
        {
            Debug.Log("couldn't complete path calculation");
        }
    }

    void FixedUpdate()
    {
        if (path == null)
        {
            // get out if there's no valid path to patrol
            return;
        }

        // if we traversed the entire path
        if (vectorPathIndex >= path.vectorPath.Count)
        {
            //Debug.Log("vectorPath index: " + vectorPathIndex);
            //Debug.Log("vectorPath size: " + path.vectorPath.Count);
            //Debug.Log("reached one of the waypoints");
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        float distToWayPt = Vector2.Distance(rb.position, wayPoints[wayPointIndex % wayPoints.Length].position);
        if (distToWayPt < .5f && wayPoints.Length == 1)
        {
            // Quaternion rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward).normalized;
            //Debug.Log("back in place");
            transform.rotation = spawnRot;
            return;
        }


        if ((Vector2)path.vectorPath[vectorPathIndex] != rb.position)
        {
            // vector2 that points from where the enemy currently is to the specified waypoint
            Vector2 lookDir = ((Vector2)path.vectorPath[vectorPathIndex] - rb.position).normalized;

            // force to apply to the enemy rigidbody
            Vector2 force = lookDir * speed * Time.deltaTime;
            rb.AddForce(force);

            float lookAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = lookAngle;
        }

        // distance that calculates how far the enemy will move
        float distance = Vector2.Distance(rb.position, path.vectorPath[vectorPathIndex]);

        if (distance < nextWayPointDist)
        {
            // wait for couple seconds
            //StartCoroutine(IdleEnemy());

            // increment
            ++vectorPathIndex;
            //Debug.Log("incrementing vectorPathIndex");
        }
    }

    IEnumerator IdleEnemy()
    {
        yield return new WaitForSeconds(10f);
    }
}


