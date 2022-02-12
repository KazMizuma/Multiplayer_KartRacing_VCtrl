using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// When this script is enabled and AI_Controller is disabled in the CarParent Inspector, a user takes control of the car

public class playerController : MonoBehaviour
{
    drivingControl drivingControl;
    // Start is called before the first frame update
    void Start()
    {
        drivingControl = this.GetComponent<drivingControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!RaceMonitor.racing) return; // Checking ReceMonitor's static bool "racing" to see if its coroutine PlayCountDown() has finished running or not

        float VrtclAxis = Input.GetAxis("Vertical");
        float HrzntlAxis = Input.GetAxis("Horizontal");
        float JumpAxis = Input.GetAxis("Jump");
        drivingControl.Go(VrtclAxis, HrzntlAxis, JumpAxis);

        drivingControl.CheckSkidding();

        drivingControl.calculateEngineSound();
    }
}
