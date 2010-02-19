using UnityEngine;
using System.Collections;

public class LapManager : MonoBehaviour
    {

    public static int lapCount = 1;

    // Use this for initialization
    void Start()
        {
        lapCount = 1;
        }

    // Update is called once per frame
    void Update()
        {
        guiText.text = "Lap: " + lapCount + "/3";
        }
    void OnTriggerExit(Collider other)
        {
        lapCount++;
        }
    }