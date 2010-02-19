using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class TimeLabelUpdate : MonoBehaviour {

	public static int timet=1;
    public static Stopwatch timer = new Stopwatch();
	
	// Use this for initialization
	void Start () {
    timer.Start();
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text="Time:"+timer.Elapsed.Hours+":"+timer.Elapsed.Minutes+":"+timer.Elapsed.Seconds;
	}
}