using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stealth : MonoBehaviour
{
    public float rotationSpeed;
    public float distance;

    public LineRenderer lineofSight;
    public Gradient redColor;
    public Gradient greenColor;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.right, distance);

        if (hitInfo.collider != null)
        {
            //Debug.Log("collided with " + hitInfo.collider.name);
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
            lineofSight.SetPosition(1, hitInfo.point);

            lineofSight.colorGradient = redColor;

            if (hitInfo.collider.CompareTag("Player"))
            {
                Destroy(hitInfo.collider.gameObject);
            }
        }
        else
        {
            //Debug.Log("no collide");
            Debug.DrawLine(transform.position,
                transform.position + transform.right * distance, Color.green);
            lineofSight.SetPosition(1, transform.position + transform.right * distance);
            lineofSight.colorGradient = greenColor;
        }

        lineofSight.SetPosition(0, transform.position);
    }
}
