using UnityEngine;
using System.Collections;

public class SledController : MonoBehaviour
    {

    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;

    // Use this for initialization
    void Start()
        {
        }

    // Update is called once per frame
    void Update()
        {
        if (Input.GetKeyUp(KeyCode.Space))
            {
            if (ThirdPersonCamera.active)
                {
                ThirdPersonCamera.active = false;
                FirstPersonCamera.active = true;
                }
            else
                {
                FirstPersonCamera.active = false;
                ThirdPersonCamera.active = true;
                }
            }
        }

    }