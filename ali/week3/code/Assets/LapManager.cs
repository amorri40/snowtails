using UnityEngine;
using System.Collections;

public class LapManager : MonoBehaviour
    {

    private static int lapCount = 1;

    public static int getLapCount()
        {
        return lapCount;
        }

    public static void addLap()
        {
        lapCount++;
        }

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
    }