using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the parent of the car, not to the rigid body
// When this script is enabled and playerController is disabled on the CarParent in the Inspector, this script controls the car(s)

public class AI_Controller : MonoBehaviour
{
    public wayPoints wayPoints;
    public string nextWaypointName; // For debug purpose only

    public wayPoints wayPointsOdd; //two-way traffic test codes 4/10/22
    public string nextWaypointNameOdd; // For debug purpose only

    public wayPoints wayPointsTrfc; //traffic control test codes 5/01/22
    public string nextWaypointNameTrfc; // For debug purpose only

    drivingControl drivingControl;
    Raycasting raycasting; //Accessing Raycasting script to control collisions
    public float steeringSensitivity = 0.01f;

    Vector3 target;
    int currentPoint = 0;
    Vector3 targetOdd; //two-way traffic test codes 4/10/22
    int currentPointOdd = 0;
    Vector3 targetTrfc; //traffic control test codes 5/01/22
    int currentPointTrfc = 0;

    bool roundTrip = false;

    // Making the values accessible from Raycasting.cs
    //public float accel = 0.5f;
    //public float steer = 0f; // = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
    //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
    //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
    //public float brake = 0f;

    public float publicAccel = 0.25f;
    public float publicBrake = 0.05f;

    public int mphSpeedInt; // For Raycasting to be able to access

    public float unStickDuration = 2f;
    public float unStickTime = 0f; // Setting the time for unstuck code according to the unStickDuration length
    //float orgTargetAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        drivingControl = this.GetComponent<drivingControl>();
        raycasting = this.transform.GetComponentInChildren<Raycasting>(); //Accessing Raycasting script to control collisions

