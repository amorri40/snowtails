using UnityEngine;
using System.Collections;

public class CheckPointCollider : MonoBehaviour
    {

    private static bool wentThroughCheckpoint = false;

    public static void setCheckpoint(bool checkpoint)
        {
        wentThroughCheckpoint = checkpoint;
        }
    public static bool getCheckpoint()
        {
        return wentThroughCheckpoint;
        }

    void OnTriggerExit(Collider other)
        {
        if (other.name == "PlayerCollider")
            setCheckpoint(true);
        }
    }