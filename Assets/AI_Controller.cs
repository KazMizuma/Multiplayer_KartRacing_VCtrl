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

    RaycastingTrfc raycastingTrfc; // 6/23 Trfc Ctrl, For Cube (8)
    RaycastingTrfc2 raycastingTrfc2;
    RaycastingTrfc3 raycastingTrfc3;
    RaycastingTrfc4 raycastingTrfc4;

    RaycastingTrfc5 raycastingTrfc5; // 6/25 Trfc Ctrl, For Cube (27)
    RaycastingTrfc6 raycastingTrfc6;
    RaycastingTrfc7 raycastingTrfc7;
    RaycastingTrfc8 raycastingTrfc8;

    RaycastingTrfc9 raycastingTrfc9; // 6/26 Trfc Ctrl, For Cube (53)
    RaycastingTrfc10 raycastingTrfc10;
    RaycastingTrfc11 raycastingTrfc11;
    RaycastingTrfc12 raycastingTrfc12;

    RaycastingTrfc13 raycastingTrfc13; // 6/26 Trfc Ctrl, For Cube (53)
    RaycastingTrfc14 raycastingTrfc14;
    RaycastingTrfc15 raycastingTrfc15;
    RaycastingTrfc16 raycastingTrfc16;

    RaycastingTrfc17 raycastingTrfc17; // 6/26 Trfc Ctrl, For Cube (47)
    RaycastingTrfc18 raycastingTrfc18;
    RaycastingTrfc19 raycastingTrfc19;
    RaycastingTrfc20 raycastingTrfc20;

    RaycastingTrfc21 raycastingTrfc21; // 6/26 Trfc Ctrl, For Cube (37)
    RaycastingTrfc22 raycastingTrfc22;
    RaycastingTrfc23 raycastingTrfc23;
    RaycastingTrfc24 raycastingTrfc24;

    //Raycasting4way1 raycasting4way1; // 6/28 Trfc Ctrl, For Cube (81)
    //Raycasting4way2 raycasting4way2; // Cube (24)
    //Raycasting4way3 raycasting4way3; // Cube (44)
    //Raycasting4way4 raycasting4way4; // Cube (64)

    timeStamp4Way timeStamp4Way; // 6/30 Trfc Ctrl Test Codes
    TimeStampTrafficLight timeStampTrafficLight; // 7/06 Trfc Ctrl Test Code

    public float timeAt81; // 6/30 Trfc Ctrl
    public float timeAt24;
    public float timeAt44;
    public float timeAt64;

    public float steeringSensitivity = 0.01f;

    Vector3 target;
    int currentPoint = 0;
    Vector3 targetOdd; //two-way traffic test codes 4/10/22
    int currentPointOdd = 0;
    Vector3 targetTrfc; //traffic control test codes 5/01/22
    int currentPointTrfc = 0;

    public bool roundTrip = false;

    // Making the values accessible from Raycasting.cs
    //public float accel = 0.5f;
    //public float steer = 0f; // = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
    //Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
    //Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
    //public float brake = 0f;

    public float publicAccel = 0.25f;
    public float publicBrake = 0.05f;

    public int mphSpeedInt; // For Raycasting to be able to access
    public float targetAngleTrfc; 

    public float unStickDuration = 2f;
    public float unStickDurationTrfc = 1f;
    public float unStickTime = 0f; // Setting the time for unstuck code according to the unStickDuration length
                                   //float orgTargetAngle = 0f;

    public bool leftTurn = false; // 6/11 Trfc Ctrl
    public bool rightTurn = false;
    public bool straight = false;
    public bool crsTrfcTrunLeft = false; // 7/07 Trfc Ctrl

    public float accelTrfc; // 6/16 Trfc Ctrl
    public float brakeTrfc;

    public int current; // 6/22 Trfc Ctrl

    // Start is called before the first frame update
    void Start()
    {
        drivingControl = this.GetComponent<drivingControl>();
        raycasting = this.transform.GetComponentInChildren<Raycasting>(); //Accessing Raycasting script to control collisions

        raycastingTrfc = GameObject.Find("Cube (42)").GetComponent<RaycastingTrfc>(); // 6/24 Trfc Ctrl, For Cube (8)
        raycastingTrfc2 = GameObject.Find("Cube (10)").GetComponent<RaycastingTrfc2>();
        raycastingTrfc3 = GameObject.Find("Cube (34)").GetComponent<RaycastingTrfc3>();
        raycastingTrfc4 = GameObject.Find("Cube (9)").GetComponent<RaycastingTrfc4>();

        raycastingTrfc5 = GameObject.Find("Cube (14)").GetComponent<RaycastingTrfc5>(); // 6/25 Trfc Ctrl, For Cube (27)
        raycastingTrfc6 = GameObject.Find("Cube (31)").GetComponent<RaycastingTrfc6>();
        raycastingTrfc7 = GameObject.Find("Cube (73)").GetComponent<RaycastingTrfc7>();
        raycastingTrfc8 = GameObject.Find("Cube (29)").GetComponent<RaycastingTrfc8>();

        raycastingTrfc9 = GameObject.Find("Cube (19)").GetComponent<RaycastingTrfc9>(); // 6/26 Trfc Ctrl, For Cube (53)
        raycastingTrfc10 = GameObject.Find("Cube (48)").GetComponent<RaycastingTrfc10>();
        raycastingTrfc11 = GameObject.Find("Cube (62)").GetComponent<RaycastingTrfc11>();
        raycastingTrfc12 = GameObject.Find("Cube (74)").GetComponent<RaycastingTrfc12>();

        raycastingTrfc13 = GameObject.Find("Cube (78)").GetComponent<RaycastingTrfc13>(); // 6/26 Trfc Ctrl, For Cube (65)
        raycastingTrfc14 = GameObject.Find("Cube (66)").GetComponent<RaycastingTrfc14>();
        raycastingTrfc15 = GameObject.Find("Cube (58)").GetComponent<RaycastingTrfc15>();
        raycastingTrfc16 = GameObject.Find("Cube (79)").GetComponent<RaycastingTrfc16>();

        raycastingTrfc17 = GameObject.Find("Cube (70)").GetComponent<RaycastingTrfc17>(); // 6/26 Trfc Ctrl, For Cube (47)
        raycastingTrfc18 = GameObject.Find("Cube (71)").GetComponent<RaycastingTrfc18>();
        raycastingTrfc19 = GameObject.Find("Cube (28)").GetComponent<RaycastingTrfc19>();
        raycastingTrfc20 = GameObject.Find("Cube (54)").GetComponent<RaycastingTrfc20>();

        raycastingTrfc21 = GameObject.Find("Cube (72)").GetComponent<RaycastingTrfc21>(); // 6/26 Trfc Ctrl, For Cube (37)
        raycastingTrfc22 = GameObject.Find("Cube (38)").GetComponent<RaycastingTrfc22>();
        raycastingTrfc23 = GameObject.Find("Cube (4)").GetComponent<RaycastingTrfc23>();
        raycastingTrfc24 = GameObject.Find("Cube (43)").GetComponent<RaycastingTrfc24>();

        //raycasting4way1 = GameObject.Find("Cube (81)").GetComponent<Raycasting4way1>(); // 6/28 Trfc Ctrl, 4-way Stop
        //raycasting4way2 = GameObject.Find("Cube (24)").GetComponent<Raycasting4way2>();
        //raycasting4way3 = GameObject.Find("Cube (44)").GetComponent<Raycasting4way3>();
        //raycasting4way4 = GameObject.Find("Cube (64)").GetComponent<Raycasting4way4>();

        timeStamp4Way = GameObject.Find("4WayStop").GetComponent<timeStamp4Way>(); // 6/30 Trfc Ctrl Test Codes
        timeStampTrafficLight = GameObject.Find("TrafficLight").GetComponent<TimeStampTrafficLight>(); // 7/06 Trfc Ctrl 

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
            wayPointsTrfc.waypoints[34].gameObject.GetComponent<BoxCollider>().enabled = false; // 6/12 Trfc Ctrl, (34) messes the starting traffic, so disabling
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
                        Debug.Log(this.transform.gameObject.name + " Sharp Curve, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleOdd));
                        accel = 0.1f;
                        brake = 0.06f;
                        break;
                    case float f when Mathf.Abs(targetAngleOdd) > 25:
                        Debug.Log(this.transform.gameObject.name + " Curve, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleOdd));
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
                    accel = 0.5f;
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
                    if (raycasting.isHittingFrontHalf == true && raycasting.isHittingRear == false) // if any gameObject is blocking front half but no "Car" is directly behind
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
            //            Debug.Log("Sharp Curve, more than 50: " + nextTargetAngle);
            //            break;
            //        case float f when nextTargetAngle > 25:
            //            Debug.Log("Curve, more than 25: " + nextTargetAngle);
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
                        Debug.Log(this.transform.gameObject.name + " Sharp Curve, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
                        accel = 0.1f;
                        brake = 0.06f;
                        break;
                    case float f when Mathf.Abs(targetAngle) > 25:
                        Debug.Log(this.transform.gameObject.name + " Curve, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngle));
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
                    accel = 0.5f;
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
                    if (raycasting.isHittingFrontHalf == true && raycasting.isHittingRear == false) // if any gameObject is blocking front half but no "Car" is directly behind
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
            targetAngleTrfc = Mathf.Atan2(localTargetTrfc.x, localTargetTrfc.z) * Mathf.Rad2Deg; // 6/05 Trfc Ctrl, Accessing from Raycasting.cs
            float distanceToTargetTrfc = Vector3.Distance(targetTrfc, drivingControl.rb.gameObject.transform.position);

            //leftTurn = false; // 6/24 Trfc Ctrl
            //rightTurn = false;
            //straight = false;

            //timeAt81 = timeStamp4Way.timeAt81; // 6/30 Trfc Ctrl
            //timeAt24 = timeStamp4Way.timeAt24;
            //timeAt44 = timeStamp4Way.timeAt44;
            //timeAt64 = timeStamp4Way.timeAt64;

            // 6/17 TRFC CTRL TEST CODES
            float steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -2, 2);
            if (currentPointTrfc < 2 && roundTrip == false) // if at the initial starting point
            {
                accelTrfc = 0.3f;
                brakeTrfc = 0f;
            }
            else // if not at the initial starting point
            {
                if (raycasting.downRayText == "Untagged")
                {
                    accelTrfc = 0.275f;
                    brakeTrfc = 0f;
                }
                else // at where intersections are; 4Way Stop sign at 24 44 64 81
                {
                    accelTrfc = 0.25f;
                    brakeTrfc = 0f;
                }
            }

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
            // 6/18 Trfc Ctrl Test Code
            if (mphSpeedInt > 15)
            {
                switch (Mathf.Abs(targetAngleTrfc))
                {
                    case float f when Mathf.Abs(targetAngleTrfc) > 50:
                        //Debug.Log(this.transform.gameObject.name + " Sharp Curve, more than 50 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        accelTrfc = 0.1f;
                        brakeTrfc = 0.1f;
                        break;
                    case float f when Mathf.Abs(targetAngleTrfc) > 25:
                        //Debug.Log(this.transform.gameObject.name + " Curve, more than 25 Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        accelTrfc = 0.15f;
                        brakeTrfc = 0.1f;
                        break;
                    default:
                        //Debug.Log(this.transform.gameObject.name + " Straight, 25 or less Mathf.Abs(targetAngle): " + Mathf.Abs(targetAngleTrfc));
                        //Debug.Log(this.transform.gameObject.name + " drivingControl.currentSpeed: " + mphSpeedInt + " MPH"); //showing the current speed in MPH
                        switch (mphSpeedInt) // CONTROLLING ONGOING SPEED WHILE GOING STRAIGHT!
                        {
                            case int i when mphSpeedInt < 20:
                                //Debug.Log(this.transform.gameObject.name + " Bring it up => 20mph");
                                accelTrfc = 0.2f;
                                brakeTrfc = 0f;
                                break;
                            case int i when mphSpeedInt > 24:
                                //Debug.Log(this.transform.gameObject.name + " Keep it at below 25mph");
                                accelTrfc = 0.1f;
                                brakeTrfc = 0.2f;
                                break;
                            default:
                                // the current speed is between 20 and 25mph
                                break;
                        }
                        break;
                }
            }

            // 5/01/22 Trfc Ctrl, Not modified for Trfc Ctrl use; Finding out the next waypoint's y position to be at uphill or downhill
            //if (currentPointTrfc != 0) //avoiding subtracting -1 from currentPoint[0]
            //{
            //    float nextWaypointTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position.y;
            //    float currentWaypointTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].transform.position.y;
            //    float yDifference = nextWaypointTrfc - currentWaypointTrfc;
            //    switch (yDifference)
            //    {
            //        case float f when yDifference > 1.5:
            //            //Debug.Log(this.transform.gameObject.name + " the target on uphill: " + yDifference);
            //            if (mphSpeedInt < 40)
            //            {
            //                accelTrfc = 3f;
            //                brakeTrfc = 0f;
            //            }
            //            else
            //            {
            //                accelTrfc = 1f;
            //                brakeTrfc = 0F;
            //            }
            //            break;
            //        case float f when yDifference < -1.5:
            //            //Debug.Log(this.transform.gameObject.name + " the target on downhill: " + yDifference);
            //            if (mphSpeedInt < 60)
            //            {
            //                accelTrfc = 0.5f;
            //                brakeTrfc = 0f;
            //            }
            //            else
            //            {
            //                accelTrfc = 0.01f;
            //                brakeTrfc = 0.1F;
            //            }
            //            break;
            //        default:
            //            //Debug.Log(this.transform.gameObject.name + " the target on flat ground: " + yDifference);
            //            break;
            //    }
            //}

            /* For AI_Controller to grab
            aboutToHitAhead = false;
            aboutToHitLeftAhead = false;
            aboutToHitDirectlyAhead = false;
            aboutToHitRightAhead = false;
            //
            aboutToHitFarAhead = false; // 6/05 Traffic Control Test Codes
            aboutToHitFarDirectly = false; 
            aboutToHitFarLeftAhead = false;
            aboutToHitFarRightAhead = false;
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
            atThresholdTrfc = false; // For Traffic Control Codes
            downRayText = null; // 6/22 Trfc Ctrl Test Codes
            isHittingRight = false;
            //
            isHittingRearHalf = false;
            isHittingRightRear = false;
            isHittingRear = false;
            isHittingLeftRear = false;
            */
            //Testing Overtaking, targetAngle += to turn right, -= to turn left 
            //if (mphSpeedInt > 9 && Time.time > unStickTime) // if driving 10mph or faster & not being stuck
            if (mphSpeedInt >= 5 && Time.time > unStickTime) // 6/26 Trfc Ctrl, if driving 5mph or faster & not being stuck
            {
                //Debug.Log(this.transform.gameObject.name + " Speed > 5mph & Being Stuck == " + (unStickTime > Time.time));
                //Debug.Log(this.transform.gameObject.name + " Speed > 0mph & Not Being Stuck == " + (Time.time > unStickTime)); // 6/12 Trfc Ctrl

                // About to hit far ahead, 6/12 Trfc Ctrl
                if (raycasting.aboutToHitFarLeftAhead == true)
                {
                    //Debug.Log(this.transform.gameObject.name + " aboutToHitFarLeftAhead, TRFC " + targetAngleTrfc);
                    targetAngleTrfc += 30;
                    brakeTrfc = 4f;
                }
                if (raycasting.aboutToHitFarDirectly == true) // Avoiding frontal collisions with other cars
                {
                    //Debug.Log(this.transform.gameObject.name + " Avoiding Far Frontal Collisions, TRFC");
                    brakeTrfc = 5f;
                }
                if (raycasting.aboutToHitFarRightAhead == true)
                {
                    //Debug.Log(this.transform.gameObject.name + " aboutToHitFarRightAhead, TRFC " + targetAngleTrfc);
                    targetAngleTrfc -= 30;
                    brakeTrfc = 4f;
                }
                // About to hit ahead, 6/12 Trfc Ctrl
                if (raycasting.aboutToHitLeftAhead == true)
                {
                    //Debug.Log(this.transform.gameObject.name + " aboutToHitLeftAhead, targetAngle = " + targetAngleTrfc);
                    targetAngleTrfc += 45;
                    brakeTrfc = 5f;
                }
                if (raycasting.aboutToHitDirectlyAhead == true) // Avoiding frontal collisions with other cars
                {
                    //Debug.Log(this.transform.gameObject.name + " Avoiding Frontal Collisions!!!");
                    brakeTrfc = 6f;
                }
                if (raycasting.aboutToHitRightAhead == true)
                {
                    //Debug.Log(this.transform.gameObject.name + " aboutToHitRightAhead, targetAngle = " + targetAngleTrfc);
                    targetAngleTrfc -= 45;
                    brakeTrfc = 5f;
                }
                // About to get hit from rear
                if (raycasting.aboutToGetHitRightRear == true)
                {
                    targetAngleTrfc -= 30;
                    //Debug.Log(this.transform.gameObject.name + " aboutToGetHitRightRear, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.aboutToGetHitRear == true) // 6/14 Trfc Ctrl, Getting rear ended by another car
                {
                    //Debug.Log(this.transform.gameObject.name + " About To Get Rear-Ended!!!");
                    brakeTrfc = 0f;
                }
                if (raycasting.aboutToGetHitLeftRear == true)
                {
                    targetAngleTrfc += 30;
                    //Debug.Log(this.transform.gameObject.name + " aboutToGetHitLeftRear, targetAngle = " + targetAngleTrfc);
                }
                // Hitting the side
                if (raycasting.isHittingRightSide == true)
                {
                    targetAngleTrfc -= 10;
                    //Debug.Log(this.transform.gameObject.name + " isHittingRightSide, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingLeftSide == true)
                {
                    targetAngleTrfc += 10;
                    //Debug.Log(this.transform.gameObject.name + " isHittingLeftSide, targetAngle = " + targetAngleTrfc);
                }
                // A part of the front half is hitting
                if (raycasting.isHittingLeft == true)
                {
                    targetAngleTrfc += 60;
                    //Debug.Log(this.transform.gameObject.name + " isHittingLeft, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingRight == true)
                {
                    targetAngleTrfc -= 60;
                    //Debug.Log(this.transform.gameObject.name + " isHittingRight, targetAngle = " + targetAngleTrfc);
                }
                // A part of the rear half is hitting
                if (raycasting.isHittingRightRear == true)
                {
                    targetAngleTrfc -= 20;
                    //Debug.Log(this.transform.gameObject.name + " isHittingRightRear, targetAngle = " + targetAngleTrfc);
                }
                if (raycasting.isHittingLeftRear == true)
                {
                    targetAngleTrfc += 20;
                    //Debug.Log(this.transform.gameObject.name + " isHittingLeftRear, targetAngle = " + targetAngleTrfc);
                }

                steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -1, 1);
            }

            // The increment of a current waypoint (currentPointTrfc) occurs as the car approaches the current waypoint at currentPointTrfc < 3 distance.
            if (distanceToTargetTrfc < 3) // increase the value if gameObject circles around the waypoint.
            {
                currentPointTrfc++; // As soon as the car reaches a current waypoint at <3 distance, currentPointTrfc gets incremented to the next point

                if (currentPointTrfc == 2) // 6/16 Trfc Ctrl, at Cube (1)
                {
                    roundTrip = true;
                }

                if (this.transform.gameObject.name == "CarPurple (3)") // 7/02 Trfc Ctrl
                {
                    wayPointsTrfc.waypoints[34].gameObject.GetComponent<BoxCollider>().enabled = true; // (34) messes the traffic at the starting point.
                                                                                                       // Enabling it back after the traffic gets cleared.
                }

                IEnumerator Wait(int way) // 6/22 Trfc Ctrl Test Codes
                {
                    yield return new WaitForSecondsRealtime(1f); // 7/01 Trfc Ctrl
                    switch (way) // 6/24 Trfc Ctrl Test Codes
                    {
                        case (8): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc.nameOfPoint == "Cube (42)" && raycastingTrfc2.nameOfPoint == "Cube (10)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[34].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc.nameOfPoint == "Cube (42)" && raycastingTrfc2.nameOfPoint == "Cube (10)" && raycastingTrfc3.nameOfPoint == "Cube (34)" && raycastingTrfc4.nameOfPoint == "Cube (9)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " " + raycastingTrfc3.nameOfPoint + " " + raycastingTrfc4.nameOfPoint + " " + " the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " " + raycastingTrfc3.nameOfPoint + " " + raycastingTrfc4.nameOfPoint + " " + " the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (27): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc5.nameOfPoint == "Cube (14)" && raycastingTrfc6.nameOfPoint == "Cube (31)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc5.nameOfPoint + " " + raycastingTrfc6.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc5.nameOfPoint + " " + raycastingTrfc6.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[73].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc5.nameOfPoint == "Cube (14)" && raycastingTrfc6.nameOfPoint == "Cube (31)" && raycastingTrfc7.nameOfPoint == "Cube (73)" && raycastingTrfc8.nameOfPoint == "Cube (29)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc5.nameOfPoint + " " + raycastingTrfc6.nameOfPoint + " " + raycastingTrfc7.nameOfPoint + " " + raycastingTrfc8.nameOfPoint + " " + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc5.nameOfPoint + " " + raycastingTrfc6.nameOfPoint + " " + raycastingTrfc7.nameOfPoint + " " + raycastingTrfc8.nameOfPoint + " " + " : the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (53): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc9.nameOfPoint == "Cube (19)" && raycastingTrfc10.nameOfPoint == "Cube (48)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[62].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc9.nameOfPoint == "Cube (19)" && raycastingTrfc10.nameOfPoint == "Cube (48)" && raycastingTrfc11.nameOfPoint == "Cube (62)" && raycastingTrfc12.nameOfPoint == "Cube (74)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " " + raycastingTrfc11.nameOfPoint + " " + raycastingTrfc12.nameOfPoint + " " + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " " + raycastingTrfc11.nameOfPoint + " " + raycastingTrfc12.nameOfPoint + " " + " : the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (65): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc13.nameOfPoint == "Cube (78)" && raycastingTrfc14.nameOfPoint == "Cube (66)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[58].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc13.nameOfPoint == "Cube (78)" && raycastingTrfc14.nameOfPoint == "Cube (66)" && raycastingTrfc15.nameOfPoint == "Cube (58)" && raycastingTrfc16.nameOfPoint == "Cube (79)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " " + raycastingTrfc15.nameOfPoint + " " + raycastingTrfc16.nameOfPoint + " " + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " " + raycastingTrfc15.nameOfPoint + " " + raycastingTrfc16.nameOfPoint + " " + " : the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (47): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc17.nameOfPoint == "Cube (70)" && raycastingTrfc18.nameOfPoint == "Cube (71)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[28].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc17.nameOfPoint == "Cube (70)" && raycastingTrfc18.nameOfPoint == "Cube (71)" && raycastingTrfc19.nameOfPoint == "Cube (28)" && raycastingTrfc20.nameOfPoint == "Cube (54)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " " + raycastingTrfc19.nameOfPoint + " " + raycastingTrfc20.nameOfPoint + " " + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " " + raycastingTrfc19.nameOfPoint + " " + raycastingTrfc20.nameOfPoint + " " + " : the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (37): // T intersection
                            if (rightTurn == true)
                            {
                                if (raycastingTrfc21.nameOfPoint == "Cube (72)" && raycastingTrfc22.nameOfPoint == "Cube (38)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning right, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " : the course is NOT clear!");
                                    rightTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else if (leftTurn == true)
                            {
                                if (wayPointsTrfc.waypoints[4].gameObject.GetComponent<BoxCollider>().isTrigger == false)
                                {
                                    leftTurn = true;
                                    crsTrfcTrunLeft = true;
                                    StartCoroutine(Wait(way));
                                }
                                if (raycastingTrfc21.nameOfPoint == "Cube (72)" && raycastingTrfc22.nameOfPoint == "Cube (38)" && raycastingTrfc23.nameOfPoint == "Cube (4)" && raycastingTrfc24.nameOfPoint == "Cube (43)")
                                {
                                    if (crsTrfcTrunLeft == true)
                                    {
                                        yield return new WaitForSecondsRealtime(1f);
                                        crsTrfcTrunLeft = false;
                                    }
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " " + raycastingTrfc23.nameOfPoint + " " + raycastingTrfc24.nameOfPoint + " " + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " turning left, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " " + raycastingTrfc23.nameOfPoint + " " + raycastingTrfc24.nameOfPoint + " " + " : the course is NOT clear!");
                                    leftTurn = true;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                            }
                            break;
                        case (34): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc.nameOfPoint == "Cube (42)" && raycastingTrfc2.nameOfPoint == "Cube (10)")
                                {
                                    // yield return new WaitForSecondsRealtime(1f); 7/01 Trfc Ctrl Test Code
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc.nameOfPoint + " " + raycastingTrfc2.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 35;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                //straight = false;
                            }
                            break;
                        case (4): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc21.nameOfPoint == "Cube (72)" && raycastingTrfc22.nameOfPoint == "Cube (38)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc21.nameOfPoint + " " + raycastingTrfc22.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 5;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                straight = false;
                            }
                            break;
                        case (28): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc17.nameOfPoint == "Cube (70)" && raycastingTrfc18.nameOfPoint == "Cube (71)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc17.nameOfPoint + " " + raycastingTrfc18.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 63;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                //straight = false;
                            }
                            break;
                        case (58): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc13.nameOfPoint == "Cube (78)" && raycastingTrfc14.nameOfPoint == "Cube (66)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc13.nameOfPoint + " " + raycastingTrfc14.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 80;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                //straight = false;
                            }
                            break;
                        case (62): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc9.nameOfPoint == "Cube (19)" && raycastingTrfc10.nameOfPoint == "Cube (48)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc9.nameOfPoint + " " + raycastingTrfc10.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 20;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                //straight = false;
                            }
                            break;
                        case (73): // 6/26 Trfc Ctrl, Left turn yield
                            if (leftTurn == true)
                            {
                                if (raycastingTrfc6.nameOfPoint == "Cube (31)" && raycastingTrfc5.nameOfPoint == "Cube (14)")
                                {
                                    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                    //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // 7/04 Trfc Ctrl Testing
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc6.nameOfPoint + " " + raycastingTrfc5.nameOfPoint + " : the course is clear!");
                                }
                                else
                                {
                                    //Debug.Log("TrafficCtrl, At " + way + " left turn yielding, " + raycastingTrfc6.nameOfPoint + " " + raycastingTrfc5.nameOfPoint + " : the course is NOT clear!");
                                    leftTurn = true;
                                    currentPointTrfc = 15;
                                    StartCoroutine(Wait(way));
                                    //Debug.Log("TrafficCtrl, At " + way + " calling StartCoroutine(Wait(way)) again!");
                                }
                            }
                            else // straight
                            {
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("TrafficCtrl, At " + way + " going straight = " + straight);
                                straight = false;
                            }
                            break;
                        // 4-Way Stop Begins
                        /* IEnumerator Wait(int way) // 6/30 Trfc Ctrl Test Codes
                        {
                            yield return new WaitForSecondsRealtime(1f);
                            switch (way) */
                        case (81):
                            // 7/13 Trfc Ctrl Test Code
                            //float time2481 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt81;
                            //float time4481 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt81;
                            //float time6481 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt81;
                            //if ((time2481 > 0 /*&& time2481 < 4*/) || (time4481 > 0 /*&& time4481 < 4*/) || (time6481 > 0 /*&& time6481 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else if ((time2481 < 0 /*&& time2481 < 4*/) && (time4481 < 0 /*&& time4481 < 4*/) && (time6481 < 0 /*&& time6481 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/13 Trfc Ctrl Test Code Ends

                            // 7/12 Trfc Ctrl Test Code Begins, List
                            //timeStamp4Way.timeStamps = new List<float> { timeStamp4Way.timeAt81, timeStamp4Way.timeAt24, timeStamp4Way.timeAt44, timeStamp4Way.timeAt64 };
                            //timeStamp4Way.timeStamps.Sort();
                            //if (timeStamp4Way.timeAt81 == timeStamp4Way.timeStamps[0])
                            //{
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //    yield return new WaitForSecondsRealtime(6f);
                            //    timeStamp4Way.timeStamps.Remove(timeStamp4Way.timeAt81);
                            //    timeStamp4Way.timeAt81 += timeStamp4Way.timeAt81;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/12 Trfc Ctrl Test Code Ends

                            // 6/30 Trfc Ctrl Begins, 4 - way, raycasting4way3 not used
                            float time8124 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt24;
                            float time8144 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt44;
                            float time8164 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt64;
                            if ((time8124 > 0 && time8124 < 4) || (time8144 > 0 && time8144 < 4) || (time8164 > 0 && time8164 < 4))
                            {
                                //7 / 01 Trfc Ctrl Test Code
                                //Debug.Log("case (81): if < 4 " + time8124 + ", " + time8144 + ", " + time8164);
                                yield return new WaitForSecondsRealtime(5f);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("case (81): adding 6 to timeStamp4Way.timeAt24: " + timeStamp4Way.timeAt24);
                                timeStamp4Way.timeAt81 += 6; // Adding 1 + 5 for next traffic
                            }
                            else
                            {
                                //Debug.Log("case (81): else " + time8124 + ", " + time8144 + ", " + time8164);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            }

                            break;
                        case (24):
                            // 7/13 Trfc Ctrl Test Code
                            //float time8124 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt24;
                            //float time4424 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt24;
                            //float time6424 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt24;
                            //if ((time8124 > 0 /*&& time8124 < 4*/) || (time4424 > 0 /*&& time4424 < 4*/) || (time6424 > 0 /*&& time6424 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else if ((time8124 < 0 /*&& time8124 < 4*/) && (time4424 < 0 /*&& time4424 < 4*/) && (time6424 < 0 /*&& time6424 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/13 Trfc Ctrl Test Code Ends

                            // 7/12 Trfc Ctrl Test Code Begins, List
                            //timeStamp4Way.timeStamps = new List<float> { timeStamp4Way.timeAt81, timeStamp4Way.timeAt24, timeStamp4Way.timeAt44, timeStamp4Way.timeAt64 };
                            //timeStamp4Way.timeStamps.Sort();
                            //if (timeStamp4Way.timeAt24 == timeStamp4Way.timeStamps[0])
                            //{
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //    yield return new WaitForSecondsRealtime(6f);
                            //    timeStamp4Way.timeStamps.Remove(timeStamp4Way.timeAt24);
                            //    timeStamp4Way.timeAt24 += timeStamp4Way.timeAt24;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/12 Trfc Ctrl Test Code Ends

                            //6 / 30 Trfc Ctrl Begins, 4 - way, raycasting4way3 not used
                            float time2481 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt81;
                            float time2444 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt44;
                            float time2464 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt64;
                            if ((time2481 > 0 && time2481 < 4) || (time2444 > 0 && time2444 < 4) || (time2464 > 0 && time2464 < 4))
                            {
                                //7 / 01 Trfc Ctrl Test Code
                                //Debug.Log("case (24): if < 4 " + time2481 + ", " + time2444 + ", " + time2464);
                                yield return new WaitForSecondsRealtime(5f);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("case (24): adding 6 to timeStamp4Way.timeAt24: " + timeStamp4Way.timeAt24);
                                timeStamp4Way.timeAt24 += 6; // Adding 1 + 5 for next traffic
                            }
                            else
                            {
                                //Debug.Log("case (24): else " + time2481 + ", " + time2444 + ", " + time2464);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            }

                            break;
                        case (44):
                            // 7/13 Trfc Ctrl Test Code
                            //float time8144 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt44;
                            //float time2444 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt44;
                            //float time6444 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt44;
                            //if ((time8144 > 0 /*&& time8144 < 4*/) || (time2444 > 0 /*&& time2444 < 4*/) || (time6444 > 0 /*&& time6444 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else if ((time8144 < 0 /*&& time8144 < 4*/) && (time2444 < 0 /*&& time2444 < 4*/) && (time6444 < 0 /*&& time6444 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/13 Trfc Ctrl Test Code Ends

                            // 7/12 Trfc Ctrl Test Code Begins, List
                            //timeStamp4Way.timeStamps = new List<float> { timeStamp4Way.timeAt81, timeStamp4Way.timeAt24, timeStamp4Way.timeAt44, timeStamp4Way.timeAt64 };
                            //timeStamp4Way.timeStamps.Sort();
                            //if (timeStamp4Way.timeAt44 == timeStamp4Way.timeStamps[0])
                            //{
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //    yield return new WaitForSecondsRealtime(6f);
                            //    timeStamp4Way.timeStamps.Remove(timeStamp4Way.timeAt44);
                            //    timeStamp4Way.timeAt44 += timeStamp4Way.timeAt44;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/12 Trfc Ctrl Test Code Ends

                            //6 / 30 Trfc Ctrl Begins, 4 - way, raycasting4way3 not used
                            float time4481 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt81;
                            float time4424 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt24;
                            float time4464 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt64;
                            if ((time4481 > 0 && time4481 < 4) || (time4424 > 0 && time4424 < 4) || (time4464 > 0 && time4464 < 4))
                            {
                                //7 / 01 Trfc Ctrl Test Code
                                //Debug.Log("case (44): if < 4 " + time4481 + ", " + time4424 + ", " + time4464);
                                yield return new WaitForSecondsRealtime(5f);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("case (44): adding 6 to timeStamp4Way.timeAt44: " + timeStamp4Way.timeAt44);
                                timeStamp4Way.timeAt44 += 6; // Adding 1 + 5 for next traffic
                            }
                            else
                            {
                                //Debug.Log("case (44): else " + time4481 + ", " + time4424 + ", " + time4464);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            }

                            break;
                        case (64):
                            // 7/13 Trfc Ctrl Test Code
                            //float time8164 = timeStamp4Way.timeAt81 - timeStamp4Way.timeAt64;
                            //float time2464 = timeStamp4Way.timeAt24 - timeStamp4Way.timeAt64;
                            //float time4464 = timeStamp4Way.timeAt44 - timeStamp4Way.timeAt64;
                            //if ((time8164 > 0 /*&& time8164 < 4*/) || (time2464 > 0 /*&& time2464 < 4*/) || (time4464 > 0 /*&& time4464 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else if ((time8164 < 0 /*&& time8164 < 4*/) && (time2464 < 0 /*&& time2464 < 4*/) && (time4464 < 0 /*&& time4464 < 4*/))
                            //{
                            //    yield return new WaitForSecondsRealtime(2f);
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/13 Trfc Ctrl Test Code Ends

                            // 7/12 Trfc Ctrl Test Code Begins, List
                            //timeStamp4Way.timeStamps = new List<float> { timeStamp4Way.timeAt81, timeStamp4Way.timeAt24, timeStamp4Way.timeAt44, timeStamp4Way.timeAt64 };
                            //timeStamp4Way.timeStamps.Sort();
                            //if (timeStamp4Way.timeAt64 == timeStamp4Way.timeStamps[0])
                            //{
                            //    wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            //    yield return new WaitForSecondsRealtime(6f);
                            //    timeStamp4Way.timeStamps.Remove(timeStamp4Way.timeAt64);
                            //    timeStamp4Way.timeAt64 += timeStamp4Way.timeAt64;
                            //}
                            //else
                            //{
                            //    StartCoroutine(Wait(way));
                            //}
                            // 7/12 Trfc Ctrl Test Code Ends

                            //6 / 30 Trfc Ctrl Begins, 4 - way, raycasting4way3 not used
                            float time6481 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt81;
                            float time6424 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt24;
                            float time6444 = timeStamp4Way.timeAt64 - timeStamp4Way.timeAt44;
                            if ((time6481 > 0 && time6481 < 4) || (time6424 > 0 && time6424 < 4) || (time6444 > 0 && time6444 < 4))
                            {
                                //7 / 01 Trfc Ctrl Test Code
                                //Debug.Log("case (64): if < 4 " + time6481 + ", " + time6424 + ", " + time6444);
                                yield return new WaitForSecondsRealtime(5f);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                                //Debug.Log("case (64): adding 6 to timeStamp4Way.timeAt64: " + timeStamp4Way.timeAt64);
                                timeStamp4Way.timeAt64 += 6; // Adding 1 + 5 for next traffic
                            }
                            else
                            {
                                //Debug.Log("case (64): else " + time6481 + ", " + time6424 + ", " + time6444);
                                wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            }

                            break;
                        // 4-Way Stop Ends

                        // Lights Begins
                            

                        default:
                            leftTurn = false;
                            rightTurn = false;
                            straight = false;
                            wayPointsTrfc.waypoints[way].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                            break;
                    }
                }

                //void TrafficCtrl(int way) // 6/24 Trfc Ctrl Test Codes
                //{

                //}

                if (currentPointTrfc == 5 && raycasting.atThresholdTrfc == true) // 6/27 Trfc Ctrl Left turn yield, at Cube (4), 6/14 WORKED WITHOUT atThresholdTrfc
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/26 Trfc Ctrl, Left turn yield
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 43;
                        //straight = true;
                    }
                    else // Left turn yield
                    {
                        if (raycastingTrfc21.nameOfPoint == "Cube (72)" && raycastingTrfc22.nameOfPoint == "Cube (38)")
                        {
                            currentPointTrfc = 5;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            leftTurn = true;
                            currentPointTrfc = 5;
                            StartCoroutine(Wait(current));
                        }
                    }
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 44 && raycasting.atThresholdTrfc == true) // 6/14 Trfc Ctrl, at Cube (43)
                {
                    currentPointTrfc = 28;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 7 && raycasting.atThresholdTrfc == true) // At Cube (6), Lights, Box Collider & Is Trigger MUST BE CHECKED!
                {
                    timeStampTrafficLight.timeAt6 = Time.time; // 7/06 Trfc Ctrl
                    //Debug.Log(this.gameObject.name + "'s Time at Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);

                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 23;
                    }
                    else if (randomNumber == 2) // straight
                    {
                        currentPointTrfc = 25;
                    }
                    else // left turn
                    {
                        currentPointTrfc = 84;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (current == 6 && currentPointTrfc == 85 && raycasting.atThresholdTrfc == true) // at Cube (84), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 7;
                    raycasting.atThresholdTrfc = false;
                }

                // A SOLUTION I CAME UP BEFORE IMPLEMENTING raycasting.atThresholdTrfc BEGINS
                //if (currentPointTrfc == 9 && raycasting.atThresholdTrfc == true) // if/when the car has reached at the Cube (8), NOT at Cube (9),
                //{
                //    int randomNumber = Random.Range(1, 3);
                //    if (randomNumber == 1) // left turn
                //    {
                //        currentPointTrfc = 9; // setting the next waypoint to the Cube (9), which is a left turn
                //        leftTurn = true;
                //        //Debug.Log("TRFC, " + this.transform.gameObject.name + "'s leftTurn = " + leftTurn + " and currentPointTrfc is " + currentPointTrfc);
                //    }
                //    else // right turn
                //    {
                //        currentPointTrfc = 10;
                //    }
                //}
                //if (leftTurn == true && currentPointTrfc == 9) // if the car has made a left turn at Cube (8) and the next waypoint has been set to the Cube (9),
                //{
                //    currentPointTrfc = 10; // Wait until the car reaches the Cube (9). At that point, setting currentPointTrfc to (10).
                //}
                //if (leftTurn == true && currentPointTrfc == 10) // if the car has made a left turn and has reached the Cube (9),
                //{
                //    currentPointTrfc = 0; // Only then can you set the currentPointTrfc to (0)!
                //                          // In actuality, the car goes directly from Cube (8) to (0), so Cube (0) and (9) are placed next to each other in the scene to workaround!
                //}
                // A SOLUTION I CAME UP BEFORE IMPLEMENTING raycasting.atThresholdTrfc ENDS

                if (currentPointTrfc == 9 && raycasting.atThresholdTrfc == true) // at Cube (8), T intersection
                {
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // left turn
                    {
                        currentPointTrfc = 9; // setting the next waypoint to the Cube (9), which is a left turn
                        leftTurn = true;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 10;
                        rightTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    //TrafficCtrl(current); // 6/24 Trfc Ctrl Test Codes
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 10 && raycasting.atThresholdTrfc == true) // at Cube (9), 6/22 Trfc Ctrl
                {
                    currentPointTrfc = 1;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 15) // just arrived at Cube (14), WORKS WITHOUT raycasting.atThresholdTrfc
                {
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 19;
                        //straight = true;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 15;
                        //rightTurn = true;
                    }
                }

                if (currentPointTrfc == 19 && raycasting.atThresholdTrfc == true) // Arrived at Cube (18) Lights, atThresholdTrfc Threshold logic FIXES/WORKS!
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    timeStampTrafficLight.timeAt18 = Time.time; // 7/06 Trfc Ctrl
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
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
                        currentPointTrfc = 84;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (current == 18 && currentPointTrfc == 85 && raycasting.atThresholdTrfc == true) // at Cube (84), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 23;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 25 && raycasting.atThresholdTrfc == true) // at Cube (24), 4-way Stop
                {
                    timeStamp4Way.timeAt24 = Time.time;
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // turning right
                    {
                        currentPointTrfc = 50;
                    }
                    else if (randomNumber == 2) // straight
                    {
                        currentPointTrfc = 49;
                    }
                    else // turning left
                    {
                        currentPointTrfc = 83;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false;
                }

                if (current == 24 && currentPointTrfc == 84 && raycasting.atThresholdTrfc == true) // at Cube (83), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 51;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 28 && raycasting.atThresholdTrfc == true) // just arrived at Cube (27), T intersection
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // left
                    {
                        currentPointTrfc = 29;
                        leftTurn = true;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 31;
                        rightTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; 
                }

                if (currentPointTrfc == 29 && raycasting.atThresholdTrfc == true) // at Cube (28), 6/27 Trfc Ctrl, Left turn yield
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/26 Trfc Ctrl, Left turn yield
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 54;
                        //straight = true;
                    }
                    else // left turn
                    {
                        if (raycastingTrfc17.nameOfPoint == "Cube (70)" && raycastingTrfc18.nameOfPoint == "Cube (71)")
                        {
                            currentPointTrfc = 63;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            currentPointTrfc = 63;
                            leftTurn = true;
                            StartCoroutine(Wait(current));
                        }
                    }
                    raycasting.atThresholdTrfc = false; 
                }

                if (currentPointTrfc == 32 && raycasting.atThresholdTrfc == true) // at Cube (31)
                {
                    currentPointTrfc = 19;
                    raycasting.atThresholdTrfc = false; 
                }

                if (currentPointTrfc == 20 && raycasting.atThresholdTrfc == true) // at Cube (19)
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 48;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 20;
                    }
                    raycasting.atThresholdTrfc = false; 
                }

                if (currentPointTrfc == 22 && raycasting.atThresholdTrfc == true) // at Cube (21)
                {
                    currentPointTrfc = 85;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 86 && raycasting.atThresholdTrfc == true) // at Cube (85) // 6/26 Trfc Ctrl
                {
                    currentPointTrfc = 44;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 45 && raycasting.atThresholdTrfc == true) // at Cube (44), 4-way Stop
                {
                    timeStamp4Way.timeAt44 = Time.time;
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 47;
                    }
                    else if (randomNumber == 2) // right turn
                    {
                        currentPointTrfc = 45;
                    }
                    else
                    {
                        currentPointTrfc = 83; // left turn
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false;
                }

                if (current == 44 && currentPointTrfc == 84 && raycasting.atThresholdTrfc == true) // at Cube (83), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 49;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 47 && raycasting.atThresholdTrfc == true) // at Cube (46) Lights
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    timeStampTrafficLight.timeAt46 = Time.time; // 7/06 Trfc Ctrl
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 7;
                    }
                    else if (randomNumber == 2) // right turn
                    {
                        currentPointTrfc = 25;
                    }
                    else
                    {
                        currentPointTrfc = 84; // left turn
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false;
                }

                if (current == 46 && currentPointTrfc == 85 && raycasting.atThresholdTrfc == true) // at Cube (84), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 22;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 31 && raycasting.atThresholdTrfc == true) // reached Cube (30)
                {
                    currentPointTrfc = 32;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 35 && raycasting.atThresholdTrfc == true) // at Cube (34), 6/27 Trfc Ctrl, Left turn yield
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/26 Trfc Ctrl, Left turn yield
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 0;
                        //straight = true;
                    }
                    else // left turn
                    {
                        if (raycastingTrfc.nameOfPoint == "Cube (42)" && raycastingTrfc2.nameOfPoint == "Cube (10)")
                        {
                            currentPointTrfc = 35;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            currentPointTrfc = 35;
                            leftTurn = true;
                            StartCoroutine(Wait(current));
                        }
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 37 && raycasting.atThresholdTrfc == true) // at Cube (36) Lights
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    timeStampTrafficLight.timeAt36 = Time.time; // 7/06 Trfc Ctrl
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 23;
                    }
                    else if (randomNumber == 2) // right turn
                    {
                        currentPointTrfc = 22;
                    }
                    else
                    {
                        currentPointTrfc = 84;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (current == 36 && currentPointTrfc == 85 && raycasting.atThresholdTrfc == true) // at Cube (84), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 25;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 23 && raycasting.atThresholdTrfc == true) // when at Cube (22)
                {
                    currentPointTrfc = 37;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 38 && raycasting.atThresholdTrfc == true) // at Cube (37), T intersection
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 38;
                        rightTurn = true;
                    }
                    else // left turn
                    {
                        currentPointTrfc = 43;
                        leftTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 43 && raycasting.atThresholdTrfc == true) // at Cube (42) 
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 10;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 35;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 44 && raycasting.atThresholdTrfc == true) // when at Cube (43)
                {
                    currentPointTrfc = 28;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 48 && raycasting.atThresholdTrfc == true) // at Cube (47) 
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 71;
                        rightTurn = true;
                    }
                    else // left turn
                    {
                        currentPointTrfc = 54;
                        leftTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 54 && raycasting.atThresholdTrfc == true) // at Cube (53), T intersection
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 48;
                        rightTurn = true;
                    }
                    else // left turn
                    {
                        currentPointTrfc = 74;
                        leftTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 49 && raycasting.atThresholdTrfc == true) // when at Cube (48)
                {
                    currentPointTrfc = 75;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 65 && raycasting.atThresholdTrfc == true) // at Cube (64), 4-way Stop
                {
                    timeStamp4Way.timeAt64 = Time.time;
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 51;
                    }
                    else if (randomNumber == 2) // right turn
                    {
                        currentPointTrfc = 49;
                    }
                    else
                    {
                        currentPointTrfc = 83; // left turn
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false;
                }

                if (current == 64 && currentPointTrfc == 84 && raycasting.atThresholdTrfc == true) // at Cube (83), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 45;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 66 && raycasting.atThresholdTrfc == true) // at Cube (65), T intersection
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 66;
                        rightTurn = true;
                    }
                    else // left turn
                    {
                        currentPointTrfc = 79;
                        leftTurn = true;
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 50 && raycasting.atThresholdTrfc == true) // when at Cube (49)
                {
                    currentPointTrfc = 65;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 51 && raycasting.atThresholdTrfc == true) // when at Cube (50)
                {
                    currentPointTrfc = 47;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 59 && raycasting.atThresholdTrfc == true) // at Cube (58), 6/27 Trfc Ctrl, Left turn yield
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/26 Trfc Ctrl, Left turn yield
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // left turn
                    {
                        if (raycastingTrfc13.nameOfPoint == "Cube (78)" && raycastingTrfc14.nameOfPoint == "Cube (66)")
                        {
                            currentPointTrfc = 80;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            currentPointTrfc = 80;
                            leftTurn = true;
                            StartCoroutine(Wait(current));
                        }  
                    }
                    else // straight
                    {
                        currentPointTrfc = 79;
                        //straight = true;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 63 && raycasting.atThresholdTrfc == true) // at Cube (62), 6/27 Trfc Ctrl, Left turn yield
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/26 Trfc Ctrl, Left turn yield
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // left turn
                    {
                        if (raycastingTrfc9.nameOfPoint == "Cube (19)" && raycastingTrfc10.nameOfPoint == "Cube (48)")
                        {
                            currentPointTrfc = 20;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            currentPointTrfc = 20;
                            leftTurn = true;
                            StartCoroutine(Wait(current));
                        }
                    }
                    else // straight
                    {
                        currentPointTrfc = 73;
                        //straight = true;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 71 && raycasting.atThresholdTrfc == true) // at Cube (70) 
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // right turn
                    {
                        currentPointTrfc = 63;
                    }
                    else // straight
                    {
                        currentPointTrfc = 71;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 73 && raycasting.atThresholdTrfc == true) // at Cube (72) 
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 39;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 5;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 74 && raycasting.atThresholdTrfc == true) // at Cube (73), 6/27 Left turn yield
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1;
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 29;
                        //straight = true;
                    }
                    else // left turn
                    {
                        if (raycastingTrfc6.nameOfPoint == "Cube (31)" && raycastingTrfc5.nameOfPoint == "Cube (14)")
                        {
                            currentPointTrfc = 15;
                        }
                        else
                        {
                            wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false;
                            currentPointTrfc = 15;
                            leftTurn = true;
                            StartCoroutine(Wait(current));
                        }
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 79 && raycasting.atThresholdTrfc == true) // at Cube (78) 
                {
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    int randomNumber = Random.Range(1, 3);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 66;
                    }
                    else // right turn
                    {
                        currentPointTrfc = 80;
                    }
                    raycasting.atThresholdTrfc = false; // Set it back to false immediately!
                }

                if (currentPointTrfc == 82 && raycasting.atThresholdTrfc == true) // at Cube (81), 4-way Stop
                {
                    timeStamp4Way.timeAt81 = Time.time;
                    // fourWayBool = raycasting.downRayText;
                    //Debug.Log(this.gameObject.name + "'s Time at Threshold Cube (" + (currentPointTrfc - 1) + ") is " + Time.time);
                    current = currentPointTrfc - 1; // 6/22 Trfc Ctrl Test Codes
                    int randomNumber = Random.Range(1, 4);
                    if (randomNumber == 1) // straight
                    {
                        currentPointTrfc = 45;
                    }
                    else if (randomNumber == 2) // right turn
                    {
                        currentPointTrfc = 51;
                    }
                    else
                    {
                        currentPointTrfc = 83; // left turn
                    }
                    wayPointsTrfc.waypoints[current].gameObject.GetComponent<BoxCollider>().isTrigger = false; // 6/22 Trfc Ctrl Test Codes
                    StartCoroutine(Wait(current));
                    raycasting.atThresholdTrfc = false;
                }

                if (current == 81 && currentPointTrfc == 84 && raycasting.atThresholdTrfc == true) // at Cube (83), 6/22 Trfc Ctrl Test Codes
                {
                    currentPointTrfc = 50;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 75 && raycasting.atThresholdTrfc == true) // when at Cube (74)
                {
                    currentPointTrfc = 73;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 79 && raycasting.atThresholdTrfc == true) // when at Cube (78)
                {
                    currentPointTrfc = 66;
                    raycasting.atThresholdTrfc = false;
                }

                if (currentPointTrfc == 80 && raycasting.atThresholdTrfc == true) // when at Cube (79)
                {
                    currentPointTrfc = 59;
                    raycasting.atThresholdTrfc = false;
                }

                // 7/04 Trfc Ctrl Test Codes
                if (currentPointTrfc >=0 || currentPointTrfc <= 86)
                {
                    targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;
                    nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name;
                }
                else
                {
                    targetTrfc = wayPointsTrfc.waypoints[0].transform.position;
                    nextWaypointNameTrfc = wayPointsTrfc.waypoints[0].name;
                }

                // 7/04 Trfc Ctrl Test, disabling the followings for the test above 
                //targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position; // Finally, the target angle is processed accordingly. 
                ////Debug.Log(this.transform.gameObject.name + "'s next waypoint is " + wayPointsTrfc.waypoints[currentPointTrfc].name);
                //nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name; // For debug purpose only
            }

            // Getting Cars Unstuck, 6/18 Trfc Ctrl Test Code
            if (mphSpeedInt < 1 /*&& raycasting.hitFrontHalfDownRayText == "Untagged"*/) // hardly moving at 1 mph or less /*and not at intersections*/
            {
                if (!(currentPointTrfc == 0 && roundTrip == false)) // The car is not at the starting point of the race
                {
                    if ((raycasting.isHittingLeft == true || raycasting.isHittingRight) && raycasting.isHittingRear == false) // if any "Car" is blocking the right or left but no "Car" is directly behind
                    {
                        unStickTime = Time.time + unStickDurationTrfc; // Setting the time to do unstuck codes for unStickDuration
                        if (Time.time < unStickTime) // if unStickTime hasn't reached Time.time yet, drive backward toward -2 previous waypoint at minus 800 wheelColliders[i].motorTorque
                        {
                            if (currentPointTrfc > 1)
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 2].transform.position;
                                //Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                //Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 2) " + wayPointsTrfc.waypoints[currentPointTrfc - 2].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 2].name; // For debug purpose only
                            }
                            else if (currentPoint == 1)
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].transform.position;
                                //Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                //Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 1) " + wayPointsTrfc.waypoints[currentPointTrfc - 1].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc - 1].name; // For debug purpose only
                            }
                            else
                            {
                                targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;
                                //Debug.Log(this.transform.gameObject.name + " Getting Stuck == " + (Time.time < unStickTime));
                                //Debug.Log(this.transform.gameObject.name + " driving back toward (currentPoint - 0) " + wayPointsTrfc.waypoints[currentPointTrfc].name);
                                nextWaypointNameTrfc = wayPointsTrfc.waypoints[currentPointTrfc].name; // For debug purpose only
                            }
                            localTargetTrfc = drivingControl.rb.gameObject.transform.InverseTransformPoint(targetTrfc);
                            targetAngleTrfc = Mathf.Atan2(localTargetTrfc.x, localTargetTrfc.z) * Mathf.Rad2Deg;
                            steer = Mathf.Clamp(targetAngleTrfc * steeringSensitivity, -1, 1); // (targetAngle * -1) to try directing towards the previous waypoint while driving backward, SAME RESULT AS * +1
                            brakeTrfc = 0f;
                            accelTrfc = -4f;
                            //Debug.Log(this.transform.gameObject.name + " drivingControl.Go Backward(" + accelTrfc + ", " + steer + ", " + brakeTrfc + ")");
                            drivingControl.Go(accelTrfc, steer, brakeTrfc);
                        }
                        targetTrfc = wayPointsTrfc.waypoints[currentPointTrfc].transform.position;

                        //ANY OF THE FOLLOWING LINES PLACING HERE WILL MESS YOU UP!!!
                        //localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                        //targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                        //steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
                        //accelTrfc = 3f; // drive forward at 600 wheelColliders[i].motorTorque after driving backward
                        //brake = 0f;
                        //ANY OF THE ABOVE LINES PLACING HERE WILL MESS YOU UP!!!
                    }
                }
            }
            //Debug.Log(this.transform.gameObject.name + " drivingControl.Go(" + accelTrfc + ", " + steer + ", " + brakeTrfc + ")"); // 7/01 Trfc Ctrl
            drivingControl.Go(accelTrfc, steer, brakeTrfc); //running Go regardless of distanceToTarget here
            drivingControl.CheckSkidding();
            drivingControl.calculateEngineSound();
        } //***TRAFFIC CONTROL ELSE IF{} TEST CODES END 5/01/22***
    }
}
