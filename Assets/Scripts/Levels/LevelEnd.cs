using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private bool SafeToTransition()
    {
        bool isSafeToTransition = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Terrorist");
        foreach (GameObject enemy in enemies)
        {
            // variable is true ONLY when ALL enemies are in patrol mode
            isSafeToTransition &=
                enemy.GetComponent<AdvancedPatrol>().enabled;
        }

        return isSafeToTransition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.gameObject.name);
        if (collision.gameObject == GameObject.Find("Player"))
        {
            if (!SafeToTransition()) return;
            GameObject.Find("Game Manager").GetComponent<Victory>().TriggerVictory();
            GameObject.Find("Game Manager").GetComponent<LevelManager>().State = LevelManager.LevelState.Results;
        }
    }
}
