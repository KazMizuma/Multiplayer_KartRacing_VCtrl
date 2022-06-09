using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the parent of the car, not to the rigid body
public class drivingControl : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float torque = 200;
    public GameObject[] wheelMeshes;
    public float maxSteerAngle = 30;
    public float maxBrakeTorque = 1250; // 6/09/22 Trfc Ctrl, increasing the brake torque by 250

    public AudioSource skidSound;
    public Transform tireSkidPrefab; //for startTireSkid and endTireSkid, tire skidding marking prefab
    Transform[] tireSkid = new Transform[4]; //for startTireSkid and endTireSkid, four tires, 
    public ParticleSystem smokePrefab; //tire skidding smoke prefab
    ParticleSystem[] skidSmoke = new ParticleSystem[4]; //for four tires to instantiate the prefab

    public GameObject brakeLight;
    public AudioSource AccelHigh;
    public Rigidbody rb;
    public float gearLength;
    public float currentSpeed { get { return rb.velocity.magnitude * gearLength; } }
    public float lowPitch = 1f;
    public float highPitch = 6f;
    public int numGear = 5;
    public float maxSpeed = 200;
    int currentGear = 1;
    float RPM;
    float currGearPerc;

    public GameObject driverNamePrefab;
    public Renderer carMesh;

    public void startTireSkid(int i)
    {
        if (tireSkid[i] == null)
        {
            tireSkid[i] = Instantiate(tireSkidPrefab);
            tireSkid[i].parent = wheelColliders[i].transform;
            //Debug.Log("wheelColliders[i].transform " + wheelColliders[i].transform);
            //Debug.Log("wheelColliders[i].gameObject " + wheelColliders[i].gameObject);
            tireSkid[i].localRotation = Quaternion.Euler(90, 0, 0);
            tireSkid[i].localPosition = -Vector3.up * wheelColliders[i].radius;
        }
    }

    public void endTireSkid(int i)
    {
        if (tireSkid[i] == null) return;
        Transform holder = tireSkid[i];
        //Debug.Log("tireSkid[i] or holder " + tireSkid[i]);
        tireSkid[i] = null;
        holder.parent = null;
        //Debug.Log("holder.gameObject " + holder.gameObject);
        holder.localRotation = Quaternion.Euler(90, 0, 0);
        Destroy(holder.gameObject, 15);
    }

    // Awake is called when the script reference is being loaded
    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //wheelColliders will be setup in Inspector, so no need to do GetComponent<WheelCollider>()
        //wheelColliders = GetComponent<WheelCollider>();
        for (int i = 0; i < 4; i++)
        {
            skidSmoke[i] = Instantiate(smokePrefab);
            skidSmoke[i].Stop();
        }

        brakeLight.SetActive(false);

        GameObject driverName = Instantiate<GameObject>(driverNamePrefab);
        driverName.GetComponent<driverNameUI>().targetCar = rb.gameObject.transform;
        driverName.GetComponent<driverNameUI>().driverName.text = rb.transform.parent.name;
        driverName.GetComponent<driverNameUI>().carRenderer = carMesh;
    }

    public void calculateEngineSound()
    {
        float gearPercentage = 1 / (float)numGear;
        //Debug.Log("gearPercentage " + gearPercentage + " = 1 / (float)numGear)");
        float targetGearFactor = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1),
            Mathf.Abs(currentSpeed / maxSpeed));
        //Debug.Log("targetGearFactor " + targetGearFactor + " = Mathf.InverseLerp(gearPercentage * currentGear, gearPercentage * (currentGear + 1), Mathf.Abs(currentSpeed / maxSpeed)) ");
        currGearPerc = Mathf.Lerp(currGearPerc, targetGearFactor, Time.deltaTime * 5f);
        //Debug.Log("currGearPerc " + currGearPerc + " = Mathf.Lerp(" + "currGearPerc " + currGearPerc + ", " + "targetGearFactor " + targetGearFactor + ", " + "Time.deltaTime * 5f " + Time.deltaTime * 5f + ")");

        var gearNumFactor = 0f;
        if (currentGear > 0) //if currentGear <= 0, set it back to 1 to avoid infinity calculation!
        {
            gearNumFactor = currentGear / (float)numGear;
        }
        if (currentGear <= 0)
        {
            gearNumFactor = (currentGear + 1) / (float)numGear;
        }
        //Debug.Log("gearNumFactor " + gearNumFactor + " = currentGear " + currentGear + " / " + "(float)numGear " + (float)numGear);

        RPM = Mathf.Lerp(gearNumFactor, 1, currGearPerc);
        //Debug.Log("RPM " + RPM + " = Mathf.Lerp(gearNumFactor, 1, currGearPerc)");

        float speedPercentage = Mathf.Abs(currentSpeed / maxSpeed); //I don't know if necessary to do Mathf.Abs here, but just trying - no change
        //Debug.Log("speedPercentage " + speedPercentage + " = " + "currentSpeed " + currentSpeed + " / " + "maxSpeed " + maxSpeed);

        float upperGearMax = 1 / (float)numGear * (currentGear + 1); //changing the calculation order
        //Debug.Log("upperGearMax " + upperGearMax + " = 1 / " + (float)numGear + " * " + currentGear + " + 1");

        if (currentGear <= 0) //again, if currentGear <= 0, set it back to 1 to avoid infinity calculation!
        {
            currentGear = 1;
        }
        float downGearMax = 1 / (float)numGear * currentGear; //changing the calculation order
        //Debug.Log("downGearMax " + downGearMax + " = 1 / " + (float)numGear + " * " + currentGear);

        if (currentGear > 0 && speedPercentage < downGearMax) 
        {
            //Debug.Log("if currentGear " + currentGear + " > 0 && " + "speedPercentage " + speedPercentage + " < " + "downGearMax " + downGearMax);
            currentGear--;
            //Debug.Log("currentGear-- = " + currentGear);
        }
        if (currentGear < numGear && speedPercentage > upperGearMax) // why (numGear - 1)? trying numGear, works the same way, no change
        {
            //Debug.Log("if currentGear " + currentGear + " < " + "numGear " + numGear + " - 1 && " + "speedPercentage " + speedPercentage + " > " + "upperGearMax " +upperGearMax);
            currentGear++;
            //Debug.Log("currentGear++ = " + currentGear);
        }

        float pitch = Mathf.Lerp(lowPitch, highPitch, RPM); //just trying to see what happnes if RPM is replaced with currentSpeed
        //Debug.Log("pitch " + pitch + " = Mathf.Lerp(lowPitch, highPitch, RPM)");
        AccelHigh.pitch = Mathf.Min(highPitch, pitch) * 0.25f;
        //Debug.Log("AccelHigh.pitch " + AccelHigh.pitch + " = Mathf.Min(highPitch, pitch) * 0.25f");

        //Debug.Log("speedPercentage " + speedPercentage + " = " + "currentSpeed " + currentSpeed + " / " + "maxSpeed " + maxSpeed + "  RPM " + RPM);
    }

    public void Go(float accel, float steer, float brake)
    {
        accel = Mathf.Clamp(accel, -4, 3);
        float thrustTorque = 0;
        if (currentSpeed < maxSpeed)
        {
            //Debug.Log("currentSpeed " + currentSpeed);
            thrustTorque = accel * torque;
        }

        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        brake = Mathf.Clamp(brake, 0, 2) * maxBrakeTorque; // 6/09/22 Trfc Ctrl, increasing the brake threshold by 1

        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("wheelColliders[" + i + "] " + wheelColliders[i]); //the orders of wheel collider listing
            if (i < 2)
            {
                wheelColliders[i].steerAngle = steer;
                //Debug.Log("wheelColliders[" + i + "] " + wheelColliders[i] + " for steerAngle");
            }
            else if (i >= 2)
            {
                wheelColliders[i].motorTorque = thrustTorque;
                //Debug.Log("wheelColliders[" + i + "] " + wheelColliders[i] + " for motorTorque");
                //Debug.Log("wheelColliders[i].motorTorque: " + thrustTorque);
                wheelColliders[i].brakeTorque = brake;
            }
            else
            {
                Debug.Log(this.transform.gameObject.name + " wheelColliders[" + i + "] " + wheelColliders[i] + " else clause");
            }
            Vector3 Vec3pos;
            Quaternion quat;
            wheelColliders[i].GetWorldPose(out Vec3pos, out quat);
            wheelMeshes[i].transform.position = Vec3pos;
            wheelMeshes[i].transform.localRotation = quat;
            //Debug.Log(wheelColliders[i] + " wheelColliders[i]");
        }

        Debug.Log(this.transform.gameObject.name + " wheelColliders[i].motorTorque: " + thrustTorque);
        Debug.Log(this.transform.gameObject.name + " wheelColliders[i].brakeTorque: " + brake);

        if (brake != 0)
        {
            brakeLight.SetActive(true);
        }
        else
        {
            brakeLight.SetActive(false);
        }
    }

    public void CheckSkidding()
    {
        int numSkid = 0;
        for (int i=0; i<4; i++)
        {
            //Debug.Log("wheelColliders[" + i + "] " + wheelColliders[i]); //the orders of wheel collider listing
            WheelHit wheelHit;
            wheelColliders[i].GetGroundHit(out wheelHit);

            if (Mathf.Abs(wheelHit.forwardSlip) >= 0.2f || Mathf.Abs(wheelHit.sidewaysSlip) >= 0.2f)
            {
                //Debug.Log("Mathf.Abs(wheelHit.forwardSlip) " + Mathf.Abs(wheelHit.forwardSlip));
                //Debug.Log("Mathf.Abs(wheelHit.sidewaysSlip) " + Mathf.Abs(wheelHit.sidewaysSlip));

                if (!skidSound.isPlaying)
                {
                    numSkid++;
                    //skidSound.Play(); //Sounds Sooo Anoying!!! So disable it!!!
                }

                //startTireSkid(i); //not using this method, using betterSkid

                //skidSound.Play(); //the sound gets distorted

                skidSmoke[i].transform.position = wheelColliders[i].transform.position - 
                    wheelColliders[i].transform.up * wheelColliders[i].radius;
                skidSmoke[i].Emit(1);
            }

            if (Mathf.Abs(wheelHit.forwardSlip) <= 0.0f || Mathf.Abs(wheelHit.sidewaysSlip) <= 0.0f) //trying out my method
            {
                if (skidSound.isPlaying)
                {
                    skidSound.Stop();
                }
            }

            //else
            //{
            //    endTireSkid(i); //not using this method, using betterSkid
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        //float VrtclAxis = Input.GetAxis("Vertical"); Below all moved over to playerController.cs
        //float HrzntlAxis = Input.GetAxis("Horizontal");
        //float JumpAxis = Input.GetAxis("Jump");
        //Go(VrtclAxis, HrzntlAxis, JumpAxis);
        //CheckSkidding();
        //calculateEngineSound(); Above all moved over to playerController.cs
    }
}