        if (this.transform.gameObject.tag == "Odd")
        {
            targetOdd = wayPointsOdd.waypoints[currentPointOdd].transform.position;
            Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPointsOdd.waypoints[currentPointOdd].name);
            nextWaypointNameOdd = wayPointsOdd.waypoints[currentPointOdd].name; // For debug purpose only
        }
        else if(this.transform.gameObject.tag == "Even")
        {
            target = wayPoints.waypoints[currentPoint].transform.position;
            Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPoints.waypoints[currentPoint].name);
            nextWaypointName = wayPoints.waypoints[currentPoint].name; // For debug purpose only
        }
        else //Traffic Control Test Codes 4/29/22
        {
            targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;
            Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPointsTrfc.waypoints[currentPointTrfc].name);
            nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name; // For debug purpose only
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!RaceMonitor.racing) // Checking ReceMonitor's static bool "racing" to see if its coroutine PlayCountDown() has finished running or not
        {
            return;
        }

        //one-way traffic codes at their original position 4/10/22
        //Vector3 localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
        //float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        //float distanceToTarget = Vector3.Distance(target, drivingControl.rb.gameObject.transform.position);
        //one-way traffic codes at their original position 4/10/22

        //***TWO-WAY TRAFFIC IF/ELSE{} TEST CODES 4/10/22***
        if (this.transform.gameObject.tag == "Odd")
        {
            Vector3 localTargetOdd = drivingControl.rb.gameObject.transform.InverseTransformPoint(targetOdd);
            float targetAngleOdd = Mathf.Atan2(localTargetOdd.x, localTargetOdd.z) * Mathf.Rad2Deg;
            float distanceToTargetOdd = Vector3.Distance(targetOdd, drivingControl.rb.gameObject.transform.position);

            //***ONE-WAY TRAFFIC ORIGINAL CODES (CLEANED & MODIFIED) WITHIN TWO-WAY TRAFFIC IF{} TEST CODES BEGIN 4/10/22***
            //Vector3 localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
            //float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            //float distanceToTarget = Vector3.Distance(target, drivingControl.rb.gameObject.transform.position);

            /* The following values need to be updated to get the car going and keep it under control
               Brought up here from below */
            //float accel = 0.5f; //original value
            float accel = 0.5f;
            float steer = Mathf.Clamp(targetAngleOdd * steeringSensitivity, -1, 1); // "Commenting Out for Overtaking Test" * Mathf.Sign(drivingControl.currentSpeed);
            //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
            //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
            float brake = 0f;

            //Get the speed in MPH
            double mphSpeed = (drivingControl.currentSpeed * 3600) * 0.000621371;
            mphSpeedInt = (int)mphSpeed;
            //Debug.Log("drivingControl.currentSpeed: " + mphSpeedInt + " MPH");

            //Finding out the angle to the next target, drivingControl.rb.gameObject.transform to waypoint, version 2; MY AI CODE!
            //Works well, POV is that of drivingControl.rb.gameObject.transform.position
            if (mphSpeedInt > 49)
            {
                switch (Mathf.Abs(targetAngleOdd))
                {
                    case float f when Mathf.Abs(targetAngleOdd) > 50:
                        Debug.Log(this.transform.gameObject.name + " Sharp Curv, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleOdd));
                        accel = 0.1f;
                        brake = 0.06f;
                        break;
                    case float f when Mathf.Abs(targetAngleOdd) > 25:
                        Debug.Log(this.transform.gameObject.name + " Curv, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleOdd));
                        accel = 0.3f;
                        brake = 0.02f;
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " Straight, 25 or less Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleOdd));
                        Debug.Log(this.transform.gameObject.name + " drivingControl.currentSpeed: " + mphSpeedInt + " MPH"); //showing the current speed in MPH
                        switch (mphSpeedInt)
                        {
                            case int i when mphSpeedInt < 61:
                                Debug.Log(this.transform.gameObject.name + " Straight line, bring it up to 60mph!");
                                accel = 0.8f;
                                brake = 0f;
                                break;
                            case int i when mphSpeedInt > 79:
                                Debug.Log(this.transform.gameObject.name + " Bring it down below 80mph!");
                                accel = publicAccel;
                                brake = publicBrake;
                                break;
                            default:
                                // the current speed is between 60 and 80mph
                                break;
                        }
                        break;
                }
            }

            //Find out the next waypoint's y position to be at uphill or downhill; MY AI CODE!
            if (currentPointOdd != 0) //avoiding subtracting -1 from currentPoint[0]
            {
                float nextWaypoint = wayPointsOdd.waypoints[currentPointOdd].transform.position.y;
                float currentWaypoint = wayPointsOdd.waypoints[currentPointOdd - 1].transform.position.y;
                float yDifference = nextWaypoint - currentWaypoint;
                switch (yDifference)
                {
                    case float f when yDifference > 1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on uphill: " + yDifference);
                        if (mphSpeedInt < 40)
                        {
                            accel = 3f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 1f;
                            brake = 0F;
                        }
                        break;
                    case float f when yDifference < -1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on downhill: " + yDifference);
                        if (mphSpeedInt < 60)
                        {
                            accel = 0.5f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 0.01f;
                            brake = 0.1F;
                        }
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " the target on flat ground: " + yDifference);
                        break;
                }
            }
            //Testing Overtaking, targetAngle += to turn right, -= to turn left 
            if (mphSpeedInt > 34 && Time.time > unStickTime) // if driving 35mph or faster & not being stuck
            {
                Debug.Log(this.transform.gameObject.name + " Speed > 34mph & Not Being Stuck == " + (Time.time > unStickTime));

                // About to hit ahead
                if (raycasting.aboutToHitLeftAhead == true)
                {
                    targetAngleOdd += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitLeftAhead, targetAngle = " + targetAngleOdd);
                }
                if (raycasting.aboutToHitDirectlyAhead == true) // Avoiding frontal collisions with other cars
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Frontal Collisions!!!");
                    accel = 0.4f;
                    brake = 0.01f;
                }
                if (raycasting.aboutToHitRightAhead == true)
                {
                    targetAngleOdd -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitRightAhead, targetAngle = " + targetAngleOdd);
                }
                // About to get hit from rear
                if (raycasting.aboutToGetHitRightRear == true)
                {
                    targetAngleOdd -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitRightRear, targetAngle = " + targetAngleOdd);
                }
                if (raycasting.aboutToGetHitRear == true) // Avoiding getting rear ended with another car
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Rear-End!!!");
                    accel = 0.6f;
                    brake = 0f;
                }
                if (raycasting.aboutToGetHitLeftRear == true)
                {
                    targetAngleOdd += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitLeftRear, targetAngle = " + targetAngleOdd);
                }
                // Hitting the side
                if (raycasting.isHittingRightSide == true)
                {
                    targetAngleOdd -= 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightSide, targetAngle = " + targetAngleOdd);
                }
                if (raycasting.isHittingLeftSide == true)
                {
                    targetAngleOdd += 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftSide, targetAngle = " + targetAngleOdd);
                }
                // A part of the front half is hitting
                if (raycasting.isHittingLeft == true)
                {
                    targetAngleOdd += 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeft, targetAngle = " + targetAngleOdd);
                }
                if (raycasting.isHittingRight == true)
                {
                    targetAngleOdd -= 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingRight, targetAngle = " + targetAngleOdd);
                }
                // A part of the rear half is hitting
                if (raycasting.isHittingRightRear == true)
                {
                    targetAngleOdd -= 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightRear, targetAngle = " + targetAngleOdd);
                }
                if (raycasting.isHittingLeftRear == true)
                {
                    targetAngleOdd += 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftRear, targetAngle = " + targetAngleOdd);
                }

                steer = Mathf.Clamp(targetAngleOdd * steeringSensitivity, -1, 1);
            }

            if (distanceToTargetOdd < 7) // Increased from 5 to be lenient for multiple cars reaching the waypoints simultaneously
            {
                if (currentPointOdd == 0 && roundTrip == false) //first time approaching the point [0]
                {
                    Debug.Log(this.transform.gameObject.name + " First time approaching the point [" + currentPointOdd + "]");
                    Debug.Log(this.transform.gameObject.name + " publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
                }
                if (currentPointOdd == 0 && roundTrip == true) //making a round trip
                {
                    Debug.Log(this.transform.gameObject.name + " Making a round trip!");
                }
            }

            if (distanceToTargetOdd < 5) /* increase the value if gameObject circles around waypoint. 
                                       * Increased from 2 to be lenient for multiple cars reaching the waypoints simultaneously */
            {
                currentPointOdd++;
                // DO NOT PUT ANY WAYPOINT RELATED CODE / DEBUG BELOW!! IT'LL MESS THINGS UP!!
                if (currentPointOdd >= wayPointsOdd.waypoints.Length)
                {
                    roundTrip = true;
                    currentPointOdd = 0;
                    Debug.Log(this.transform.gameObject.name + " target is back to the origin [" + currentPointOdd + "]");
                }

                targetOdd = wayPointsOdd.waypoints[currentPointOdd].transform.position;
                // DO NOT PUT ANY WAYPOINT RELATED CODE / DEBUG ABOVE!! IT'LL MESS THINGS UP!!

                Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPointsOdd.waypoints[currentPointOdd].name);
                nextWaypointNameOdd = wayPointsOdd.waypoints[currentPointOdd].name; // For debug purpose only
            }

            // Getting Cars Unstuck 
            if (mphSpeedInt < 20) // hardly moving at 19mph or less
            {
                if (!(currentPointOdd == 0 && roundTrip == false)) // The car is not at the starting point of the race
                {
                    if (raycasting.isHittingFrontHalf == true && raycasting.isHittingRear == false) // if any gameObject blocking front half but none directly behind
                    {
                        unStickTime = Time.time + unStickDuration; // Setting the time to do unstuck codes for unStickDuration
                        if (Time.time < unStickTime) // if unStickTime hasn't reached Time.time yet, drive backward toward -2 previous waypoint at minus 800 wheelColliders[i].motorTorque
                        {
                            if (currentPointOdd > 1)
                            {
                                targetOdd = wayPointsOdd.waypoints[currentPointOdd - 2].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 2) " + wayPointsOdd.waypoints[currentPointOdd - 2].name);
                                nextWaypointNameOdd = wayPointsOdd.waypoints[currentPointOdd - 2].name; // For debug purpose only
                            }
                            else if (currentPointOdd == 1)
                            {
                                targetOdd = wayPointsOdd.waypoints[currentPointOdd - 1].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 1) " + wayPointsOdd.waypoints[currentPointOdd - 1].name);
                                nextWaypointNameOdd = wayPointsOdd.waypoints[currentPointOdd - 1].name; // For debug purpose only
                            }
                            else
                            {
                                targetOdd = wayPointsOdd.waypoints[currentPointOdd].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 0) " + wayPointsOdd.waypoints[currentPointOdd].name);
                                nextWaypointNameOdd = wayPointsOdd.waypoints[currentPointOdd].name; // For debug purpose only
                            }
                            localTargetOdd = drivingControl.rb.gameObject.transform.InverseTransformPoint(targetOdd);
                            targetAngleOdd = Mathf.Atan2(localTargetOdd.x, localTargetOdd.z) * Mathf.Rad2Deg;
                            steer = Mathf.Clamp(targetAngleOdd * steeringSensitivity, -1, 1); // (targetAngleOdd * -1) to try directing towards the previous waypoint while driving backward, SAME RESULT AS * +1
                            brake = 0f;
                            accel = -4f;
                            Debug.Log(this.transform.gameObject.name + " drivingControl.Go Backward(" + accel + ", " + steer + ", " + brake + ")");
                            drivingControl.Go(accel, steer, brake);
                        }
                        targetOdd = wayPointsOdd.waypoints[currentPointOdd].transform.position;

                        //ANY OF THE FOLLOWING LINES PLACING HERE WILL MESS YOU UP!!!
                        //localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                        //targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                        //steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
                        //accel = 3f; // drive forward at 600 wheelColliders[i].motorTorque after driving backward
                        //brake = 0f;
                        //ANY OF THE ABOVE LINES PLACING HERE WILL MESS YOU UP!!!
                    }
                }
            }
            Debug.Log(this.transform.gameObject.name + " drivingControl.Go(" + accel + ", " + steer + ", " + brake + ")");
            drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
            drivingControl.CheckSkidding();
            drivingControl.calculateEngineSound();
            //***ONE-WAY TRAFFIC ORIGINAL CODES (CLEANED & MODIFED) WITHIN TWO-WAY TRAFFIC IF{} CODES END HERE 4/10/22***
        }
        // else //***ONE-WAY TRAFFIC ORIGINAL CODES WITHIN TWO-WAY TRAFFIC ELSE{} TEST CODES BEGIN 4/10/22***
        else if (this.transform.gameObject.tag == "Even") //***TRAFFIC CONTROL ELSE IF{} TEST CODES BEGIN 4/27/22*** 
        {
            Vector3 localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
            float targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            float distanceToTarget = Vector3.Distance(target, drivingControl.rb.gameObject.transform.position);

            /* Finding out the angle to the next target, waypoint to waypoint, version 1; MY AI CODE!
               Does Not Quite Work because POV is World based */
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

            /* The following values need to be updated to get the car going and keep it under control
               Brought up here from below */
            //float accel = 0.5f; //original value
            float accel = 0.5f;
            float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1); // "Commenting Out for Overtaking Test" * Mathf.Sign(drivingControl.currentSpeed);
            //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
            //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
            float brake = 0f;

            //Get the distance to the next waypoint
            //Debug.Log("distanceToTarget: " + (int)distanceToTarget + " Meters");
            //double distanceInMile = distanceToTarget * 0.000621371;
            //decimal distanceInMileInDecimal = (decimal)distanceInMile;
            //Debug.Log("distanceInMileInDecimal Rounded 4: " + decimal.Round(distanceInMileInDecimal, 4) + " Miles");

            //Debug.Log("next waypoint is " + wayPoints.waypoints[currentPoint].name); // WRONG SPOT TO BE QUERYING THESE!!
            //nextWaypointName = wayPoints.waypoints[currentPoint].name; // For debug purpose only
            //Debug.Log("targetAngle is " + targetAngle);

            //Get the speed in MPH
            double mphSpeed = (drivingControl.currentSpeed * 3600) * 0.000621371;
            mphSpeedInt = (int)mphSpeed;
            //Debug.Log("drivingControl.currentSpeed: " + mphSpeedInt + " MPH");

            //Finding out the angle to the next target, drivingControl.rb.gameObject.transform to waypoint, version 2; MY AI CODE!
            //Works well, POV is that of drivingControl.rb.gameObject.transform.position
            if (mphSpeedInt > 49)
            {
                switch (Mathf.Abs(targetAngle))
                {
                    case float f when Mathf.Abs(targetAngle) > 50:
                        Debug.Log(this.transform.gameObject.name + " Sharp Curv, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                        accel = 0.1f;
                        brake = 0.06f;
                        break;
                    case float f when Mathf.Abs(targetAngle) > 25:
                        Debug.Log(this.transform.gameObject.name + " Curv, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                        accel = 0.3f;
                        brake = 0.02f;
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " Straight, 25 or less Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                        Debug.Log(this.transform.gameObject.name + " drivingControl.currentSpeed: " + mphSpeedInt + " MPH"); //showing the current speed in MPH
                        switch (mphSpeedInt)
                        {
                            case int i when mphSpeedInt < 61:
                                Debug.Log(this.transform.gameObject.name + " Straight line, bring it up to 60mph!");
                                accel = 0.8f;
                                brake = 0f;
                                break;
                            case int i when mphSpeedInt > 79:
                                Debug.Log(this.transform.gameObject.name + " Bring it down below 80mph!");
                                accel = publicAccel;
                                brake = publicBrake;
                                break;
                            default:
                                // the current speed is between 60 and 80mph
                                break;
                        }
                        break;
                }
            }

            //Find out the next waypoint's y position to be at uphill or downhill; MY AI CODE!
            if (currentPoint != 0) //avoiding subtracting -1 from currentPoint[0]
            {
                float nextWaypoint = wayPoints.waypoints[currentPoint].transform.position.y;
                float currentWaypoint = wayPoints.waypoints[currentPoint - 1].transform.position.y;
                float yDifference = nextWaypoint - currentWaypoint;
                switch (yDifference)
                {
                    case float f when yDifference > 1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on uphill: " + yDifference);
                        if (mphSpeedInt < 40)
                        {
                            accel = 3f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 1f;
                            brake = 0F;
                        }
                        break;
                    case float f when yDifference < -1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on downhill: " + yDifference);
                        if (mphSpeedInt < 60)
                        {
                            accel = 0.5f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 0.01f;
                            brake = 0.1F;
                        }
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " the target on flat ground: " + yDifference);
                        break;
                }
            }

            /* For AI_Controller to grab
            aboutToHitAhead = false;
            aboutToHitLeftAhead = false;
            aboutToHitDirectlyAhead = false;
            aboutToHitRightAhead = false;
            //
            aboutToGetHitRightRear = false;
            aboutToGetHitRear = false;
            aboutToGetHitLeftRear = false;
            //
            isHittingRightSide = false;
            isHittingLeftSide = false;
            //
            isHittingFrontHalf = false;
            isHittingLeft = false;
            isHittingFront = false;
            isHittingRight = false;
            //
            isHittingRearHalf = false;
            isHittingRightRear = false;
            isHittingRear = false;
            isHittingLeftRear = false;
            */
            //Testing Overtaking, targetAngle += to turn right, -= to turn left 
            if (mphSpeedInt > 34 && Time.time > unStickTime) // if driving 35mph or faster & not being stuck
            {
                Debug.Log(this.transform.gameObject.name + " Speed > 34mph & Not Being Stuck == " + (Time.time > unStickTime));

                // About to hit ahead
                if (raycasting.aboutToHitLeftAhead == true)
                {
                    targetAngle += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitLeftAhead, targetAngle = " + targetAngle);
                }
                if (raycasting.aboutToHitDirectlyAhead == true) // Avoiding frontal collisions with other cars
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Frontal Collisions!!!");
                    accel = 0.4f;
                    brake = 0.01f;
                }
                if (raycasting.aboutToHitRightAhead == true)
                {
                    targetAngle -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitRightAhead, targetAngle = " + targetAngle);
                }
                // About to get hit from rear
                if (raycasting.aboutToGetHitRightRear == true)
                {
                    targetAngle -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitRightRear, targetAngle = " + targetAngle);
                }
                if (raycasting.aboutToGetHitRear == true) // Avoiding getting rear ended with another car
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Rear-End!!!");
                    accel = 0.6f;
                    brake = 0f;
                }
                if (raycasting.aboutToGetHitLeftRear == true)
                {
                    targetAngle += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitLeftRear, targetAngle = " + targetAngle);
                }
                // Hitting the side
                if (raycasting.isHittingRightSide == true)
                {
                    targetAngle -= 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightSide, targetAngle = " + targetAngle);
                }
                if (raycasting.isHittingLeftSide == true)
                {
                    targetAngle += 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftSide, targetAngle = " + targetAngle);
                }
                // A part of the front half is hitting
                if (raycasting.isHittingLeft == true)
                {
                    targetAngle += 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeft, targetAngle = " + targetAngle);
                }
                if (raycasting.isHittingRight == true)
                {
                    targetAngle -= 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingRight, targetAngle = " + targetAngle);
                }
                // A part of the rear half is hitting
                if (raycasting.isHittingRightRear == true)
                {
                    targetAngle -= 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightRear, targetAngle = " + targetAngle);
                }
                if (raycasting.isHittingLeftRear == true)
                {
                    targetAngle += 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftRear, targetAngle = " + targetAngle);
                }

                steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1);
            }

            //DOESN'T WORK WELL! STOPS ON ANY OBJECT CAR SENSES AHEAD!
            /* if (mphSpeedInt < 35 && Time.time > unStickTime) // if driving below 35mph & unStickTime codes not being run
            {
                Debug.Log("Speed < 35mph & Not Being Stuck == " + (Time.time > unStickTime));

                // About to hit gameObject ahead. 
                if (raycasting.aboutToHitAhead == true && targetAngle > 0)
                {
                    targetAngle += 20;
                    Debug.Log("aboutToHitAhead, waypoint on the right: " + targetAngle);
                }
                else if (raycasting.aboutToHitAhead == true && targetAngle < 0)
                {
                    targetAngle -= 20;
                    Debug.Log("aboutToHitAhead, waypoint on the left: " + targetAngle);
                }

                // Hitting the side of "Car"
                if (raycasting.isHittingRightSide == true)
                {
                    targetAngle -= 5;
                    Debug.Log("isHittingRightSide");
                }
                if (raycasting.isHittingLeftSide == true)
                {
                    targetAngle += 5;
                    Debug.Log("isHittingLeftSide");
                }

                // The front half is hitting
                if (raycasting.isHittingFrontHalf == true && targetAngle > 0)
                {
                    targetAngle += 30;
                    Debug.Log("isHittingFrontHalf, waypoint on the right: " + targetAngle);
                }
                else if (raycasting.isHittingFrontHalf == true && targetAngle < 0)
                {
                    targetAngle -= 30;
                    Debug.Log("isHittingFrontHalf, waypoint on the left: " + targetAngle);
                } 

                steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
            } */
            // DOESN'T WORK WELL! STOPS ON ANY OBJECT CAR SENSES AHEAD!


            //Commenting out to bring the following above
            ////float accel = 0.5f; //original value
            //float accel = 0.5f;
            //float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
            ////Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
            ////Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
            //float brake = 0;

            if (distanceToTarget < 7) // Increased from 5 to be lenient for multiple cars reaching the waypoints simultaneously
            {
                if (currentPoint == 0 && roundTrip == false) //first time approaching the point [0]
                {
                    Debug.Log(this.transform.gameObject.name + " First time approaching the point [" + currentPoint + "]");
                    Debug.Log(this.transform.gameObject.name + " publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
                }
                if (currentPoint == 0 && roundTrip == true) //making a round trip
                {
                    Debug.Log(this.transform.gameObject.name + " Making a round trip!");
                }
            }

            if (distanceToTarget < 5) /* increase the value if gameObject circles around waypoint. 
                                       * Increased from 2 to be lenient for multiple cars reaching the waypoints simultaneously */
            {
                currentPoint++;
                // DO NOT PUT ANY WAYPOINT RELATED CODE / DEBUG BELOW!! IT'LL MESS THINGS UP!!
                if (currentPoint >= wayPoints.waypoints.Length)
                {
                    roundTrip = true;
                    currentPoint = 0;
                    Debug.Log(this.transform.gameObject.name + " target is back to the origin [" + currentPoint + "]");
                    //steeringSensitivity = 0.008f; maybe no need to lower the value

                    //COMMENTING OUT TO TRY OUT MY AI CODE!
                    //accel = publicAccel;
                    //brake = publicBrake;
                    //Debug.Log("accel = publicAccel & brake = publicBrake: " + publicAccel + " " + publicBrake);
                }

                target = wayPoints.waypoints[currentPoint].transform.position;
                // DO NOT PUT ANY WAYPOINT RELATED CODE / DEBUG ABOVE!! IT'LL MESS THINGS UP!!

                Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPoints.waypoints[currentPoint].name);
                nextWaypointName = wayPoints.waypoints[currentPoint].name; // For debug purpose only

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

            // Getting Cars Unstuck 
            if (mphSpeedInt < 20) // hardly moving at 19mph or less
            {
                if (!(currentPoint == 0 && roundTrip == false)) // The car is not at the starting point of the race
                {
                    /*if ((raycasting.isHittingFront == true || raycasting.aboutToHitAhead == true) && raycasting.isHittingRear == false)*/ // if any gameObject blocking or about to block ahead but none directly behind
                    if (raycasting.isHittingFrontHalf == true && raycasting.isHittingRear == false) // if any gameObject blocking front half but none directly behind
                    /*if ((raycasting.isHittingFrontHalf == true || mphSpeedInt < 1) && (raycasting.isHittingRear == false || raycasting.isHittingRearHalf == false))*/ // This one doesn't seem to work well!!
                    {
                        //orgTargetAngle = targetAngle; // saving the original targetAngle value

                        //switch (targetAngle) // Backing up with extreme angle depending on targetAngle location
                        //{
                        //    case float f when targetAngle < -79f:
                        //        Debug.Log("targetAngle on the left: " + targetAngle);
                        //        targetAngle += 60f;
                        //        Debug.Log("Moving Cars Back To The Right: " + targetAngle);
                        //        break;
                        //    case float f when targetAngle < -39f:
                        //        Debug.Log("targetAngle on the left: " + targetAngle);
                        //        targetAngle += 40f;
                        //        Debug.Log("Moving Cars Back To The Right: " + targetAngle);
                        //        break;
                        //    case float f when targetAngle > 79f:
                        //        Debug.Log("targetAngle on the right: " + targetAngle);
                        //        targetAngle -= 60f;
                        //        Debug.Log("Moving Cars Back To The Left: " + targetAngle);
                        //        break;
                        //    case float f when targetAngle > 39f:
                        //        Debug.Log("targetAngle on the right: " + targetAngle);
                        //        targetAngle -= 40f;
                        //        Debug.Log("Moving Cars Back To The Left: " + targetAngle);
                        //        break;
                        //    default:
                        //        Debug.Log("targetAngle in close to straight line: " + targetAngle);
                        //        if (targetAngle < 0) // targetAngle slightly on my left
                        //        {
                        //            targetAngle -= 60f;
                        //        }
                        //        else
                        //        {
                        //            targetAngle += 60f; // assuming targetAngle slightly on my right
                        //        }
                        //        Debug.Log("targetAngle +/- 60: " + targetAngle);
                        //        break;
                        //}

                        unStickTime = Time.time + unStickDuration; // Setting the time to do unstuck codes for unStickDuration
                        if (Time.time < unStickTime) // if unStickTime hasn't reached Time.time yet, drive backward toward -2 previous waypoint at minus 800 wheelColliders[i].motorTorque
                        {
                            if (currentPoint > 1)
                            {
                                target = wayPoints.waypoints[currentPoint - 2].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 2) " + wayPoints.waypoints[currentPoint - 2].name);
                                nextWaypointName = wayPoints.waypoints[currentPoint - 2].name; // For debug purpose only
                            }
                            else if (currentPoint == 1)
                            {
                                target = wayPoints.waypoints[currentPoint - 1].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 1) " + wayPoints.waypoints[currentPoint - 1].name);
                                nextWaypointName = wayPoints.waypoints[currentPoint - 1].name; // For debug purpose only
                            }
                            else
                            {
                                target = wayPoints.waypoints[currentPoint].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 0) " + wayPoints.waypoints[currentPoint].name);
                                nextWaypointName = wayPoints.waypoints[currentPoint].name; // For debug purpose only
                            }
                            localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                            targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                            steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1); // (targetAngle * -1) to try directing towards the previous waypoint while driving backward, SAME RESULT AS * +1
                            brake = 0f;
                            accel = -4f;
                            Debug.Log(this.transform.gameObject.name + " drivingControl.Go Backward(" + accel + ", " + steer + ", " + brake + ")");
                            drivingControl.Go(accel, steer, brake);
                        }
                        target = wayPoints.waypoints[currentPoint].transform.position;

                        //ANY OF THE FOLLOWING LINES PLACING HERE WILL MESS YOU UP!!!
                        //localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                        //targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                        //steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
                        //accel = 3f; // drive forward at 600 wheelColliders[i].motorTorque after driving backward
                        //brake = 0f;
                        //ANY OF THE ABOVE LINES PLACING HERE WILL MESS YOU UP!!!
                    }

                    // DRIVING FORWARD CODE GOES HERE AFTER GETTING STUCK & HAS BACKED UP, BUT DOES NOT WORK ACCORDING TO CODES!!
                    //unStickTime += unStickDuration; // Adding additional time duration for driving forward task
                    //if (Time.time < unStickTime /* && (raycasting.aboutToHitAhead == false || raycasting.isHittingFrontHalf == false) */ )
                    //{
                    //    targetAngle = orgTargetAngle;
                    //    //Debug.Log("aboutToHit or Hitting Ahead == true, targetAngle = " + targetAngle);
                    //    //if (targetAngle < 0) // targetAngle slightly on my left
                    //    //{
                    //    //    targetAngle -= 150f; // EXTREME ANGLE METHOD DOESN'T WORK, MISSES NEXT WAYPOINT!
                    //    //}
                    //    //else // assuming targetAngle slightly on my right
                    //    //{
                    //    //    targetAngle += 150f; 
                    //    //}
                    //    accel = 3f; // move forward at 600 wheelColliders[i].motorTorque
                    //    steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2); // changing values just for getting cars unstuck
                    //    brake = 0f;
                    //    Debug.Log("drivingControl.Go Forward(" + accel + ", " + steer + ", " + brake + ")");
                    //    Debug.Log("Being Stuck == " + (Time.time < unStickTime));
                    //    drivingControl.Go(accel, steer, brake);
                    //}
                }
            }
            Debug.Log(this.transform.gameObject.name + " drivingControl.Go(" + accel + ", " + steer + ", " + brake + ")");
            drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
            drivingControl.CheckSkidding();
            drivingControl.calculateEngineSound();
            //***ONE-WAY TRAFFIC ORIGINAL CODES WITHIN TWO-WAY TRAFFIC ELSE{} CODES END HERE 4/10/22***
        }
        //***TWO-WAY TRAFFIC IF/ELSE{} TEST CODES END 4/10/22***
        else //***TRAFFIC CONTROL ELSE IF{} TEST CODES BEGIN (CONTINUE ALL THE WAY TO THE END OF THE PAGE) 5/01/22***
        {
            Vector3 localTargetTrfc = drivingControl.rb.gameObject.transform.InverseTransformPoint(targetTrfc);
            float targetAngleTrfc = Mathf.Atan2(localTargetTrfc.x, localTargetTrfc.z) * Mathf.Rad2Deg;
            float distanceToTargetTrfc = Vector3.Distance(targetTrfc, drivingControl.rb.gameObject.transform.position);

            /* The following values need to be updated to get the car going and keep it under control
               Brought up here from below */
            //float accel = 0.5f; //original value
            float accel = 0.5f;
            float steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -1, 1); // "Commenting Out for Overtaking Test" * Mathf.Sign(drivingControl.currentSpeed);
            //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
            //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
            float brake = 0f;

            //Get the distance to the next waypoint
            //Debug.Log("distanceToTarget: " + (int)distanceToTarget + " Meters");
            //double distanceInMile = distanceToTarget * 0.000621371;
            //decimal distanceInMileInDecimal = (decimal)distanceInMile;
            //Debug.Log("distanceInMileInDecimal Rounded 4: " + decimal.Round(distanceInMileInDecimal, 4) + " Miles");

            //Get the speed in MPH
            double mphSpeed = (drivingControl.currentSpeed * 3600) * 0.000621371;
            mphSpeedInt = (int)mphSpeed;
            //Debug.Log("drivingControl.currentSpeed: " + mphSpeedInt + " MPH");

            //Finding out the angle to the next target, drivingControl.rb.gameObject.transform to waypoint, version 2; MY AI CODE!
            //Works well, POV is that of drivingControl.rb.gameObject.transform.position
            if (mphSpeedInt > 29)
            {
                switch (Mathf.Abs(targetAngleTrfc))
                {
                    case float f when Mathf.Abs(targetAngleTrfc) > 50:
                        Debug.Log(this.transform.gameObject.name + " Sharp Curv, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        accel = 0.1f;
                        brake = 0.06f;
                        break;
                    case float f when Mathf.Abs(targetAngleTrfc) > 25:
                        Debug.Log(this.transform.gameObject.name + " Curv, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        accel = 0.3f;
                        brake = 0.02f;
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " Straight, 25 or less Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        Debug.Log(this.transform.gameObject.name + " drivingControl.currentSpeed: " + mphSpeedInt + " MPH"); //showing the current speed in MPH
                        switch (mphSpeedInt)
                        {
                            case int i when mphSpeedInt < 21:
                                Debug.Log(this.transform.gameObject.name + " Straight line, bring it up a bit");
                                accel = 0.8f;
                                brake = 0f;
                                break;
                            case int i when mphSpeedInt > 39:
                                Debug.Log(this.transform.gameObject.name + " Bring it down below 80mph!");
                                accel = publicAccel;
                                brake = publicBrake;
                                break;
                            default:
                                // the current speed is between 60 and 80mph
                                break;
                        }
                        break;
                }
            }

            //Find out the next waypoint's y position to be at uphill or downhill; MY AI CODE!
            if (currentPointTrfc != 0) //avoiding subtracting -1 from currentPoint[0]
            {
                float nextWaypointTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position.y;
                float currentWaypointTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].transform.position.y;
                float yDifference = nextWaypointTrfc - currentWaypointTrfc;
                switch (yDifference)
                {
                    case float f when yDifference > 1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on uphill: " + yDifference);
                        if (mphSpeedInt < 40)
                        {
                            accel = 3f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 1f;
                            brake = 0F;
                        }
                        break;
                    case float f when yDifference < -1.5:
                        Debug.Log(this.transform.gameObject.name + " the target on downhill: " + yDifference);
                        if (mphSpeedInt < 60)
                        {
                            accel = 0.5f;
                            brake = 0f;
                        }
                        else
                        {
                            accel = 0.01f;
                            brake = 0.1F;
                        }
                        break;
                    default:
                        Debug.Log(this.transform.gameObject.name + " the target on flat ground: " + yDifference);
                        break;
                }
            }

            /* For AI_Controller to grab
            aboutToHitAhead = false;
            aboutToHitLeftAhead = false;
            aboutToHitDirectlyAhead = false;
            aboutToHitRightAhead = false;
            //
            aboutToGetHitRightRear = false;
            aboutToGetHitRear = false;
            aboutToGetHitLeftRear = false;
            //
            isHittingRightSide = false;
            isHittingLeftSide = false;
            //
            isHittingFrontHalf = false;
            isHittingLeft = false;
            isHittingFront = false;
            isHittingFrontTrfc = false; // For Traffic Control Codes
            isHittingRight = false;
            //
            isHittingRearHalf = false;
            isHittingRightRear = false;
            isHittingRear = false;
            isHittingLeftRear = false;
            */
            //Testing Overtaking, targetAngle += to turn right, -= to turn left 
            if (mphSpeedInt > 19 && Time.time > unStickTime) // if driving 35mph or faster & not being stuck
            {
                Debug.Log(this.transform.gameObject.name + " Speed > 34mph & Not Being Stuck == " + (Time.time > unStickTime));

                // About to hit ahead
                if (raycasting.aboutToHitLeftAhead == true)
                {
                    targetAngleTrfc += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitLeftAhead, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.aboutToHitDirectlyAhead == true) // Avoiding frontal collisions with other cars
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Frontal Collisions!!!");
                    accel = 0.4f;
                    brake = 0.01f;
                }
                if (raycasting.aboutToHitRightAhead == true)
                {
                    targetAngleTrfc -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToHitRightAhead, targetAngle = " + targetAngleTrfc);
                }
                // About to get hit from rear
                if (raycasting.aboutToGetHitRightRear == true)
                {
                    targetAngleTrfc -= 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitRightRear, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.aboutToGetHitRear == true) // Avoiding getting rear ended with another car
                {
                    Debug.Log(this.transform.gameObject.name + " Avoiding Rear-End!!!");
                    accel = 0.6f;
                    brake = 0f;
                }
                if (raycasting.aboutToGetHitLeftRear == true)
                {
                    targetAngleTrfc += 30;
                    Debug.Log(this.transform.gameObject.name + " aboutToGetHitLeftRear, targetAngle = " + targetAngleTrfc);
                }
                // Hitting the side
                if (raycasting.isHittingRightSide == true)
                {
                    targetAngleTrfc -= 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightSide, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingLeftSide == true)
                {
                    targetAngleTrfc += 10;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftSide, targetAngle = " + targetAngleTrfc);
                }
                // A part of the front half is hitting
                if (raycasting.isHittingLeft == true)
                {
                    targetAngleTrfc += 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeft, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingRight == true)
                {
                    targetAngleTrfc -= 15;
                    Debug.Log(this.transform.gameObject.name + " isHittingRight, targetAngle = " + targetAngleTrfc);
                }
                // A part of the rear half is hitting
                if (raycasting.isHittingRightRear == true)
                {
                    targetAngleTrfc -= 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingRightRear, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingLeftRear == true)
                {
                    targetAngleTrfc += 20;
                    Debug.Log(this.transform.gameObject.name + " isHittingLeftRear, targetAngle = " + targetAngleTrfc);
                }

                steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -1, 1);
            }

            // The increment of a current waypoint (currentPointTrfc) occurs as the car approaches the current waypoint at currentPointTrfc < 3 distance.
            if (distanceToTargetTrfc < 3) // increase the value if gameObject circles around the waypoint.
            {
                bool leftTurn = false;
                bool rightTurn = false;
                bool straight = false;
                currentPointTrfc++; // As soon as the car reaches a current waypoint, currentPointTrfc gets incremented to the next point
                roundTrip = true; // Since it's not looping, as soon as the first increment occurs, changing the value to true.
                if (currentPointTrfc == 9) // if/when the car has reached at the Cube (8), NOT at Cube (9),
                {
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // left turn
                    {
                        currentPointTrfc = 9; // setting the next waypoint to the Cube (9), which is a left turn
                        leftTurn = true;
                        //Debug.Log("TRFC, " + this.transform.gameObject.name + "'s leftTurn = " + leftTurn + " and currentPointTrfc is " + currentPointTrfc);
                    }
                    else // right turn
                    {
                        currentPointTrfc = 10;
                    }
                }
                if (leftTurn == true && currentPointTrfc == 9) // if the car has made a left turn at Cube (8) and the next waypoint has been set to the Cube (9),
                {
                    currentPointTrfc = 10; // Wait until the car reaches the Cube (9). At that point, setting currentPointTrfc to (10).
                }
                if (leftTurn == true && currentPointTrfc == 10) // if the car has made a left turn and has reached the Cube (9),
                {
                    currentPointTrfc = 0; // Only then can you set the currentPointTrfc to (0)!
                                          // In actuality, the car goes directly from Cube (8) to (0), so Cube (0) and (9) are placed next to each other in the scene to workaround!
                }
                if (currentPointTrfc == 15) // just arrived at the corner (14)
                {
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 19;
                        straight = true;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 15;
                        rightTurn = true;
                    }
                }
                if (currentPointTrfc == 19 && raycasting.isHittingFrontTrfc == true) // just arrived at the corner (18)
                {
                    Debug.Log("TRFC, HELLOOOO!!!");
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // turning right
                    {
                        currentPointTrfc = 7;
                    }
                    else if (randomNumber == 2) // straight
                    {
                        currentPointTrfc = 22;
                    }
                    else // turning left
                    {
                        currentPointTrfc = 23;
                    }
                }

                //currentPointTrfc++; // As soon as the car reaches a current waypoint, currentPointTrfc gets incremented to the next point

                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // Finally, the target angle is processed accordingly. 

                Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPointsTrfc.waypoints[currentPointTrfc].name);
                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name; // For debug purpose only
            } 

            // Getting Cars Unstuck 
            if (mphSpeedInt < 11) // hardly moving at 10 mph or less
            {
                if (!(currentPointTrfc == 0 && roundTrip == false)) // The car is not at the starting point of the race
                {
                    if (raycasting.isHittingFrontHalf == true && raycasting.isHittingRear == false) // if any gameObject blocking front half but none directly behind
                    {
                        unStickTime = Time.time + unStickDuration; // Setting the time to do unstuck codes for unStickDuration
                        if (Time.time < unStickTime) // if unStickTime hasn't reached Time.time yet, drive backward toward -2 previous waypoint at minus 800 wheelColliders[i].motorTorque
                        {
                            if (currentPointTrfc > 1)
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 2].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 2) " + wayPointsTrfc.waypoints[currentPointTrfc - 2].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 2].name; // For debug purpose only
                            }
                            else if (currentPoint == 1)
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 1) " + wayPointsTrfc.waypoints[currentPointTrfc - 1].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].name; // For debug purpose only
                            }
                            else
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;
                                Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 0) " + wayPointsTrfc.waypoints[currentPointTrfc].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name; // For debug purpose only
                            }
                            localTargetTrfc = drivingControl.rb.gameObject.transform.InverseTransformPoint(targetTrfc);
                            targetAngleTrfc = Mathf.Atan2(localTargetTrfc.x, localTargetTrfc.z) * Mathf.Rad2Deg;
                            steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -1, 1); // (targetAngle * -1) to try directing towards the previous waypoint while driving backward, SAME RESULT AS * +1
                            brake = 0f;
                            accel = -4f;
                            Debug.Log(this.transform.gameObject.name + " drivingControl.Go Backward(" + accel + ", " + steer + ", " + brake + ")");
                            drivingControl.Go(accel, steer, brake);
                        }
                        targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;

                        //ANY OF THE FOLLOWING LINES PLACING HERE WILL MESS YOU UP!!!
                        //localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                        //targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                        //steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
                        //accel = 3f; // drive forward at 600 wheelColliders[i].motorTorque after driving backward
                        //brake = 0f;
                        //ANY OF THE ABOVE LINES PLACING HERE WILL MESS YOU UP!!!
                    }
                }
            }
            Debug.Log(this.transform.gameObject.name + " drivingControl.Go(" + accel + ", " + steer + ", " + brake + ")");
            drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
            drivingControl.CheckSkidding();
            drivingControl.calculateEngineSound();
        } //***TRAFFIC CONTROL ELSE IF{} TEST CODES END 5/01/22***
    }
}
