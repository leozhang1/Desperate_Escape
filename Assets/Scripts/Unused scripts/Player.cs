using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: base character is always idling up
// laser showing in scene but not showing in game
public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    [SerializeField] GameObject particles;
    [SerializeField] public Animator animator;

    // this vector tracks the position of the gameobject by a point at all times
    private Vector2 mv;

    void Start()
    {
        // we access the gameobject's rigidbody 2d component, if it has one
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 mi = new Vector2(Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"));

        // normalized makes this vector have a magnitude of 1.
        // update the class member vector2, 'mv', according to player inputs
        mv = mi.normalized * speed;

        animator.SetFloat("Horizontal", mi.x);
        animator.SetFloat("Vertical", mi.y);
        animator.SetFloat("Speed", mi.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + mv * Time.fixedDeltaTime);
    }

    public void DestroyPlayer()
    {
        Destroy(gameObject);
        GameObject sparkles =
                Instantiate(particles, transform.position, transform.rotation);
        Destroy(sparkles, 1f);
    }
}

