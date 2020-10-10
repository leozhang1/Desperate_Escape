using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: make sure enemy can't see player thru the walls DONE
// TRY USING RAYCASTS
public class EnemyBrains : MonoBehaviour
{
    public GameObject eyes;
    public GameObject leftEyes;
    public GameObject rightEyes;

    public GameObject viewCone;
    public float fieldOfViewAngle = 45f;
    public float lookRadius = 8f;

    public Gradient redColor;
    public Gradient greenColor;
    public float setTimeNotSeeingPlayer = 10f;
    public LayerMask playerDetection;


    float timeNotSeeingPlayer;
    bool canSeePlayer = false;
    public bool avengeMyBro { get; set; }
    GameObject player;

    Vector2 basePt;
    float deltaX;
    float deltaY;
    Vector2 rightPt;
    Vector2 centerPt;
    Vector2 leftPt;



    private void Awake()
    {
        timeNotSeeingPlayer = setTimeNotSeeingPlayer;

        // PATROL AND FOLLOW PLAYER SCRIPTS SHOULD NOT BE BOT ENABLED AT THE SAME TIME
        // start out patroling
        if (!GetComponent<AdvancedPatrol>().enabled)
        {
            GetComponent<AdvancedPatrol>().enabled = true;
        }

        // disable follow player from last game startup
        if (GetComponent<AdvancedFollowPlayer>().enabled)
        {
            GetComponent<AdvancedFollowPlayer>().enabled = false;
        }

        basePt = eyes.transform.position;
        deltaX = lookRadius * Mathf.Sin((fieldOfViewAngle / 2) * Mathf.Deg2Rad);
        deltaY = lookRadius * Mathf.Cos((fieldOfViewAngle / 2) * Mathf.Deg2Rad);
        rightPt = new Vector2(basePt.x + deltaX, basePt.y + deltaY);
        centerPt = new Vector2(basePt.x, basePt.y + lookRadius);
        leftPt = new Vector2(basePt.x - deltaX, basePt.y + deltaY);
    }

    private void OnDrawGizmos()
    {
        basePt = eyes.transform.position;
        deltaX = lookRadius * Mathf.Sin((fieldOfViewAngle / 2) * Mathf.Deg2Rad);
        deltaY = lookRadius * Mathf.Cos((fieldOfViewAngle / 2) * Mathf.Deg2Rad);
        rightPt = new Vector2(basePt.x + deltaX, basePt.y + deltaY);
        centerPt = new Vector2(basePt.x, basePt.y + lookRadius);
        leftPt = new Vector2(basePt.x - deltaX, basePt.y + deltaY);

        Gizmos.DrawLine(basePt, rightPt);
        Gizmos.DrawLine(rightPt, centerPt);
        Gizmos.DrawLine(centerPt, leftPt);
        Gizmos.DrawLine(basePt, leftPt);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.queriesStartInColliders = false;
    }

    void Update()
    {
        if (avengeMyBro)
        {
            ChasePlayer();
            avengeMyBro = false;
        }

        // *** RAYCAST Code begin *** ///

        // center raycast
        RaycastHit2D hitInfo =
            Physics2D.Raycast(transform.position, transform.up,
            lookRadius, playerDetection);

        // left diagonal raycast
        RaycastHit2D hitInfoLeft =
            Physics2D.Raycast(leftEyes.transform.position,
            leftEyes.transform.up, lookRadius, playerDetection);

        // right diagonal raycast
        RaycastHit2D hitInfoRight =
            Physics2D.Raycast(rightEyes.transform.position,
            rightEyes.transform.up, lookRadius, playerDetection);

        leftRayCast(hitInfoLeft);
        CenterRayCast(hitInfo);
        rightRayCast(hitInfoRight);

        // coding defensively
        if (player == null)
        {
            // go back to patrol mode because player is dead
            PatrolMode();
            return;
        }
        // *** RAYCAST Code end *** ///

        // if player's gunshots are no longer applied with a silencer,
        // then all the terrorists will be attracted to the player
        if (player.GetComponent<Shooting>().IsFiring && player.GetComponent<Shooting>().SilencerBroken)
        {
            if (!GetComponent<AdvancedFollowPlayer>().enabled)
            {
                ChasePlayer();
            }
        }

        // exhaust this if-statement to trigger the following if-statement
        if (!canSeePlayer)
        {
            timeNotSeeingPlayer -= Time.deltaTime;
        }

        // if cant find player, then go back to patrolling
        if (timeNotSeeingPlayer <= 0)
        {
            if (!GetComponent<AdvancedPatrol>().enabled)
            {
                PatrolMode();
            }
            hitInfo = Physics2D.Raycast(transform.position, transform.up, lookRadius);
            hitInfoLeft = Physics2D.Raycast(leftEyes.transform.position, leftEyes.transform.up, lookRadius);
            hitInfoRight = Physics2D.Raycast(rightEyes.transform.position, rightEyes.transform.up, lookRadius);
        }

        var viewDirection = transform.up;
        Vector3 translationVector = player.transform.position - eyes.transform.position;
        if (translationVector.magnitude > lookRadius)
        {
            return;
        }

        translationVector = translationVector.normalized;

        var result = Vector3.Dot(viewDirection, translationVector);

        if (Mathf.Acos(result) * Mathf.Rad2Deg < (fieldOfViewAngle / 2))
        {
            Debug.DrawLine(eyes.transform.position, player.transform.position);
        }
    }

