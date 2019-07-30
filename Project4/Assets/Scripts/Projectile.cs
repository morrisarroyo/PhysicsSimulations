﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float stopYDisplacement = 0.05f;
    [SerializeField]
    private float halfBoatLength = 4;
    [SerializeField]
    private Transform gun;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform bullet;
    [SerializeField]
    private Text updatesText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Vector3 targetRange;
    [SerializeField]
    private float flightTime;

    [SerializeField]
    private float initialSpeed;
    [SerializeField]
    //private float firingAngle;
    //[SerializeField]
    private float alpha;
    [SerializeField]
    private float twoAlpha;
    [SerializeField]
    private float gamma;
    [SerializeField]
    //private float landingAngle;
    //[SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private Vector3 displacement;
    [SerializeField]
    private Vector3 velocity;
    private bool isFiring;
    private int updates = 0;
    private float time = 0;
    // Use this for initialization
    void Start()
    {
        isFiring = false;
        gamma = Mathf.Acos(targetRange.z / Mathf.Sqrt(targetRange.x * targetRange.x + targetRange.z * targetRange.z));
        gamma *= Mathf.Rad2Deg;
        twoAlpha = Mathf.Asin(-gravity * Mathf.Sqrt(targetRange.x * targetRange.x + targetRange.z * targetRange.z) / (initialSpeed * initialSpeed));
        twoAlpha *= Mathf.Rad2Deg;
        gun.localRotation = Quaternion.Euler(0, 90 + gamma, -twoAlpha / 2);

        twoAlpha = 180 - twoAlpha;
        alpha = twoAlpha / 2;

        //firingAngle = (Mathf.Asin((-gravity * targetRange.z) / initialSpeed / initialSpeed) / 2) * 180 / Mathf.PI;
        velocity = new Vector3(initialSpeed * Mathf.Sin(alpha * Mathf.Deg2Rad) * Mathf.Sin(gamma * Mathf.Deg2Rad)
            , initialSpeed * Mathf.Cos(alpha * Mathf.Deg2Rad)
            , initialSpeed * Mathf.Sin(alpha * Mathf.Deg2Rad) * Mathf.Cos(gamma * Mathf.Deg2Rad));
        //velocity = new Vector3(0, Mathf.Sin(DegToRad(firingAngle)), Mathf.Cos(DegToRad(firingAngle))) * initialSpeed;

        flightTime = Mathf.Sqrt(targetRange.x * targetRange.x + targetRange.z * targetRange.z) / (initialSpeed * Mathf.Sin(alpha * Mathf.Deg2Rad));
        //flightTime = targetRange.z / velocity.z;
        //gun.localRotation = Quaternion.Euler(0, 90, -firingAngle);
        //gun.position = gun.position + new Vector3(0, Mathf.Sin(DegToRad(firingAngle)), 0);
        target.position = new Vector3(targetRange.x, 0, targetRange.z - halfBoatLength);
        bullet.position = new Vector3(0, 0, -halfBoatLength);
        displacement = bullet.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFiring = true;
        }
        Fire();
    }

    void Fire()
    {
        if (isFiring && (bullet.position.y > stopYDisplacement || velocity.y > 0))
        {
            time += Time.fixedDeltaTime;
            ++updates;
            updatesText.text = "Updates: " + updates;
            timeText.text = "Time: " + time;
            displacement.z += velocity.z * Time.fixedDeltaTime;
            displacement.y += velocity.y * Time.fixedDeltaTime + .5f * gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
            displacement.x += velocity.x * Time.fixedDeltaTime;
            velocity.y += gravity * Time.fixedDeltaTime;
            bullet.transform.position = displacement;

            if (bullet.position.y <= stopYDisplacement && velocity.y <= 0)
            {
                Debug.Log("Finished Firing");
                //landingAngle = 90 - firingAngle;
                Debug.Break();
            }

        }
    }

    float DegToRad(float deg)
    {
        return deg * Mathf.PI / 180;
    }
}

