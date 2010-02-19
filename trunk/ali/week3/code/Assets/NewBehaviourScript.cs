using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public int timet=1;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		guiText.text="Time:"+timet;
	}
}
