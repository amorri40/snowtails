using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class LapTextManager : MonoBehaviour
    {

    public static Stopwatch lapTextTimer = new Stopwatch();
    public GameObject LapText;

    // Use this for initialization
    void Start()
        {
        
        }

    

    // Update is called once per frame
    void Update()
        {
        //hide text after a certain amount of seconds (5?)
        if (lapTextTimer.Elapsed.Seconds > 2)
            {
            LapText.active = false;
            lapTextTimer.Stop();
            lapTextTimer.Reset();
            }
        }
    }
