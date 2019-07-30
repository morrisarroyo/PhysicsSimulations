using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    //public Collider collider;
    public Collision gameController;
    public Vector3 velocity;
    private Vector3 position;
    // Use this for initialization
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(velocity.x);
        position = position + velocity * Time.fixedDeltaTime;
        transform.position = position;
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        //gameController.collide = true;
        //Debug.Break();
    }
}
