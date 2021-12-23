using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float VrtclAxis = Input.GetAxis("Vertical");
        float HrzntlAxis = Input.GetAxis("Horizontal");
        float JumpAxis = Input.GetAxis("Jump");
        drivingControl.Go(VrtclAxis, HrzntlAxis, JumpAxis);
        drivingControl.CheckSkidding();
        drivingControl.calculateEngineSound();
    }
}
