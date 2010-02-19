using UnityEngine;
using System.Collections;

public class LapCollider : MonoBehaviour
    {



    // Use this for initialization
    void Start()
        {

        }

    // Update is called once per frame
    void Update()
        {

        }
    void OnTriggerExit(Collider other)
        {
        if (other.name == "PlayerCollider" && CheckPointCollider.wentThroughCheckpoint)
            {
            if (LapManager.lapCount > 2) { 
                //finish
                }
            else
                {
                LapManager.lapCount++;
                CheckPointCollider.wentThroughCheckpoint = false;
                }
            }
        
        }
    }