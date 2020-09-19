using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreColl : MonoBehaviour
{

    private void FixedUpdate()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("bullet").GetComponent<Collider2D>(),
            GetComponent<Collider2D>());
    }



}
