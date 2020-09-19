using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.name = DataDriven.explosionName;
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        float timeActive = 0;
        Animator animator = gameObject.GetComponent<Animator>();
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).length);
        while (timeActive < animator.GetCurrentAnimatorStateInfo(0).length)
        {
            if (GameObject.Find("Game Manager").GetComponent<LevelManager>().State != LevelManager.LevelState.Gameplay)
            {
                float explosionSpeed = animator.speed;
                animator.speed = 0;
                AudioSource audio = gameObject.GetComponent<AudioSource>();
                audio.Pause();
                yield return new WaitUntil(() => GameObject.Find("Game Manager").GetComponent<LevelManager>().State == LevelManager.LevelState.Gameplay);
                audio.UnPause();
                animator.speed = explosionSpeed;
            }
            else
                yield return null;
            timeActive += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