    void CenterRayCast(RaycastHit2D raycast)
    {
        // if the ray hits something
        if (raycast.collider != null)
        {
            Debug.DrawLine(transform.position, raycast.point, Color.red);
            //Debug.DrawLine(transform.position, hitInfoLeft.point, Color.red);
            //Debug.DrawLine(transform.position, hitInfoRight.point, Color.red);

            //lineofSight.SetPosition(0, transform.position);
            //lineofSight.SetPosition(1, hitInfo.point);

            if (raycast.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
                // Debug.Log("Sees player");

                // if player is close enough,
                // then start firing at the player. If the player is far enough, then stop shooting
                //if (distFromPlayer < 20f)
                //{
                //    GetComponent<ShootPlayer>().enabled = true;
                //}
                //else
                //{
                //    GetComponent<ShootPlayer>().enabled = false;
                //}

                // each time this gets called, the enemy raycast will be longer and longer
                // posing as a bigger threat to the player
                ChasePlayer();

                GetComponent<ShootPlayer>().enabled = true;
                //lineofSight.colorGradient = redColor;
            }
            else
            {
                //lineofSight.colorGradient = greenColor;
                canSeePlayer = false;
                GetComponent<ShootPlayer>().enabled = false;
            }
        }
        else // the ray doesn't hit anything
        {
            canSeePlayer = false;
            GetComponent<ShootPlayer>().enabled = false;
            Debug.DrawLine(transform.position,
                transform.position + transform.up * lookRadius, Color.green);

            //Debug.DrawLine(leftEyes.transform.position,
            //    leftEyes.transform.position + leftEyes.transform.up * lookRadius, Color.blue);

            //Debug.DrawLine(rightEyes.transform.position,
            //    rightEyes.transform.position + rightEyes.transform.up * lookRadius, Color.blue);

            //Debug.DrawLine(transform.position,
            //    transform.position + rightEyes.transform.up * lookRadius, Color.blue);
            // lineofSight.SetPosition(0, transform.position);
            // lineofSight.SetPosition(1, transform.position + transform.right * lookRadius);
            //lineofSight.colorGradient = greenColor;
        }
    }

    void leftRayCast(RaycastHit2D raycast)
    {
        // if the ray hits something
        if (raycast.collider != null)
        {
            Debug.DrawLine(transform.position, raycast.point, Color.red);

            if (raycast.collider.CompareTag("Player"))
            {
                canSeePlayer = true;

                // each time this gets called, the enemy raycast will be longer and longer
                // posing as a bigger threat to the player
                ChasePlayer();

                GetComponent<ShootPlayer>().enabled = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else // the ray doesn't hit anything
        {
            canSeePlayer = false;

            Debug.DrawLine(leftEyes.transform.position,
                leftEyes.transform.position + leftEyes.transform.up * lookRadius, Color.blue);
        }
    }

    void rightRayCast(RaycastHit2D raycast)
    {
        // if the ray hits something
        if (raycast.collider != null)
        {
            Debug.DrawLine(transform.position, raycast.point, Color.red);

            if (raycast.collider.CompareTag("Player"))
            {
                canSeePlayer = true;
                // Debug.Log("Sees player");

                // each time this gets called, the enemy raycast will be longer and longer
                // posing as a bigger threat to the player
                ChasePlayer();

                GetComponent<ShootPlayer>().enabled = true;
            }
            else
            {
                canSeePlayer = false;
                //GetComponent<ShootPlayer>().enabled = false;
            }
        }
        else // the ray doesn't hit anything
        {
            canSeePlayer = false;
            //GetComponent<ShootPlayer>().enabled = false;

            Debug.DrawLine(rightEyes.transform.position,
                rightEyes.transform.position + rightEyes.transform.up * lookRadius, Color.blue);
        }
    }

    void ChasePlayer()
    {
        // the player has been spotted!

        // view range is increased
        // if (lookRadius < 16f)
        lookRadius = 32f;
        canSeePlayer = true;
        timeNotSeeingPlayer = setTimeNotSeeingPlayer;
        GetComponent<AdvancedPatrol>().enabled = false;
        GetComponent<AdvancedFollowPlayer>().enabled = true;
        viewCone.GetComponent<SpriteRenderer>().enabled = false;
    }

    void PatrolMode()
    {
        // go back to patrolling
        lookRadius = 8f;
        GetComponent<AdvancedPatrol>().enabled = true;
        GetComponent<ShootPlayer>().enabled = false;
        GetComponent<AdvancedFollowPlayer>().enabled = false;
        viewCone.GetComponent<SpriteRenderer>().enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("bullet"))
        {
            ChasePlayer();
        }
    }

    //private IEnumerator ChasingOutOfSightPlayer()
    //{
    //    yield return new WaitForSeconds(setTimeNotSeeingPlayer);
    //    Debug.Log("BACK TO PATROL");
    //    GetComponent<AdvancedPatrol>().enabled = true;
    //    GetComponent<AdvancedFollowPlayer>().enabled = false;
    //}
}
