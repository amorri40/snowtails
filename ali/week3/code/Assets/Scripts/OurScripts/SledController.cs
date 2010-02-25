using UnityEngine;
using System.Collections;

public class SledController : MonoBehaviour
    {

    public WheelCollider FrontLeftWheel;
    public WheelCollider FrontRightWheel;
    public GameObject ThirdPersonCamera;
    public GameObject FirstPersonCamera;
    public GameObject PauseMenu;

    // Use this for initialization
    void Start()
        {
        //make sure the first person camera isn't showing
        FirstPersonCamera.active = false;
        }

    // Update is called once per frame
    void Update()
        {
        /*
         * Change camera 
         */
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

        /*
         * Pause the game
         */
        if (Input.GetKeyUp(KeyCode.P))
            {
            if (Time.timeScale == 1)
                {
                Time.timeScale = 0;
                TimeLabelUpdate.getTimer().Stop();
                PauseMenu.active = true;
                }
            else
                {
                PauseMenu.active = false;
                Time.timeScale = 1;
                TimeLabelUpdate.getTimer().Start();
                }
            }
        }

    }