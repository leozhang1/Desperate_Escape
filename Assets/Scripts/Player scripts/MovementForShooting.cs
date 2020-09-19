using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementForShooting : MonoBehaviour
{
    private float speed = DataDriven.PlayerSpeed;
    private Rigidbody2D rb;
    public Camera cam;

    Vector2 movement;

    Vector2 mousePosInWorldUnits;

    public Transform TEST;

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
            if (speed < 0f)
                speed = 0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float xAxis, float yAxis, float speed, Vector3 mousePos)
    {
        // store input points in a vector
        movement = new Vector2(xAxis, yAxis);
        Speed = speed;

        // convert mousePosition's pixel units to world units
        // captures the mouse's position
        mousePosInWorldUnits = cam.ScreenToWorldPoint(mousePos);

        // rotate player to where ever the mouse is
        //transform.Rotate(mousePosInWorldUnits);
    }

    private void FixedUpdate()
    {
        //var distance = Vector2.Distance(transform.position, TEST.position);

        //Debug.Log("distance: " + distance);

        // move your character based off of movement vector
        rb.AddForce(movement * Speed * Time.fixedDeltaTime);

        // ???
        Vector2 lookDirection = mousePosInWorldUnits - rb.position;

        float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;

        rb.rotation = lookAngle;
    }
}
