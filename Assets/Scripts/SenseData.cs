using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenseData : MonoBehaviour
{
    public GameObject player { get; set; }

    [SerializeField] public float viewCone;

    // readonly
    public Vector2 lookDir
    {
        get { return transform.up; }
    }


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // TODO: get light component and set its angle and look direction
    }
}
