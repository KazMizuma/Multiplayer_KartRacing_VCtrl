using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public wayPoints wayPoints;
    drivingControl drivingControl;
    public float steeringSensitivity = 0.01f;
    Vector3 target;
    int currentPoint = 0;
    bool roundTrip = false;
    //float accel = 0.25f; //these values need to be updated to get the car going and keep it under control
    //float brake = 0f;
    public float publicAccel; // = 0.25f;
    public float publicBrake; // = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        drivingControl = this.GetComponent<drivingControl>();
        target = wayPoints.waypoints[currentPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
        float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;

        float distanceToTarget = Vector3.Distance(target, drivingControl.rb.gameObject.transform.position);

        //Finding out the angle to the next target, waypoint to waypoint, version 1; MY AI CODE!
        //Does Not Quite Work because POV is World based
        //Debug.Log("next waypoint is " + wayPoints.waypoints[currentPoint].name);
        //if (currentPoint != 0) //avoiding subtracting -1 from currentPoint[0]
        //{
        //    Vector3 nextTarget = wayPoints.waypoints[currentPoint - 1].transform.InverseTransformPoint(target); //target angle from waypoint to waypoint
        //    Debug.Log("the next waypoint is at " + nextTarget + "  " + nextTarget.x + " " + nextTarget.y + " " + nextTarget.z);
        //    float nextTargetAngle = Mathf.Atan2(nextTarget.x, nextTarget.z) * Mathf.Rad2Deg;
        //    Debug.Log("nextTargetAngle is " + nextTargetAngle); //the numbers don't indicate differences in angle simply!
        //    // East to West -90, West to East 90, South to North 0, North to South 180, North West -45, North East 45, South West -140, South East 140, etc.
        //    nextTargetAngle = Mathf.Abs(nextTargetAngle);
        //    switch (nextTargetAngle) //calculating an angle difference from current to next waypoint
        //    {
        //        case float f when nextTargetAngle > 50:
        //            Debug.Log("Sharp Curv, more than 50: " + nextTargetAngle);
        //            break;
        //        case float f when nextTargetAngle > 25:
        //            Debug.Log("Curv, more than 25: " + nextTargetAngle);
        //            break;
        //        default:
        //            Debug.Log("Straight, 25 or less: " + nextTargetAngle);
        //            break;
        //    }
        //}

        //Bringing the following to here at the top!
        //float accel = 0.5f; //original value
        float accel = 0.5f;
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
        //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
        //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
        float brake = 0;

        //Get the distance to the next waypoint
        //Debug.Log("distanceToTarget: " + (int)distanceToTarget + " Meters");
        //double distanceInMile = distanceToTarget * 0.000621371;
        //decimal distanceInMileInDecimal = (decimal)distanceInMile;
        //Debug.Log("distanceInMileInDecimal Rounded 4: " + decimal.Round(distanceInMileInDecimal, 4) + " Miles");

        Debug.Log("next waypoint is " + wayPoints.waypoints[currentPoint].name);
        //Debug.Log("targetAngle is " + targetAngle);

        //Get the speed in MPH
        double mphSpeed = (drivingControl.currentSpeed * 3600) * 0.000621371;
        int mphSpeedInt = (int)mphSpeed;
        //Debug.Log("drivingControl.currentSpeed: " + mphSpeedInt + " MPH");

        //Finding out the angle to the next target, drivingControl.rb.gameObject.transform to waypoint, version 2; MY AI CODE!
        //Works well, POV is that of drivingControl.rb.gameObject.transform.position
        switch (Mathf.Abs(targetAngle))
        {
            case float f when Mathf.Abs(targetAngle) > 50:
                Debug.Log("Sharp Curv, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                accel = 0.1f;
                brake = 0.06f;
                break;
            case float f when Mathf.Abs(targetAngle) > 25:
                Debug.Log("Curv, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                accel = 0.3f;
                brake = 0.02f;
                break;
            default:
                Debug.Log("Straight, 25 or less Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                Debug.Log("drivingControl.currentSpeed: " + mphSpeedInt + " MPH"); //showing the current speed in MPH
                switch (mphSpeedInt)
                {
                    case int i when mphSpeedInt < 60:
                        Debug.Log("bring it up to 60mph!");
                        accel = 0.8f;
                        brake = 0;
                        break;
                    case int i when mphSpeedInt > 80:
                        Debug.Log("bring the shit down below 80!");
                        accel = publicAccel;
                        brake = publicBrake;
                        break;
                    default:
                        // the current speed is between 60 and 80mph
                        break;
                }
                break;
        }

        //Find out the next waypoint's y position to be at uphill or downhill; MY AI CODE!
        if (currentPoint != 0) //avoiding subtracting -1 from currentPoint[0]
        {
            float nextWaypoint = wayPoints.waypoints[currentPoint].transform.position.y;
            float currentWaypoint = wayPoints.waypoints[currentPoint-1].transform.position.y;
            float yDifference = nextWaypoint - currentWaypoint;
            switch (yDifference)
            {
                case float f when yDifference > 1.5:
                    Debug.Log("the target on uphill: " + yDifference);
                    accel = 0.7f;
                    brake = 0;
                    break;
                case float f when yDifference < -1.5:
                    Debug.Log("the target on downhill: " + yDifference);
                    accel = 0.01f;
                    brake = 0.1F;
                    break;
                default:
                    Debug.Log("the target on flat ground: " + yDifference);
                    break;
            }
        }

        //Commenting out to bring the following to the top!
        ////float accel = 0.5f; //original value
        //float accel = 0.5f;
        //float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
        ////Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
        ////Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
        //float brake = 0;

        if (distanceToTarget < 5)
        {
            //Debug.Log("distance to target is < 5 ");
            if (currentPoint == 0 && roundTrip == false) //first time approaching the point [0]
            {
                Debug.Log("First time approaching the point [" + currentPoint + "]");
                Debug.Log("publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
                //accel = 0.5f; //original value

                //COMMENTING OUT TO TRY OUT MY AI CODE!
                //accel = 0.4f; //trying new value
                //brake = 0;
            }
            else //making a round trip
            {
                //COMMENTING OUT TO TRY OUT MY AI CODE!
                //accel = publicAccel;
                //brake = publicBrake;
                //Debug.Log("publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
            }
        }

        if (distanceToTarget < 2) //increase the value if gameObject circles around waypoint
        {
            //Debug.Log("distance to target is < 2 ");

            currentPoint++;

            if (currentPoint >= wayPoints.waypoints.Length) 
            {
                roundTrip = true;
                currentPoint = 0;
                Debug.Log("target is back to the origin [" + currentPoint + "]");
                //steeringSensitivity = 0.008f; maybe no need to lower the value

                //COMMENTING OUT TO TRY OUT MY AI CODE!
                //accel = publicAccel;
                //brake = publicBrake;
                //Debug.Log("accel = publicAccel & brake = publicBrake: " + publicAccel + " " + publicBrake);
            }

            target = wayPoints.waypoints[currentPoint].transform.position;

            //switch (wayPoints.waypoints[currentPoint].tag) //waypoints tag detection test, works without a hitch
            //{
            //    case "Finish":
            //        Debug.Log("case is Finish - " + wayPoints.waypoints[currentPoint].tag);
            //        break;
            //    case "Untagged":
            //        Debug.Log("case is Untagged - " + wayPoints.waypoints[currentPoint].tag);
            //        break;
            //    default:
            //        Debug.Log("case is default - " + wayPoints.waypoints[currentPoint].tag);
            //        break;
            //}

            //if (wayPoints.waypoints[currentPoint].tag == "Finish") //waypoints tag detection test, works without a hitch
            //{
            //    Debug.Log("wayPoints.waypoints[currentPoint].tag is " + wayPoints.waypoints[currentPoint].tag);
            //    Debug.Log("Say Finish!!");
            //}
            //else if (wayPoints.waypoints[currentPoint].tag == "Untagged")
            //{
            //    Debug.Log("wayPoints.waypoints[currentPoint].tag is " + wayPoints.waypoints[currentPoint].tag);
            //    Debug.Log("Say Untagged!!");
            //}
            //else
            //{
            //    Debug.Log("wayPoints.waypoints[currentPoint].tag is " + wayPoints.waypoints[currentPoint].tag);
            //    Debug.Log("Just say the tag " + wayPoints.waypoints[currentPoint].tag);
            //}
        }

        Debug.Log("drivingControl.Go(" + accel + ", " + steer + ", " + brake + ")");
        drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
        drivingControl.CheckSkidding();
        drivingControl.calculateEngineSound();
    }
}
