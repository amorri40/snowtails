using UnityEngine;
using System.Collections;

public class RaceEndGUI : MonoBehaviour
    {


    // Use this for initialization
    void Start()
        {

        }

    // Update is called once per frame
    void Update()
        {

        }

    void OnGUI()
        {
        // Make a group on the center of the screen
        GUI.BeginGroup(new Rect(Screen.width / 2 - 200, Screen.height / 2 - 200, 400, 400));
        // All rectangles are now adjusted to the group. (0,0) is the topleft corner of the group.

        // We'll make a box so you can see where the group is on-screen.
        GUI.Box(new Rect(0, 0, 400, 400), "Race complete");
        GUI.Button(new Rect(5, 370, 80, 20),"Back to menu");

        // End the group we started above. This is very important to remember!
        GUI.EndGroup();

        }
    }
