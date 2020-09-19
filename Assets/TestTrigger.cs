using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrigger : MonoBehaviour
{
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(flag = !flag);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(flag = !flag);
    }
}
