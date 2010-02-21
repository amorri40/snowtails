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
        if (other.name == "PlayerCollider" && CheckPointCollider.getCheckpoint())
            {
            if (LapManager.getLapCount() > 2)
                {
                //finish
                GameEndGUI.active = true;
                Time.timeScale = 0;
                TimeLabelUpdate.getTimer().Stop();
                }
            else
                {
                LapManager.addLap();
                CheckPointCollider.setCheckpoint(false);

                if (LapManager.getLapCount() > 2)
                    LapText.guiText.text = "Final Lap";
                else
                    LapText.guiText.text = "Lap " + LapManager.getLapCount();

                LapText.active=true;
                LapTextManager.lapTextTimer.Start();
                }
            }

        }
    }