using UnityEngine;
using System.Collections;

public class SledSlip : MonoBehaviour
    {

    public WheelCollider CorrespondingCollider;
    public GameObject SlipPrefab;

    // Use this for initialization
    void Start()
        {

        }

    // Update is called once per frame
    void Update()
        {
        /* Taken from driving example start */
        // define a wheelhit object, this stores all of the data from the wheel collider and will allow us to determine
        // the slip of the tire.
        WheelHit CorrespondingGroundHit;
        CorrespondingCollider.GetGroundHit(out CorrespondingGroundHit);

        // if the slip of the tire is greater than 2.0, and the slip prefab exists, create an instance of it on the ground at
        // a zero rotation.
        if (Mathf.Abs(CorrespondingGroundHit.sidewaysSlip) > 2.0)
            {
            if (SlipPrefab && Time.timeScale > 0)
                { //added && part
                Instantiate(SlipPrefab, CorrespondingGroundHit.point, Quaternion.identity);
                }
            }
        /* end taken from driving example */

        }
    }
