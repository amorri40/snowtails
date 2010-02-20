using UnityEngine;
using System.Collections;

public class LapCollider : MonoBehaviour
    {

    public GameObject GameEndGUI;
    public GameObject LapText;

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
            if (LapManager.lapCount > 2)
                {
                //finish
                GameEndGUI.active = true;
                Time.timeScale = 0;
                TimeLabelUpdate.timer.Stop();
                }
            else
                {
                LapManager.lapCount++;
                CheckPointCollider.wentThroughCheckpoint = false;
                
                if (LapManager.lapCount > 2)
                    LapText.guiText.text = "Final Lap";
                else
                    LapText.guiText.text = "Lap " + LapManager.lapCount;

                LapText.active=true;
                LapTextManager.lapTextTimer.Start();
                }
            }

        }
    }