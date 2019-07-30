using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision11 : MonoBehaviour
{

    [Serializable]
    public class Sphere
    {
        public GameObject sphere;
        public float mass;
        //public Vector3 initPosition;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 collisionPosition;
        public Vector3 r;
        public float moi;
        //public float initSpeed;
        public Vector3 initLinearVelocity;
        public Vector3 linearVelocity;
        public Vector3 initLinearMomentum;
        public Vector3 linearMomentum;
        public Vector3 initLinearKineticEnergy;
        public Vector3 linearKineticEnergy;
        public Vector3 initAngulgarVelocity;
        public Vector3 angularVelocity;
        public Vector3 initAngularMomentum;
        public Vector3 angularMomentum;
        public Vector3 initRotationalKineticEnergy;
        public Vector3 rotationalKineticEnergy;
    }

    public float cRestiution;
    public Vector3 j;
    public float jn;
    public Vector3 relativeLinearVelocity;
    public Vector3 collisionPoint;
    public Vector3 collisionNormal;
    public Vector3 collisionTangent;
    public Vector3 totalInitLinearMomentum;
    public Vector3 totalLinearMomentum;
    public Vector3 totalInitLinearKineticEnergy;
    public Vector3 totalLinearKineticEnergy;
    public Vector3 initAngularMomentum;
    public Vector3 angularMomentum;
    public Vector3 totalRotationalKineticEnergy;
    public float totalKineticEnergy;
    public Sphere sphere1;
    public Sphere sphere2;

    // Use this for initialization
    void Start()
    {
        relativeLinearVelocity = sphere1.initLinearVelocity - sphere2.initLinearVelocity;
        //j = new Vector3(-relativeLinearVelocity.magnitude * (cRestiution + 1) * sphere1.mass * sphere2.mass / (sphere1.mass + sphere2.mass)
        //    , 0, 0);
        sphere1.linearVelocity = sphere1.initLinearVelocity;
        sphere2.linearVelocity = sphere2.initLinearVelocity;

        sphere1.initLinearMomentum = sphere1.mass * sphere1.initLinearVelocity;
        sphere2.initLinearMomentum = sphere2.mass * sphere2.initLinearVelocity;
        sphere1.initLinearKineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.initLinearVelocity, sphere1.initLinearVelocity);
        sphere2.initLinearKineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.initLinearVelocity, sphere2.initLinearVelocity);
        totalInitLinearMomentum = sphere1.initLinearMomentum + sphere2.initLinearMomentum;
        totalInitLinearKineticEnergy = sphere1.initLinearKineticEnergy + sphere2.initLinearKineticEnergy;

        sphere1.moi = (1.0f / 12.0f) * sphere1.mass * (1 + 1);
        sphere2.moi = (1.0f / 12.0f) * sphere2.mass * (1 + 1);
        /*
        sphere1.linearMomentum = sphere1.mass * sphere1.linearVelocity;
        sphere2.linearMomentum = sphere2.mass * sphere2.linearVelocity;
        sphere1.linearKineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.linearVelocity, sphere1.linearVelocity);
        sphere2.linearKineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.linearVelocity, sphere2.linearVelocity);
        totalLinearMomentum = sphere1.linearMomentum + sphere2.linearMomentum;
        totalLinearKineticEnergy = sphere1.linearKineticEnergy + sphere2.linearKineticEnergy;
        */
        //sphere1.sphere.GetComponent<Movement>().linearVelocity = sphere1.initLinearVelocity;
        //sphere2.sphere.GetComponent<Movement>().linearVelocity = sphere2.initLinearVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        sphere1.position += sphere1.linearVelocity * Time.fixedDeltaTime;
        sphere2.position += sphere2.linearVelocity * Time.fixedDeltaTime;
        sphere1.rotation += sphere1.angularVelocity * Time.fixedDeltaTime;
        sphere2.rotation += sphere2.angularVelocity * Time.fixedDeltaTime;
        //sphere1.rotation += sphere1.angularVelocity * Time.fixedDeltaTime * Mathf.Rad2Deg;
        //sphere2.rotation += sphere2.angularVelocity * Time.fixedDeltaTime * Mathf.Rad2Deg;

        sphere1.sphere.transform.position = sphere1.position;
        sphere2.sphere.transform.position = sphere2.position;
        sphere1.sphere.transform.rotation = Quaternion.Euler(sphere1.rotation);
        sphere2.sphere.transform.rotation = Quaternion.Euler(sphere2.rotation);
    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {

        //float x = (sphere2.position.z - sphere1.position.z);  
        float x = (sphere2.position.x - sphere1.position.x) - 1;
        //Debug.Log(x);
        //float z = Mathf.Sqrt(1 - (x * x));
        //sphere1.position = new Vector3(sphere2.position.x - (x / 2), 0, sphere1.position.z);
        sphere1.position = new Vector3(sphere1.position.x + (x / 2), 0, sphere1.position.z);
        sphere2.position = new Vector3(sphere2.position.x - (x / 2), 0, sphere2.position.z);
        sphere1.sphere.transform.position = sphere1.position;
        sphere2.sphere.transform.position = sphere2.position;
        sphere1.collisionPosition = sphere1.position;
        sphere2.collisionPosition = sphere2.position;

        collisionPoint = new Vector3(sphere2.collisionPosition.x - .5f, 0, sphere1.collisionPosition.z + (sphere2.collisionPosition.z - sphere1.collisionPosition.z) / 2);
        collisionNormal = new Vector3(1, 0, 0);
        collisionTangent = new Vector3(-collisionNormal.z, 0, collisionNormal.x);


        sphere1.r = collisionPoint - sphere1.collisionPosition;
        sphere2.r = collisionPoint - sphere2.collisionPosition;

        //collisionNormal = Vector3.Normalize(sphere2.r - sphere1.r);
        //j = new Vector3(-relativeLinearVelocity.magnitude * (cRestiution + 1) * sphere1.mass * sphere2.mass / (sphere1.mass + sphere2.mass), 0, 0);

        j = new Vector3(-relativeLinearVelocity.magnitude * (cRestiution + 1) *
            (1 /
                (
                    (1 / sphere1.mass
                    )
                    + (1 / sphere2.mass
                    )
                    + Vector3.Dot
                        (collisionNormal, Vector3.Cross
                            (
                                (Vector3.Cross
                                    (sphere1.r, collisionNormal
                                    ) / sphere1.moi
                                ), sphere1.r
                            )
                        )
                    + Vector3.Dot
                        (collisionNormal, Vector3.Cross
                            (
                                (Vector3.Cross
                                    (sphere2.r, collisionNormal
                                    ) / sphere2.moi
                                ), sphere2.r
                            )
                        )
                )
            )
            , 0, 0);
        jn = Vector3.Dot(j, collisionNormal);
        //
        sphere1.linearVelocity = jn * collisionNormal / sphere1.mass + Vector3.Scale(sphere1.linearVelocity, Vector3.one);
        sphere2.linearVelocity = -jn * collisionNormal / sphere2.mass + Vector3.Scale(sphere2.linearVelocity, Vector3.one);
        //
        //Vector3 ilm1 = new Vector3(sphere1.initLinearMomentum.x, sphere1.initLinearMomentum.x, sphere1.initLinearMomentum.x);
        //Vector3 ilm2 = new Vector3(sphere2.initLinearMomentum.x, sphere2.initLinearMomentum.x, sphere2.initLinearMomentum.x);
        Vector3 ilm1 = sphere1.initLinearMomentum;
        Vector3 ilm2 = sphere2.initLinearMomentum;

        //initAngularMomentum = Vector3.Cross(sphere1.r, ilm1) + Vector3.Cross(sphere2.r, ilm2);
        initAngularMomentum = new Vector3(-sphere1.r.z * sphere1.initLinearMomentum.x, -sphere2.r.z * sphere2.initLinearMomentum.x, (-sphere1.r.z * sphere1.initLinearMomentum.x) + (-sphere2.r.z * sphere2.initLinearMomentum.x));
        //initAngularMomentum = Vector3.Cross(sphere1.r, sphere1.mass * sphere1.linearVelocity) + Vector3.Cross(sphere2.r, sphere2.mass * sphere2.linearVelocity);
        /*Linitial = new Vector3(-LCaseR1.x * p1Initial, -LCaseR2.x * p2Initial, (-LCaseR1.x * p1Initial) + (-LCaseR2.x * p2Initial));
        LFinal1 = new Vector3(-LCaseR1.x * p1Final, inertia1 * omega1.y, (-LCaseR1.x * p1Final) + (inertia1 * omega1.y));
        LFinal2 = new Vector3(-LCaseR2.x * p2Final, inertia2 * omega2.y, (-LCaseR2.x * p2Final) + (inertia2 * omega2.y));
        LFinalTotal = LFinal1.z + LFinal2.z;*/


        sphere1.angularVelocity += Vector3.Cross(sphere1.r, jn * collisionNormal) / sphere1.moi;
        sphere2.angularVelocity += Vector3.Cross(sphere2.r, -jn * collisionNormal) / sphere2.moi;
        sphere1.linearMomentum = sphere1.mass * sphere1.linearVelocity;
        sphere2.linearMomentum = sphere2.mass * sphere2.linearVelocity;

        sphere1.angularMomentum = new Vector3(-sphere1.r.z * sphere1.linearMomentum.x, sphere1.moi * sphere1.angularVelocity.y, (-sphere1.r.z * sphere1.linearMomentum.x) + sphere1.moi * sphere1.angularVelocity.y);
        sphere2.angularMomentum = new Vector3(-sphere2.r.z * sphere2.linearMomentum.x, sphere2.moi * sphere2.angularVelocity.y, (-sphere2.r.z * sphere2.linearMomentum.x) + sphere2.moi * sphere2.angularVelocity.y);


        sphere1.linearKineticEnergy = .5f * sphere1.mass * Vector3.Scale(sphere1.linearVelocity, sphere1.linearVelocity);
        sphere2.linearKineticEnergy = .5f * sphere2.mass * Vector3.Scale(sphere2.linearVelocity, sphere2.linearVelocity);
        sphere1.rotationalKineticEnergy.y = 0.5f * sphere1.moi * sphere1.angularVelocity.y * sphere1.angularVelocity.y;
        sphere2.rotationalKineticEnergy.y = 0.5f * sphere2.moi * sphere2.angularVelocity.y * sphere2.angularVelocity.y;

        totalLinearMomentum = sphere1.linearMomentum + sphere2.linearMomentum;
        totalLinearKineticEnergy = sphere1.linearKineticEnergy + sphere2.linearKineticEnergy;

        totalRotationalKineticEnergy = sphere1.rotationalKineticEnergy + sphere2.rotationalKineticEnergy;

        totalKineticEnergy = totalLinearKineticEnergy.x + totalRotationalKineticEnergy.y;
        //sphere1.linearVelocity = Vector3.zero;
        //sphere2.linearVelocity = Vector3.zero;

        Debug.Log("Collide");
        Debug.Break();
    }
}
