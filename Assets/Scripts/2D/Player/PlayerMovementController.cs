using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovementController : MonoBehaviour {

    private Rigidbody2D playerRigidbody;
    private float walkSpeed;
    private float sprintSpeed;
    private float curSpeed;
    private float maxSpeed;

    // Use this for initialization
    void Start () {
        walkSpeed = 5f;
        sprintSpeed = walkSpeed + (walkSpeed / 2);
        playerRigidbody = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        JoystickMovement();
	}

    void JoystickMovement()
    {
        curSpeed = walkSpeed;
        maxSpeed = curSpeed;

        // Mov
        playerRigidbody.velocity = new Vector2(Mathf.Lerp(0, CrossPlatformInputManager.GetAxis("Horizontal") * curSpeed, 0.8f ), Mathf.Lerp(0, CrossPlatformInputManager.GetAxis("Vertical") * curSpeed, 0.8f)); // * Time.deltaTime;
    }


    public class PlayerMovements : MonoBehaviour
    {
        // Normal Movements Variables
        



        
    }


}
