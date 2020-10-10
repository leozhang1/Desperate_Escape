using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// searches and follows the player
public class AdvancedFollowPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 2000f;
    public float curWayPointDist = 1f;

    public float stoppingDist = 9f;
    public float retreatDist = 5f;


    Path path;
    int wayPointIndex = 0;
    bool reachedEndOfPath = false;
    bool tooClose = false;
    Seeker seeker;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            wayPointIndex = 0;
        }
        else
        {
            // Debug.Log("couldn't complete path calculation");
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        float distFromPlayer = Vector2.Distance(transform.position, target.position);

        if (path == null)
        {
            return;
        }

        if (wayPointIndex >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return; // how is it that we come back in here after returning out??????
        }
        else
        {
            reachedEndOfPath = false;
        }

        // vector2 that points from where the enemy currently is to the specified waypoint
        Vector2 pathDir = ((Vector2)path.vectorPath[wayPointIndex] - rb.position).normalized;

        Vector2 playerDir = ((Vector2)target.position - rb.position).normalized;

        float pathFindingAngle = Mathf.Atan2(pathDir.y, pathDir.x) * Mathf.Rad2Deg - 90f;

        float playerFindingAngle = Mathf.Atan2(playerDir.y, playerDir.x) * Mathf.Rad2Deg - 90f;


        // KEEPS SOME DISTANCE FROM THE PLAYER AND RETREATS IF NECESSARY
        // Keep moving towards player until Vector2.Distance() == distance

        // pathfinds to the player
        if (distFromPlayer > stoppingDist)
        {
            print("chasing player");
            // force to apply to the enemy rigidbody
            rb.rotation = pathFindingAngle;
            Vector2 force = pathDir * speed * Time.deltaTime;
            rb.AddForce(force);

            // distance that calculates how far the enemy will move
            float distance = Vector2.Distance(rb.position, path.vectorPath[wayPointIndex]);

            if (distance < curWayPointDist)
            {
                ++wayPointIndex;
            }
        }
        else if (distFromPlayer < stoppingDist && distFromPlayer > retreatDist)
        {
            print("idling");
            transform.position = transform.position; // don't move
        }
        else if (distFromPlayer < retreatDist) // player is too close to enemy
        {
            print("retreating");
            // force to apply to the enemy rigidbody
            rb.rotation = playerFindingAngle;
            Vector2 force = playerDir * -speed * Time.deltaTime;
            rb.AddForce(force);
        }



    }
}
