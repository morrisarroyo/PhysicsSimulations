using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCharacterMovement : MonoBehaviour
{
    public Text timeText;
    public Text updateText;

    public bool drag;
    public float _displacement;
    public float velocity;
    public float acceleration;
    public float time;


    public float _k = 0;
    public float dragTime = 0;

    private float displacement;
    private int update;
    private float elapsedTime = 0;
    private float initialVelocity;
    private float initialDisplacement;
    // Use this for initialization
    void Start()
    {
        update = 0;
        initialVelocity = velocity;
        timeText.text = "Time: " + 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        updateText.text = "Updates: " + update;

        //Debug.Log(Time.fixedDeltaTime);

        if (drag)
        {

            elapsedTime += Time.fixedDeltaTime;
            _k = (-acceleration / initialVelocity / initialVelocity);

            float tempK = (-acceleration / (velocity * velocity));
            dragTime = ((Mathf.Exp(displacement * _k) - 1) / _k / initialVelocity);

            timeText.text = "Time: " + dragTime;
            velocity = velocity / (1 + (tempK * velocity * Time.fixedDeltaTime));
            displacement += Mathf.Log(1 + (tempK * velocity * Time.fixedDeltaTime)) / tempK;

            if (time < elapsedTime && _displacement < displacement)
            {

                Debug.Break();
            }
        }
        else
        {
            elapsedTime += Time.fixedDeltaTime;
            timeText.text = "Time: " + elapsedTime;
            displacement += velocity * Time.fixedDeltaTime + .5f * acceleration * Time.fixedDeltaTime * Time.fixedDeltaTime;
            velocity += acceleration * Time.fixedDeltaTime;

            if (time < elapsedTime && _displacement < displacement)
            {
                Debug.Break();
            }

        }
        transform.position = new Vector3(0, 0, displacement);


        ++update;
    }
}
