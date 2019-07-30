using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision10 : MonoBehaviour
{

    [Serializable]
    public class Sphere
    {
        public GameObject sphere;
        public float mass;
        //public Vector3 initPosition;
        public Vector3 position;
        public Vector3 r;
        //public float initSpeed;
        public Vector3 initVelocity;
        public Vector3 velocity;
        public Vector3 initMomentum;
        public Vector3 momentum;
        public Vector3 initKineticEnergy;
        public Vector3 kineticEnergy;
    }

    public float cRestiution;
    public Vector3 j;
    public float jn;
    public Vector3 relativeVelocity;
    public Sphere sphere1;
    public Sphere sphere2;
    public Vector3 collisionNormal;
    public Vector3 collisionTangent;
    public Vector3 totalInitMomentum;
    public Vector3 totalMomentum;
    public Vector3 totalInitKineticEnergy;
    public Vector3 totalKineticEnergy;
    public bool collide = false;

    // Use this for initialization
    void Start()
    {
        relativeVelocity = sphere1.initVelocity - sphere2.initVelocity;
        j = new Vector3(-relativeVelocity.magnitude * (cRestiution + 1) * sphere1.mass * sphere2.mass / (sphere1.mass + sphere2.mass)
            , 0, 0);
        sphere1.velocity = sphere1.initVelocity;
        sphere2.velocity = sphere2.initVelocity;

        sphere1.initMomentum = sphere1.mass * sphere1.initVelocity;
        sphere2.initMomentum = sphere2.mass * sphere2.initVelocity;
        sphere1.initKineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.initVelocity, sphere1.initVelocity);
        sphere2.initKineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.initVelocity, sphere2.initVelocity);
        totalInitMomentum = sphere1.initMomentum + sphere2.initMomentum;
        totalInitKineticEnergy = sphere1.initKineticEnergy + sphere2.initKineticEnergy;
        /*
        sphere1.momentum = sphere1.mass * sphere1.velocity;
        sphere2.momentum = sphere2.mass * sphere2.velocity;
        sphere1.kineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.velocity, sphere1.velocity);
        sphere2.kineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.velocity, sphere2.velocity);
        totalMomentum = sphere1.momentum + sphere2.momentum;
        totalKineticEnergy = sphere1.kineticEnergy + sphere2.kineticEnergy;
        */
        sphere1.sphere.GetComponent<Movement>().velocity = sphere1.initVelocity;
        sphere2.sphere.GetComponent<Movement>().velocity = sphere2.initVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        sphere1.position += sphere1.velocity * Time.fixedDeltaTime;
        sphere2.position += sphere2.velocity * Time.fixedDeltaTime;
        sphere1.sphere.transform.position = sphere1.position;
        sphere2.sphere.transform.position = sphere2.position;

    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        float x = (sphere2.position.z - sphere1.position.z);
        Debug.Log(x);
        float z = Mathf.Sqrt(1 - (x * x));
        sphere1.position = new Vector3(sphere2.position.x - z, 0, sphere1.position.z);
        sphere1.sphere.transform.position = sphere1.position;
        sphere1.r = sphere1.position;
        sphere2.r = sphere2.position;
        collisionNormal = Vector3.Normalize(sphere2.r - sphere1.r);
        collisionTangent = new Vector3(-collisionNormal.z, 0, collisionNormal.x);
        j = new Vector3(-relativeVelocity.magnitude * (cRestiution + 1) * sphere1.mass * sphere2.mass / (sphere1.mass + sphere2.mass)
            , 0, 0);
        jn = Vector3.Dot(j, collisionNormal);
        sphere1.velocity = jn * collisionNormal / sphere1.mass + Vector3.Scale(sphere1.velocity, Vector3.one);
        sphere2.velocity = -jn * collisionNormal / sphere2.mass + Vector3.Scale(sphere2.velocity, Vector3.one);
        sphere1.momentum = sphere1.mass * sphere1.velocity;
        sphere2.momentum = sphere2.mass * sphere2.velocity;
        sphere1.kineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.velocity, sphere1.velocity);
        sphere2.kineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.velocity, sphere2.velocity);
        totalMomentum = sphere1.momentum + sphere2.momentum;
        totalKineticEnergy = sphere1.kineticEnergy + sphere2.kineticEnergy;
        //sphere1.velocity = Vector3.zero;
        //sphere2.velocity = Vector3.zero;

        Debug.Log("Collide");
        Debug.Break();
    }
}
