using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drives : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float torque = 200;
    public GameObject[] wheelMeshes;
    public float maxSteerAngle = 30;

    // Start is called before the first frame update
    void Start()
    {
        //wheelColliders will be setup in Inspector, so no need to do GetComponent<WheelCollider>()
        //wheelColliders = GetComponent<WheelCollider>();
    }

    void Go(float accel, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        float thrustTorque = accel * torque;
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;

        for (int i = 0; i < 4; i++)
        {
            if (i < 2)
            {
                wheelColliders[i].steerAngle = steer;
            }
            else if (i >= 2)
            {
                wheelColliders[i].motorTorque = thrustTorque;
                //Debug.Log(WC[i] + " WheelCollider.motorTorque");
            }
            else
            {
                Debug.Log(wheelColliders[i]);
            }

            Vector3 Vec3pos;
            Quaternion quat;
            wheelColliders[i].GetWorldPose(out Vec3pos, out quat);
            wheelMeshes[i].transform.position = Vec3pos;
            wheelMeshes[i].transform.localRotation = quat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float VrtclAxis = Input.GetAxis("Vertical");
        float HrzntlAxis = Input.GetAxis("Horizontal");
        Go(VrtclAxis, HrzntlAxis);
    }
}
