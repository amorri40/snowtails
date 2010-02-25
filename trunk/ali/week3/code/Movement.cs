using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Movement : MonoBehaviour
    {
	public WheelCollider FrontLeftWheel ;
public WheelCollider FrontRightWheel;
		float Speed=300;
        public static int playerid = 0; //this will set the plaer id for multiplayer

void Start () {
	// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
	//rigidbody.centerOfMass.y = -1.5;
	rigidbody.centerOfMass = new Vector3 (rigidbody.centerOfMass.x, -1.5f, rigidbody.centerOfMass.z);
}

void Update () {

    if (playerid == 1)
        {
        if ((Input.GetKey("right")) && (Input.GetKey("left")))
            {
            FrontLeftWheel.motorTorque = Speed;
            FrontRightWheel.motorTorque = Speed;
            }
        else
            {

            if ((Input.GetKey("right")) || Input.GetKey(KeyCode.JoystickButton3))
                {

                FrontLeftWheel.motorTorque = 600;
                }
            else
                {
                FrontLeftWheel.motorTorque = 0;
                }
            if ((Input.GetKey("left")) || Input.GetKey(KeyCode.JoystickButton2))
                {

                FrontRightWheel.motorTorque = 600;
                }
            else
                {
                FrontRightWheel.motorTorque = 0;
                }
            }
        if (Input.GetKey("down") || Input.GetKey(KeyCode.JoystickButton1))
            {
            FrontRightWheel.motorTorque = -290;
            FrontLeftWheel.motorTorque = -290;
            }
        else
            {
            FrontRightWheel.brakeTorque = 0;
            FrontLeftWheel.brakeTorque = 0;
            }
        }
    else if (playerid == 2)
        {
        if ((Input.GetKey("d")) && (Input.GetKey("a")))
            {
            FrontLeftWheel.motorTorque = Speed;
            FrontRightWheel.motorTorque = Speed;
            }
        else
            {

            if ((Input.GetKey("d")))
                {

                FrontLeftWheel.motorTorque = 600;
                }
            else
                {
                FrontLeftWheel.motorTorque = 0;
                }
            if ((Input.GetKey("a")))
                {

                FrontRightWheel.motorTorque = 600;
                }
            else
                {
                FrontRightWheel.motorTorque = 0;
                }
            }
        if (Input.GetKey("s"))
            {
            FrontRightWheel.motorTorque = -290;
            FrontLeftWheel.motorTorque = -290;
            }
        else
            {
            FrontRightWheel.brakeTorque = 0;
            FrontLeftWheel.brakeTorque = 0;
            }
        }
}
}//end of class