using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    [SerializeField] private GameObject victory;
    
    public void TriggerVictory()
    {
        victory.SetActive(true);
        StartCoroutine(FreezeGameplay(0.01f));
    }

    IEnumerator FreezeGameplay(float timeScaleDelta)
    {
        Rigidbody2D rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        while (Time.timeScale > 0)
        {
            if (Time.timeScale - timeScaleDelta <= 0)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale -= timeScaleDelta;
            }

            rb.AddForce(rb.velocity * -1f, ForceMode2D.Impulse);
            yield return null;
        }
    }
}
