using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the rigid body of the car, not to the parent
public class antiRollBar : MonoBehaviour
{
    public float antiRoll = 5000.00f;
    public WheelCollider wheelRFront;
    public WheelCollider wheelLFront;
    public WheelCollider wheelRBack;
    public WheelCollider wheelLBack;
    Rigidbody rigidBody;
    public GameObject CtrOfMass; //center of mass entry

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.GetComponent<Rigidbody>();
        //CtrOfMass.transform.localPosition = rigidBody.centerOfMass; //my trying center of mass entry
        rigidBody.centerOfMass = CtrOfMass.transform.localPosition; //center of mass entry
    }

    void GroundWheels (WheelCollider WR, WheelCollider WL)
    {
        WheelHit wheelHit;
        float shiftR = 1.0f;
        float shiftL = 1.0f;

        bool groundedR = WR.GetGroundHit(out wheelHit);
        if (groundedR)
        {
            //Debug.Log("if (groundedR) {shiftR = ...");
            //Debug.Log("WR " + WR);
            shiftR = (-WR.transform.InverseTransformPoint(wheelHit.point).y - WR.radius) / WR.suspensionDistance;
            //Debug.Log("-WR.transform.InverseTransformPoint(wheelHit.point).y " + -WR.transform.InverseTransformPoint(wheelHit.point).y);
            //Debug.Log("WR.radius " + WR.radius);
            //Debug.Log("WR.suspensionDistance " + WR.suspensionDistance);
            //Debug.Log("shiftR " + shiftR);
        }

        bool groundedL = WL.GetGroundHit(out wheelHit);
        if (groundedL)
        {
            //Debug.Log("if (groundedL) {shiftL = ...");
            //Debug.Log("WL " + WL);
            shiftL = (-WL.transform.InverseTransformPoint(wheelHit.point).y - WL.radius) / WL.suspensionDistance;
            //Debug.Log("-WL.transform.InverseTransformPoint(wheelHit.point).y " + -WL.transform.InverseTransformPoint(wheelHit.point).y);
            //Debug.Log("WL.radius " + WL.radius);
            //Debug.Log("WL.suspensionDistance " + WL.suspensionDistance);
            //Debug.Log("shiftL " + shiftL);
        }

        float antiRollForce = (shiftR - shiftL) * antiRoll;

        if (groundedR)
        {
            //Debug.Log("if (groundedR) {rigidBody.AddForceAtPosition...");
            //Debug.Log("groundedR = ture, -antiRollForce " + -antiRollForce);
            rigidBody.AddForceAtPosition(WR.transform.up * -antiRollForce, WR.transform.position);
        }

        if (groundedL)
        {
            //Debug.Log("if (groundedL) {rigidBody.AddForceAtPosition...");
            //Debug.Log("groundedL = ture, antiRollForce " + antiRollForce);
            rigidBody.AddForceAtPosition(WL.transform.up * antiRollForce, WL.transform.position);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        GroundWheels(wheelRFront, wheelLFront);
        GroundWheels(wheelRBack, wheelLBack);
    }
}
