﻿using System.Collections;
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
    public float publicAccel; // = 0.15f;
    public float publicBrake; // = 0.5f;

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

        //Finding out the angle to the next target, waypoint to waypoint, version 1
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

        //Finding out the angle to the next target, drivingControl.rb.gameObject.transform to waypoint, version 2
        //Works well, POV is that of drivingControl.rb.gameObject.transform.position
        Debug.Log("next waypoint is " + wayPoints.waypoints[currentPoint].name);
        Debug.Log("targetAngle is " + targetAngle);
        switch (Mathf.Abs(targetAngle))
        {
            case float f when Mathf.Abs(targetAngle) > 50:
                Debug.Log("Sharp Curv, more than 50: " + Mathf.Abs(targetAngle));
                break;
            case float f when Mathf.Abs(targetAngle) > 25:
                Debug.Log("Curv, more than 25: " + Mathf.Abs(targetAngle));
                break;
            default:
                Debug.Log("Straight, 25 or less: " + Mathf.Abs(targetAngle));
                break;
        }

        //Find out the next waypoint's y position to be at uphill or downhill
        if (currentPoint != 0) //avoiding subtracting -1 from currentPoint[0]
        {
            float nextWaypoint = wayPoints.waypoints[currentPoint].transform.position.y;
            float currentWaypoint = wayPoints.waypoints[currentPoint-1].transform.position.y;
            float yDifference = nextWaypoint - currentWaypoint;
            switch (yDifference)
            {
                case float f when yDifference > 1.5:
                    Debug.Log("the target on uphill: " + yDifference);
                    break;
                case float f when yDifference < -1.5:
                    Debug.Log("the target on downhill: " + yDifference);
                    break;
                default:
                    Debug.Log("the target on flat ground: " + yDifference);
                    break;
            }
        }

        //Get the speed in MPH
        double mphSpeed = (drivingControl.currentSpeed * 3600) * 0.000621371;
        Debug.Log("drivingControl.currentSpeed: " + (int)mphSpeed + "mph");

        //float accel = 0.5f; //original value
        float accel = 0.5f;
        float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
        //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
        //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
        float brake = 0;

        if (distanceToTarget < 5)
        {
            //Debug.Log("distance to target is < 5 ");
            if (currentPoint == 0 && roundTrip == false) //first time approaching the point [0]
            {
                Debug.Log("First time approaching the point [" + currentPoint + "]");
                Debug.Log("publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
                //accel = 0.5f; //original value
                accel = 0.4f; //trying new value
                brake = 0;
            }
            else //making a round trip
            {
                accel = publicAccel;
                brake = publicBrake;
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
                accel = publicAccel;
                brake = publicBrake;
                Debug.Log("accel = publicAccel & brake = publicBrake: " + publicAccel + " " + publicBrake);
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

        //Debug.Log("drivingControl.Go(" + accel + ", " + steeringSensitivity + ", " + brake + ")");
        drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
        drivingControl.CheckSkidding();
        drivingControl.calculateEngineSound();
    }
}
