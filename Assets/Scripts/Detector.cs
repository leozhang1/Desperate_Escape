using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [HideInInspector]
    public bool canSeePlayer, warning;
    private SpriteRenderer sp;

    public float setTime = 5f;
    public float timeToSeePlayer;

    // Start is called before the first frame update
    void Start()
    {
        canSeePlayer = warning = false;
        sp = GetComponent<SpriteRenderer>();
        sp.color = Color.white;
        timeToSeePlayer = setTime;
    }

    private void Update()
    {
        if (warning)
        {
            timeToSeePlayer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            warning = true;
        }

        if (collision.CompareTag("Player") && timeToSeePlayer <= 0f)
        {
            canSeePlayer = true;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            // player is no longer in its view, so reset warning the necessary variables
            warning = false;
            timeToSeePlayer = setTime;
        }
    }

    //IEnumerator TimeToSee()
    //{
    //    yield return new WaitForSeconds(5);
    //}
}
