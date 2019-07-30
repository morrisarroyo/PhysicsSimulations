using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterController : MonoBehaviour {

	[System.Serializable]
    public class BTSBoundary
    {
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;
        public float zMin;
        public float zMax;
    }

    public float Speed = 10.0f;
    public float xTilt = 1.0f;
    public float zTilt = 0.5f;
           

    public BTSBoundary Boundary;

    private Rigidbody MainCharacterRigidbody;

    // Use this for initialization
	void Start () {
        MainCharacterRigidbody = GetComponent<Rigidbody>();

    }
	
	// FixUpdate is called once per frame, use for physics stuff
	void FixedUpdate () {
        float l_horizontal = Input.GetAxis("Horizontal");
        float l_vertical = Input.GetAxis("Vertical");
                
        Vector3 l_movement = new Vector3(l_horizontal, 0.0f, l_vertical);

        MainCharacterRigidbody.velocity = l_movement * Speed;
        MainCharacterRigidbody.position = new Vector3(
                Mathf.Clamp(MainCharacterRigidbody.position.x, Boundary.xMin, Boundary.xMax),
                0.0f,
                Mathf.Clamp(MainCharacterRigidbody.position.z, Boundary.zMin, Boundary.zMax)
            );

        MainCharacterRigidbody.rotation = Quaternion.Euler(MainCharacterRigidbody.velocity.z * -zTilt, 0.0f, MainCharacterRigidbody.velocity.x * -xTilt);
        

    }
}
