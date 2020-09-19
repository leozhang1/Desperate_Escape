using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Rigidbody2D player;
    private Transform mainCamera;
    private Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject.transform;

        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        Debug.Log("Player found!");
        newPos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            newPos.Set(player.position.x, player.position.y, mainCamera.position.z); //The Z position never changes in a 2D game.
            mainCamera.SetPositionAndRotation(newPos, mainCamera.rotation);
        }
    }
}
