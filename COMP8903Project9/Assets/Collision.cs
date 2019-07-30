using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Serializable]
    public class Sphere
    {
        public GameObject sphere;
        public float mass;
        public Vector3 velocity;
        public Vector3 initVelocity;
        public float momementum;
        public float initMomentum;
    }

    public float cRestiution;
    public float j;
    public Vector3 relativeVelocity;
    public Sphere sphere1;
    public Sphere sphere2;
    public float totalInitMomentum;
    public bool collide = false;
    // Use this for initialization
    void Start()
    {
        relativeVelocity.x = sphere1.initVelocity.x - sphere2.initVelocity.x;
        j = -relativeVelocity.x * (cRestiution + 1) * sphere1.mass * sphere2.mass / (sphere1.mass + sphere2.mass);
        sphere1.velocity.x = j / sphere1.mass + sphere1.initVelocity.x;
        sphere2.velocity.x = -j / sphere2.mass + sphere2.initVelocity.x;
        sphere1.initMomentum = sphere1.mass * sphere1.initVelocity.x;
        sphere2.initMomentum = sphere2.mass * sphere2.initVelocity.x;
        totalInitMomentum = sphere1.initMomentum + sphere2.initMomentum;
        sphere1.momementum = sphere1.mass * sphere1.velocity.x;
        sphere2.momementum = sphere2.mass * sphere2.velocity.x;
        sphere1.sphere.GetComponent<Movement>().velocity = sphere1.initVelocity;
        sphere2.sphere.GetComponent<Movement>().velocity = sphere2.initVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (collide)
        {
            sphere1.sphere.GetComponent<Movement>().velocity = sphere1.velocity;
            sphere2.sphere.GetComponent<Movement>().velocity = sphere2.velocity;
        }
    }
}
