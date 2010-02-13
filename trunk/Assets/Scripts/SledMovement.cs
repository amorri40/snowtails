using UnityEngine;
using System.Collections;

public class SledMovement : MonoBehaviour {

	Vector3	moveDirection;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
	if (Input.GetKey ("left") ||Input.GetKey (KeyCode.JoystickButton3)) {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x-1,rigidbody.velocity.y,rigidbody.velocity.z-1);
		//rigidbody.velocity = new Vector3(rigidbody.position.x-10,0,rigidbody.position.z-10);
		//rigidbody.position =  new Vector3(rigidbody.position.x-1,rigidbody.position.y+0,rigidbody.position.z-1);
		//rigidbody.AddTorque(Vector3.right *100,ForceMode.Acceleration);
		//rigidbody.drag=10;
		//FrontLeftWheel.motorTorque =  350;//EngineTorque / GearRatio[CurrentGear] * 2;//Input.GetAxis("Vertical");
	}
	else
	{
		//rigidbody.velocity.z=0;
	//FrontLeftWheel.motorTorque = 0;
	}
    if (Input.GetKey ("right") ||Input.GetKey (KeyCode.JoystickButton2)) {
		int horizontalInput =1; //right?
		int verticalInput = -1; //forward?
		
		Vector3 forward = transform.forward;
	Vector3 right = new Vector3(forward.z, 0, forward.x);
		Vector3 targetDirection = horizontalInput * right + verticalInput * forward;	
 
moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, 20 * Mathf.Deg2Rad * Time.deltaTime, 1000);
 
		if (targetDirection != Vector3.zero)
	{
	transform.rotation = Quaternion.LookRotation(moveDirection);
	}
		
	rigidbody.velocity = moveDirection  * Time.deltaTime * 200;
	//controller.Move(movement);
		//rigidbody.velocity=movement;
		
		// rigidbody.velocity = new Vector3(rigidbody.velocity.x-1,rigidbody.velocity.y,rigidbody.velocity.z+1);
		//		rigidbody.position =  new Vector3(rigidbody.position.x-1,rigidbody.position.y+0,rigidbody.position.z+1);
		//FrontRightWheel.motorTorque = 350;//EngineTorque / GearRatio[CurrentGear] * 2;//Input.GetAxis("Vertical");
    }
	else
	{
	//FrontRightWheel.motorTorque = 0;
	}
	if (Input.GetKey ("down")) {
	//FrontRightWheel.motorTorque = 0;
	//FrontLeftWheel.motorTorque = 0;
	}
	}
}
