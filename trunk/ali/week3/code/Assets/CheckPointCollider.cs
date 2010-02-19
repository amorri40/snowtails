using UnityEngine;
using System.Collections;

public class CheckPointCollider : MonoBehaviour
    {

    public static bool wentThroughCheckpoint = false;

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
        if (other.name == "PlayerCollider")
            wentThroughCheckpoint = true;
        
        }
    }