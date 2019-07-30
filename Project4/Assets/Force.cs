using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Force : MonoBehaviour
{
    [SerializeField]
    private Text updatesText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Transform hull;
    [SerializeField]
    private Vector3 boatTransform = Vector3.zero;
    [SerializeField]
    private Vector3 boatRotation = Vector3.zero;
    [SerializeField]
    private float laForce;
    [SerializeField]
    private float forceAngle;
    [SerializeField]
    private Vector3 laThrustForce;
    [SerializeField]
    private Vector3 laAcceleration;
    [SerializeField]
    private Vector3 laVelocity;
    [SerializeField]
    private Vector3 laVelocityFinal;
    [SerializeField]
    private float laTime;
    [SerializeField]
    private float displacementFinal;
    [SerializeField]
    private float aaForce;
    [SerializeField]
    private Vector3 aaThrust;
    [SerializeField]
    private Vector3 aaLeftR;
    [SerializeField]
    private Vector3 aaRightR;
    [SerializeField]
    private Vector3 aaLeftTorque;
    [SerializeField]
    private Vector3 aaRightTorque;
    [SerializeField]
    private Vector3 aaLeftAcceleration;
    [SerializeField]
    private Vector3 aaRightAcceleration;
    [SerializeField]
    private Vector3 aaLeftVelocityFinal;
    [SerializeField]
    private Vector3 aaRightVelocityFinal;
    [SerializeField]
    private Vector3 aaLeftDisplacementFinal;
    [SerializeField]
    private Vector3 aaRightDisplacementFinal;
    [SerializeField]
    private float aaLeftTimeFinal;
    [SerializeField]
    private float aaRightTimeFinal;
    [SerializeField]
    private Vector3 aaVelocity;
    [SerializeField]
    private Vector3 aaDisplacement;
    [SerializeField]
    private bool fluidDynamicDrag;
    [SerializeField]
    private float fddDragCoefficcient;
    [SerializeField]
    private float fddTotalTime;
    [SerializeField]
    private Vector3 fddVelocityMax;
    [SerializeField]
    private Vector3 fddAcceleration;
    [SerializeField]
    private Vector3 fddVelocity;
    [SerializeField]
    private Vector3 fddDisplacement;
    private int updates = 0;
    private float timer = 0;
    private bool isLAForcing;
    private bool isAAForcingLeft;
    private bool isAAForcingRight;
    private BoatMomentOfInertia boatMOI;

    // Use this for initialization
    void Start()
    {
        boatMOI = gameObject.GetComponent<BoatMomentOfInertia>();
        // Linear Acceleration
        laThrustForce.z = laForce * Mathf.Cos(forceAngle * Mathf.Deg2Rad);
        laThrustForce.x = laForce * Mathf.Sin(forceAngle * Mathf.Deg2Rad);
        if (fluidDynamicDrag)
        {
            fddVelocityMax.z = laForce / fddDragCoefficcient;
            fddAcceleration.z = (laForce - fddDragCoefficcient * fddVelocity.z) / boatMOI.mass.total;
        }
        else
        {
            laAcceleration.x = laThrustForce.x / boatMOI.mass.total;
            laAcceleration.z = laThrustForce.z / boatMOI.mass.total;
        }
        laTime = Mathf.Sqrt((2 * displacementFinal) / laAcceleration.z);
        boatTransform = transform.position;
        isLAForcing = false;
        // Angular Acceleration

        isAAForcingLeft = false;
        isAAForcingRight = false;
        aaThrust = new Vector3(aaForce * Mathf.Sin(forceAngle * Mathf.Deg2Rad)
                , 0
                , aaForce * Mathf.Cos(forceAngle * Mathf.Deg2Rad));
        aaLeftR = new Vector3(hull.localScale.x / 2 - boatMOI.position.com.x, 0, -hull.localScale.z / 2 - boatMOI.position.com.z);
        aaRightR = new Vector3(-hull.localScale.x / 2 - boatMOI.position.com.x, 0, -hull.localScale.z / 2 - boatMOI.position.com.z);
        aaLeftTorque = new Vector3(aaLeftR.y * aaThrust.z - aaLeftR.z * aaThrust.y
            , aaLeftR.z * aaThrust.x - aaLeftR.x * aaThrust.z
            , aaLeftR.x * aaThrust.y - aaLeftR.y * aaThrust.x);
        aaRightTorque = new Vector3(aaRightR.y * aaThrust.z - aaRightR.z * aaThrust.y
            , aaRightR.z * aaThrust.x - aaRightR.x * aaThrust.z
            , aaRightR.x * aaThrust.y - aaRightR.y * aaThrust.x);
        aaLeftAcceleration = new Vector3(aaLeftTorque.x / boatMOI.totalMomentOfInertia.total
            , aaLeftTorque.y / boatMOI.totalMomentOfInertia.total
            , aaLeftTorque.z / boatMOI.totalMomentOfInertia.total);
        aaRightAcceleration = new Vector3(aaRightTorque.x / boatMOI.totalMomentOfInertia.total
            , aaRightTorque.y / boatMOI.totalMomentOfInertia.total
            , aaRightTorque.z / boatMOI.totalMomentOfInertia.total);
        //aaLeftVelocityFinal
        //aaRightVelocityFinal
        //aaLeftDisplacementFinal = new Vector3(0, Mathf.PI, 0);
        //aaRightDisplacementFinal = new Vector3(0, Mathf.PI, 0);
        aaLeftTimeFinal = Mathf.Sqrt(2 * aaLeftDisplacementFinal.y / Mathf.Abs(aaLeftAcceleration.y));
        aaRightTimeFinal = Mathf.Sqrt(2 * aaRightDisplacementFinal.y / Mathf.Abs(aaRightAcceleration.y));


    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //float verticalMovement = Input.GetAxisRaw("Vertical");
        //if (verticalMovement > 0)
        if (Input.GetKeyDown(KeyCode.W))
        {
            isLAForcing = true;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            isAAForcingLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            isAAForcingRight = true;
        }
        if (isLAForcing && !isAAForcingLeft && !isAAForcingRight)
        {
            //if (timer <= laTime)
            if (fluidDynamicDrag)
            {

                if (timer <= fddTotalTime)
                {
                    timer += Time.fixedDeltaTime;
                    ++updates;
                    updatesText.text = "Updates: " + updates;
                    timeText.text = "Time: " + timer;
                    boatTransform.z += (laForce / fddDragCoefficcient) * Time.fixedDeltaTime
                             + ((laForce - fddDragCoefficcient * fddVelocity.z) / fddDragCoefficcient) * (boatMOI.mass.total / fddDragCoefficcient)
                        * (Mathf.Exp((-fddDragCoefficcient * Time.fixedDeltaTime) / boatMOI.mass.total) - 1)

                    ;

                    fddVelocity.z = fddVelocityMax.z - (Mathf.Exp(-fddDragCoefficcient * Time.fixedDeltaTime / boatMOI.mass.total))
                        * (fddVelocityMax.z - fddVelocity.z);
                    fddAcceleration.z = (laForce - fddDragCoefficcient * fddVelocity.z) / boatMOI.mass.total;

                    transform.position = boatTransform;
                    fddDisplacement = boatTransform;
                }
                //Debug.Break();
            }
            else
            {
                if (transform.position.z <= displacementFinal)
                {


                    ++updates;
                    updatesText.text = "Updates: " + updates;
                    timeText.text = "Time: " + timer;
                    //Debug.Log(laAcceleration.z);

                    boatTransform.x += laVelocity.x * Time.fixedDeltaTime + .5f * laAcceleration.x * Time.fixedDeltaTime * Time.fixedDeltaTime;
                    boatTransform.z += laVelocity.z * Time.fixedDeltaTime + .5f * laAcceleration.z * Time.fixedDeltaTime * Time.fixedDeltaTime;
                    laVelocity.x += laAcceleration.x * Time.fixedDeltaTime;
                    laVelocity.z += laAcceleration.z * Time.fixedDeltaTime;
                    transform.position = boatTransform;
                    timer += Time.fixedDeltaTime;

                }
            }
        }
        if (!isLAForcing && isAAForcingLeft && !isAAForcingRight)
        {
            timer += Time.fixedDeltaTime;

            //if (timer <= aaLeftTimeFinal)
            //if (transform.rotation.eulerAngles.y >= aaLeftDisplacementFinal.y * Mathf.Rad2Deg || transform.rotation.eulerAngles.y == 0)
            if (Mathf.Abs(aaDisplacement.y) <= aaLeftDisplacementFinal.y)
            {
                Debug.Log(laAcceleration.z);
                ++updates;
                updatesText.text = "Updates: " + updates;
                timeText.text = "Time: " + timer;
                aaDisplacement += aaVelocity * Time.fixedDeltaTime + .5f * aaLeftAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
                aaVelocity += aaLeftAcceleration * Time.fixedDeltaTime;
                transform.rotation = Quaternion.Euler(aaDisplacement * Mathf.Rad2Deg);
            }
        }
        if (!isLAForcing && !isAAForcingLeft && isAAForcingRight)
        {
            //if (timer <= aaRightTimeFinal)
            //if (transform.rotation.eulerAngles.y <= aaRightDisplacementFinal.y * Mathf.Rad2Deg)
            if (aaDisplacement.y <= aaRightDisplacementFinal.y)
            {
                Debug.Log(laAcceleration.z);
                ++updates;
                updatesText.text = "Updates: " + updates;
                timeText.text = "Time: " + timer;
                aaDisplacement += aaVelocity * Time.fixedDeltaTime + .5f * aaRightAcceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
                aaVelocity += aaRightAcceleration * Time.fixedDeltaTime;
                transform.rotation = Quaternion.Euler(aaDisplacement * Mathf.Rad2Deg);
            }
            timer += Time.fixedDeltaTime;
        }
    }
}
