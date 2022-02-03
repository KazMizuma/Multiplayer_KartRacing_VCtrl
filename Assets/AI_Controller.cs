using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Controller : MonoBehaviour
{
    public wayPoints wayPoints;
    drivingControl drivingControl;
    Raycasting raycasting; //Accessing Raycasting script to control collisions
    public float steeringSensitivity = 0.01f;
    Vector3 target;
    int currentPoint = 0;
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

    float unStickDuration = 5f;
    float unStickTime = 0f; // Setting the time for unstuck code according to the unStickDuration length
    //float orgTargetAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        drivingControl = this.GetComponent<drivingControl>();
        raycasting = this.transform.GetComponentInChildren<Raycasting>(); //Accessing Raycasting script to control collisions
        target = wayPoints.waypoints[currentPoint].transform.position;
    }

    // Update is called once per frame
    void Update()
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

        Debug.Log("next waypoint is " + wayPoints.waypoints[currentPoint].name);
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
                        case int i when mphSpeedInt < 61:
                            Debug.Log("Straight line, bring it up to 60mph!");
                            accel = 0.8f;
                            brake = 0f;
                            break;
                        case int i when mphSpeedInt > 79:
                            Debug.Log("Bring it down below 80mph!");
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
                    Debug.Log("the target on uphill: " + yDifference);
                    accel = 1.75f;
                    brake = 0f;
                    //if (mphSpeedInt < 20) // It does not seem to be detected as getting stuck on uphill
                    //{
                    //    Debug.Log("Getting stuck on uphill: " + accel);
                    //    accel = 3f; 
                    //}
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
        if (mphSpeedInt > 34)
        {
            // About to hit ahead
            if (raycasting.aboutToHitLeftAhead == true)
            {
                targetAngle += 30;
                Debug.Log("aboutToHitLeftAhead, targetAngle = " + targetAngle);
            }
            if (raycasting.aboutToHitDirectlyAhead == true) // Avoiding frontal collisions with other cars
            {
                Debug.Log("Avoiding Frontal Collisions!!!");
                accel = 0.4f;
                brake = 0.01f;
            }
            if (raycasting.aboutToHitRightAhead == true)
            {
                targetAngle -= 30;
                Debug.Log("aboutToHitRightAhead, targetAngle = " + targetAngle);
            }
            // About to get hit from rear
            if (raycasting.aboutToGetHitRightRear == true)
            {
                targetAngle -= 30;
                Debug.Log("aboutToGetHitRightRear, targetAngle = " + targetAngle);
            }
            if (raycasting.aboutToGetHitRear == true) // Avoiding getting rear ended with another car
            {
                Debug.Log("Avoiding Rear-End!!!");
                accel = 0.6f;
                brake = 0f;
            }
            if (raycasting.aboutToGetHitLeftRear == true)
            {
                targetAngle += 30;
                Debug.Log("aboutToGetHitLeftRear, targetAngle = " + targetAngle);
            }
            // Hitting the side
            if (raycasting.isHittingRightSide == true)
            {
                targetAngle -= 10;
                Debug.Log("isHittingRightSide, targetAngle = " + targetAngle);
            }
            if (raycasting.isHittingLeftSide == true)
            {
                targetAngle += 10;
                Debug.Log("isHittingLeftSide, targetAngle = " + targetAngle);
            }
            // A part of the front half is hitting
            if (raycasting.isHittingLeft == true)
            {
                targetAngle += 15;
                Debug.Log("isHittingLeft, targetAngle = " + targetAngle);
            }
            if (raycasting.isHittingRight == true)
            {
                targetAngle -= 15;
                Debug.Log("isHittingRight, targetAngle = " + targetAngle);
            }
            // A part of the rear half is hitting
            if (raycasting.isHittingRightRear == true)
            {
                targetAngle -= 20;
                Debug.Log("isHittingRightRear, targetAngle = " + targetAngle);
            }
            if (raycasting.isHittingLeftRear == true)
            {
                targetAngle += 20;
                Debug.Log("isHittingLeftRear, targetAngle = " + targetAngle);
            }
            steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1);
        }

        //Commenting out to bring the following above
        ////float accel = 0.5f; //original value
        //float accel = 0.5f;
        //float steer = Mathf.Clamp(targetAngle * steeringSensitivity, -1, 1) * Mathf.Sign(drivingControl.currentSpeed);
        ////Debug.Log("targetAngle & steeringSensitivity: " + targetAngle + " & " + steeringSensitivity);
        ////Debug.Log(" drivingControl.currentSpeed " + drivingControl.currentSpeed + " Sign of it = " + Mathf.Sign(drivingControl.currentSpeed));
        //float brake = 0;

        if (distanceToTarget < 7) // Increased from 5 to be lenient for multiple cars reaching the waypoints simultaneously
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
                Debug.Log("Making a round trip!");
                //COMMENTING OUT TO TRY OUT MY AI CODE!
                //accel = publicAccel;
                //brake = publicBrake;
                //Debug.Log("publicAccel & publicBrake: " + publicAccel + "; " + publicBrake);
            }
        }

        if (distanceToTarget < 5) /* increase the value if gameObject circles around waypoint. 
                                   * Increased from 2 to be lenient for multiple cars reaching the waypoints simultaneously */
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
                    if (Time.time < unStickTime ) // if the time hasn't reached
                    {
                        if (currentPoint != 0)
                        {
                            target = wayPoints.waypoints[currentPoint - 1].transform.position;
                        }
                        else if (currentPoint == 0)
                        {
                            target = wayPoints.waypoints[currentPoint].transform.position;
                        }
                        localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                        targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                        steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2); // Only for Getting Cars Unstuck am I using -2, 2 range
                        brake = 0f;
                        accel = -4f; // Drive backward at minus 800 wheelColliders[i].motorTorque
                        Debug.Log("drivingControl.Go Backward(" + accel + ", " + steer + ", " + brake + ") " + "Time: " + unStickTime);
                        drivingControl.Go(accel, steer, brake);
                    }
                    target = wayPoints.waypoints[currentPoint].transform.position;
                    /* THE FOLLOWING LINES WILL MESS YOU UP!!!
                    localTarget = drivingControl.rb.gameObject.transform.InverseTransformPoint(target);
                    targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
                    steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2);
                    brake = 0f;
                    accel = 3f; // move backward at minus 600 wheelColliders[i].motorTorque
                    Debug.Log("drivingControl.Go Forward(" + accel + ", " + steer + ", " + brake + ") "); */
                }

                // Driving forward with extreme targetAngle after getting stuck & backing up 
                // unStickTime += unStickDuration; // Adding additional time duration for driving forward task
                // if (Time.time < unStickTime /* && (raycasting.aboutToHitAhead == false || raycasting.isHittingFrontHalf == false) */ )
                // // if unStickTime has not reached and no gameObject is at the front, drive forward!
                // {
                //     targetAngle = orgTargetAngle;
                //     //Debug.Log("aboutToHit or Hitting Ahead == true, targetAngle = " + targetAngle);
                //     //if (targetAngle < 0) // targetAngle slightly on my left
                //     //{
                //     //    targetAngle -= 150f;
                //     //}
                //     //else
                //     //{
                //     //    targetAngle += 150f; // assuming targetAngle slightly on my right
                //     //}
                //     accel = 3f; // move forward at 600 wheelColliders[i].motorTorque
                //     steer = Mathf.Clamp(targetAngle * (steeringSensitivity * 2f), -2, 2); // changing values just for getting cars unstuck
                //     brake = 0f;
                //     Debug.Log("drivingControl.Go Forward(" + accel + ", " + steer + ", " + brake + ")" + "Time: " + unStickTime);
                //     drivingControl.Go(accel, steer, brake);
                // }
            }
        }
        Debug.Log("drivingControl.Go(" + accel + ", " + steer + ", " + brake + ")");
        drivingControl.Go(accel, steer, brake); //running Go regardless of distanceToTarget here
        drivingControl.CheckSkidding();
        drivingControl.calculateEngineSound();
    }
}
