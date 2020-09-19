using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: FIX ISSUE WITH NOT BEING ABLE TO DRAG WAYPOINTS INTO INSPECTOR FOR THIS SCRIPT
public class PatrolBehavior : StateMachineBehaviour
{
    // how fast the ai is patrolling
    [SerializeField] public float speed;

    // store the coordinates of your waypoints (hint: collection of transforms)
    [SerializeField] public Transform[] wayPoints;
    private int randomIndex;

    // determine how long the enemy ai should wait at any given waypoint
    [SerializeField] public float setWaitTime;
    private float waitTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // initialize waitTime
        waitTime = setWaitTime;

        // pick a random spot to move towards
        randomIndex = UnityEngine.Random.Range(0, wayPoints.Length);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // move from current position to randomly assignmed waypoint position
        animator.transform.position = Vector2.MoveTowards(animator.transform.position,
                                        wayPoints[randomIndex].position,
                                        speed * Time.deltaTime
                                        );

        if (Vector2.Distance(animator.transform.position, wayPoints[randomIndex].position) < .2f)
        {
            // make necessary adjustments when idle in waypoint position
            // long enough
            if (waitTime <= 0)
            {
                // make a new random wayPoint index
                randomIndex = UnityEngine.Random.Range(0, wayPoints.Length);
                waitTime = setWaitTime;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
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
