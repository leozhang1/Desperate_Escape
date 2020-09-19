using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Object " + this.gameObject.name + " collided with " + collision.gameObject.name);
        if (collision.gameObject.name.Equals("Player"))
        {
            OnPlayerTrigger(collision.gameObject);
            Destroy(this.gameObject);
        }
    }

    protected abstract void OnPlayerTrigger(GameObject player);
}
