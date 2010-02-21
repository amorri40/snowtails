using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class TimeLabelUpdate : MonoBehaviour {

	private static Stopwatch timer = new Stopwatch();

    public static Stopwatch getTimer()
        {
        return timer;
        }

	// Use this for initialization
	void Start () {
    timer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text="Time:"+timer.Elapsed.Hours+":"+timer.Elapsed.Minutes+":"+timer.Elapsed.Seconds;
	}
}