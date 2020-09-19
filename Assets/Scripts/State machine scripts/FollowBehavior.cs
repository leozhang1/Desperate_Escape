using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: implement shooting and retreat behaviors DONE
public class FollowBehavior : StateMachineBehaviour
{
    // define an AI following speed and get player's position via a
    // Transform variable
    public float followSpeed;
    public float stoppingDist;
    public float retreatDist;
    public float startTimeBetweenShots;
    public GameObject projectile;

    // apply fixed update
    private Rigidbody2D rb;

    public AudioClip gunSound;


    private Transform playerTransform;
    private float timeBetweenShots;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        timeBetweenShots = startTimeBetweenShots;
    }


    // Enter positive 1 for follow or negative 1 for retreat
    private void AIBehavior(int val, Animator myAnim)
    {
        if (val != -1 && val != 1)
        {
            Debug.LogError("Please enter either -1 or 1 as input to this method");
            return;
        }

        myAnim.transform.position = Vector2.MoveTowards(
                                        myAnim.transform.position,
                                        playerTransform.position,
                                        followSpeed * Time.deltaTime * val);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // calculate distance between player and the enemy this script
        // is attached to
        float distance = Vector2.Distance(animator.transform.position, playerTransform.position);

        if (distance > stoppingDist)
        {
            // keep chasing the player!
            AIBehavior(1, animator);
        }
        else if (distance > stoppingDist && distance > retreatDist)
        {
            // don't move
            animator.transform.position = animator.transform.position;
        }
        else if (distance < retreatDist)
        {
            // player is too close, so retreat!
            AIBehavior(-1, animator);
        }


        if (timeBetweenShots <= 0)
        {
            if (projectile != null)
            {
                Instantiate(projectile, animator.transform.position, Quaternion.identity);
                AudioSource.PlayClipAtPoint(gunSound, Camera.main.transform.position, 10f);
            }

            timeBetweenShots = startTimeBetweenShots;
            Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(),
            GameObject.FindGameObjectWithTag("Terrorist").GetComponent<Collider2D>());
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }


        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool("isFollowing", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
