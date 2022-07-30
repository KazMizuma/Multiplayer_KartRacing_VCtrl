using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to the rigid body of the car, not to the parent
public class Raycasting : MonoBehaviour
{
    //public float targetVelocity = 10.0f; // World of Zero's, I'm not using it
    public int numberOfRays = 17;
    public float angle = 30.0f;
    public GameObject frontRotation; // Rays for front half
    public GameObject rearRotation; // Rays for rear half 

    //public Quaternion rotationAngleAxTrfc; // 7/21 Trfc Ctrl TEST

    // Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
    public static bool racingRaycasting;

    // For AI_Controller to grab
    public bool aboutToHitAhead = false;
    public bool aboutToHitLeftAhead = false; 
    public bool aboutToHitDirectlyAhead = false;
    public bool aboutToHitRightAhead = false;
    //
    public bool aboutToHitFarAhead = false; // 6/05 Traffic Control Test Codes
    public bool aboutToHitFarDirectly = false; 
    public bool aboutToHitFarLeftAhead = false;
    public bool aboutToHitFarRightAhead = false;
    //
    public bool aboutToGetHitRightRear = false;
    public bool aboutToGetHitRear = false;
    public bool aboutToGetHitLeftRear = false;
    //
    public bool isHittingRightSide = false; // 6/23 Trfc Ctrl
    public bool isHittingLeftSide = false;
    //
    public bool isHittingFrontHalf = false;
    public bool isHittingLeft = false;
    public bool isHittingFront = false;
    public bool atThresholdTrfc = false; // 5/13 Traffic Control Test Codes
    public string downRayText; // 5/30 Traffic Control Test Codes, casting rays downward, debug purpose only
    public bool isHittingRight = false;
    //
    public bool isHittingRearHalf = false;
    public bool isHittingRightRear = false;
    public bool isHittingRear = false;
    public bool isHittingLeftRear = false;

    AI_Controller ai_controller; // To access AI_Controller, 6/05 Trfc Ctrl

    //public Vector3 deltaPosition; // Trying World of Zero's avoidance method

    // Start is called before the first frame update
    void Start()
    {
        // 7/30 Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
        racingRaycasting = false;

        ai_controller = this.transform.parent.GetComponent<AI_Controller>(); // To access AI_Controller, 6/05 Trfc Ctrl
    }

    // Update is called once per frame
    void Update()
    {
        ////My Raycasting Starts//
        ////In the Inspector, CarParent (transform.gameObject.name) has no Collider and the tag is "Untagged".
        ////CarBodyColor, a child of CarParent, has Collider, tagged "Car" (hit.transform.gameObject.tag) and
        ////has this Raycasting script attached.
        //RaycastHit hit;
        //
        //Ray hittingRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 4f, Color.green);
        //
        //Ray gettingHitRay = new Ray(gameObject.transform.position, -gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, -gameObject.transform.forward * 2.5f, Color.white);
        //
        //if (Physics.Raycast(hittingRay, out hit, 4f))
        //{
        //    if (hit.transform.gameObject.tag == "Car")
        //    {
        //        Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + "!!!");
        //    }
        //}
        //
        //if (Physics.Raycast(gettingHitRay, out hit, 2.5f))
        //{
        //    if (hit.transform.gameObject.tag == "Car")
        //    {
        //        Debug.Log(transform.gameObject.name + " GETTING HIT BY " + hit.transform.gameObject.name + "!!!");
        //    }
        //}
        ////My Raycasting Ends//

        // Using the method of fanning rays from World of Zero's Raycasting and Object Avoidance Algorithm  https://youtu.be/SVazwHyfB7g?t=944

        RaycastHit hit; // Combining my code
        //Ray hittingRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 4f, Color.green);
        //Ray gettingHitRay = new Ray(gameObject.transform.position, -gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, -gameObject.transform.forward * 2.5f, Color.white);

        // For AI_Controller to grab
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
        isHittingRightSide = false; // 6/23 Trfc Ctrl
        isHittingLeftSide = false;
        //
        isHittingFrontHalf = false;
        isHittingLeft = false;
        isHittingFront = false;
        atThresholdTrfc = false; // 5/13 Traffic Control Test Codes
        isHittingRight = false;
        //
        isHittingRearHalf = false;
        isHittingRightRear = false;
        isHittingRear = false;
        isHittingLeftRear = false;

        Ray downRay = new Ray(gameObject.transform.position, gameObject.transform.up * -0.8f); // 6/22 Trfc Ctrl, raycast down, not wasting 16 rays!
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.up * -0.8f, Color.green);

        if (Physics.Raycast(downRay, out hit, 0.8f)) // 5/30 Traffic Control Test Codes, casting rays downward
        {
            //Debug.Log(transform.gameObject.name + " IS ON " + hit.transform.gameObject.name + ", TRFC");
            downRayText = hit.transform.gameObject.tag;
            switch (hit.transform.gameObject.tag)
            {
                case "T":
                    //Debug.Log(transform.gameObject.name + " IS AT T INTERSECTION, TRFC");
                    break;
                case "RightOfT":
                    //Debug.Log(transform.gameObject.name + " IS AT THE RIGHT OF T INTERSECTION, TRFC");
                    break;
                case "LeftOfT":
                    //Debug.Log(transform.gameObject.name + " IS AT THE LEFT OF T INTERSECTION, TRFC");
                    break;
                case "4Way":
                    //Debug.Log(transform.gameObject.name + " IS AT THE 4-WAY INTERSECTION, TRFC");
                    break;
                case "Lights":
                    //Debug.Log(transform.gameObject.name + " IS AT THE TRAFFIC LIGHTS, TRFC");
                    break;
                default:
                    break;
            }
        }

        Ray rightSideRay = new Ray(gameObject.transform.position, gameObject.transform.right * 1.55f); // 6/23 Trfc Ctrl, raycast right side, not wasting 16 rays!
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * 1.55f, Color.red);

        // Detecting right mid-side collision with another car
        if (Physics.Raycast(rightSideRay, out hit, 1.55f))
        {
            if (hit.transform.gameObject.tag == "Car")
            {
                isHittingRightSide = true;
                //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE RIGHT SIDE!!!");
            }
        }

        Ray leftSideRay = new Ray(gameObject.transform.position, gameObject.transform.right * -1.55f); // 6/23 Trfc Ctrl, raycast left side, not wasting 16 rays!
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.right * -1.55f, Color.green);

        // Detecting left mid-side collision with another car
        if (Physics.Raycast(leftSideRay, out hit, 1.55f))
        {
            if (hit.transform.gameObject.tag == "Car")
            {
                isHittingLeftSide = true;
                //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE LEFT SIDE!!!");
            }
        }

        for (int i = 1; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;

            var rotationFrontHalf = frontRotation.transform.rotation; // Rays for front half
            //var rotationFrontDown = frontRotation.transform.rotation; // 5/30 Traffic Control Test Codes, casting rays downward
            var rotationRearHalf = rearRotation.transform.rotation; // Rays for rear half
            var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * 25 - 13, this.transform.up); // For the front and back
            var rotationAngleAxTrfc = Quaternion.AngleAxis(i / (float)numberOfRays * (ai_controller.targetAngleTrfc * 1/2), this.transform.up); // For the front, 6/05 Trfc Ctrl
            //var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 160 - 80, this.transform.up); // 6/23 Trfc Ctrl, For the sides
            var rotationAngleAxFront = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, frontRotation.transform.up); // Rays for front half
            //var rotationAngleAxFrontDown = Quaternion.AngleAxis(1, frontRotation.transform.up); // 5/30 Traffic Control Test Codes, casting rays downward
            var rotationAngleAxRear = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, rearRotation.transform.up); // Rays for rear half

            var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;
            var forwardVec3Trfc = rotation * rotationAngleAxTrfc * Vector3.forward; // 6/05 Trfc Ctrl
            var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
            //var rightVec3 = rotation * rotationAngleAxSides * Vector3.right; // Adding the rays on the right
            //var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left
            var frontCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.forward; // Rays for front half
            //var frontCubeDownVec3 = rotationFrontDown * rotationAngleAxFrontDown * Vector3.down; // 5/30 Traffic Control Test Codes, casting rays downward
            var rearCubeVec3 = rotationRearHalf * rotationAngleAxRear * Vector3.back; // Rays for rear half

            Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 5f); // Combining my code to detect frontal collision
            Ray hittingRayTrfc = new Ray(this.transform.position, forwardVec3Trfc * 8f); // 6/05 Trfc Ctrl
            Ray gettingHitRay = new Ray(this.transform.position, backwardVec3 * 3.5f); // Combining my code to detect rear end collision
            //Ray hitRightRay = new Ray(this.transform.position, rightVec3 * 1.5f); // Combining my code to detect right side collision
            //Ray hitLeftRay = new Ray(this.transform.position, leftVec3 * 1.5f); // Combining my code to detect left side collision
            Ray hitFrontHalfRay = new Ray(frontRotation.transform.position, frontCubeVec3 * 2f);
            //Ray hitFrontHalfDownRay = new Ray(frontRotation.transform.position, frontCubeDownVec3 * 0.8f); // 5/30 Traffic Control Test Codes, casting rays downward
            Ray hitRearHalfRay = new Ray(rearRotation.transform.position, rearCubeVec3 * 1.5f);

            //It's EITHER using Debug.DrawRay(), with which I can't color code properly, or Gizmos.DrawRay()
            //Debug.DrawRay(this.transform.position, forwardVec3 * 4f);
            //Debug.DrawRay(this.transform.position, backwardVec3 * 3.5f); // Adding the rays on the back
            //Debug.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            //Debug.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            //Debug.DrawRay(frontRotation.transform.position, frontCubeVec3 * 1.5f); // Rays for front half
            //Debug.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half

            // 7/30 Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
            racingRaycasting = true;

            //Combining my code, detecting frontal collision with another car
            if (Physics.Raycast(hittingRay, out hit, 5f))
            {
                aboutToHitAhead = true;
                if (hit.transform.gameObject.tag == "Car")
                {
                    // Trying World of Zero's avoidance method - It Doesn't Work Well !!
                    //deltaPosition -= (1.0f / 51) * ai_controller.mphSpeedInt * forwardVec3;
                    //this.transform.position += deltaPosition * Time.deltaTime;

                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the left, 6 on the left
                            aboutToHitLeftAhead = true; // For AI_Controller to grab
                            //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE LEFT AHEAD!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            aboutToHitDirectlyAhead = true; 
                            //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " DIRECTLY AHEAD!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                            aboutToHitRightAhead = true;
                            //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE RIGHT AHEAD!!!");
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Physics.Raycast(hittingRayTrfc, out hit, 8f)) // 6/05 Trfc Ctrl
            {
                aboutToHitFarAhead = true;
                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " FAR AHEAD, TRFC"); 
                if (hit.transform.gameObject.tag == "Car" && Mathf.Abs(ai_controller.targetAngleTrfc) <= 25) // 6/07 Trfc Ctrl, if driving straight
                {
                    aboutToHitFarDirectly = true;
                    //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " FAR STRAIGHT AHEAD, TRFC");
                }
                if (hit.transform.gameObject.tag == "Car" && Mathf.Abs(ai_controller.targetAngleTrfc) > 25) // 6/07 Trfc Ctrl, if driving a curve
                {
                    if (ai_controller.targetAngleTrfc > 0) // right curve turn
                    {
                        switch (i)
                        {
                            case int r when i < 7: //Gizmos.color = Color.green; // Starting from the left, 6 on the left
                                aboutToHitFarLeftAhead = true; // For AI_Controller to grab
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE FAR LEFT AHEAD, TRFC");
                                break;
                            case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                                aboutToHitFarDirectly = true;
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " FAR STRAIGHT AHEAD, TRFC");
                                break;
                            case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                                aboutToHitFarRightAhead = true;
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE FAR RIGHT AHEAD, TRFC");
                                break;
                            default:
                                break;
                        }
                    }
                    else // left curve turn
                    {
                        switch (i)
                        {
                            case int r when i < 7: //Gizmos.color = Color.green; // Starting from the right, 6 on the right
                                aboutToHitFarRightAhead = true;
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE FAR RIGHT AHEAD, TRFC");
                                break;
                            case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                                aboutToHitFarDirectly = true;
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " FAR STRAIGHT AHEAD, TRFC");
                                break;
                            case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left, 6 on the left
                                aboutToHitFarLeftAhead = true; // For AI_Controller to grab
                                //Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE FAR LEFT AHEAD, TRFC");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            //Combining my code, detecting rear-end collision with another car
            if (Physics.Raycast(gettingHitRay, out hit, 3.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the right (facing forward), 6 on the right
                            aboutToGetHitRightRear = true;
                            //Debug.Log(transform.gameObject.name + "'S RIGHT REAR IS ABOUT TO GET HIT BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            aboutToGetHitRear = true;
                            //Debug.Log(transform.gameObject.name + " IS ABOUT TO GET REAR ENDED BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            aboutToGetHitLeftRear = true;
                            //Debug.Log(transform.gameObject.name + "'S LEFT REAR IS ABOUT TO GET HIT BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        default:
                            break;
                    }
                }
            }

            //Combining my code, detecting frontal-half collision with another car
            if (Physics.Raycast(hitFrontHalfRay, out hit, 2f))
            {
                isHittingFrontHalf = true;
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the left, 6 on the left
                            isHittingLeft = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " ON THE LEFT FRONT!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            isHittingFront = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY FRONT!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                            isHittingRight = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " ON THE RIGHT FRONT!!!");
                            break;
                        default:
                            break;
                    }
                }

                // 5/13 Traffic Control Test Codes
                if (hit.transform.gameObject.tag == "Threshold")
                {
                    if (i > 6 && i < 11)
                    {
                        atThresholdTrfc = true;
                        //Debug.Log(transform.parent.gameObject.name + " IS AT " + hit.transform.gameObject.name + " THRESHOLD");
                    }
                }
            }

            // 6/23 Trfc Ctrl disabling to not waste 16 rays, detecting right mid-side collision with another car
            //if (Physics.Raycast(hitRightRay, out hit, 1.5f))
            //{
            //    if (hit.transform.gameObject.tag == "Car")
            //    {
            //        //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
            //        if (i > 6 && i < 11)
            //        {
            //            isHittingRightSide = true;
            //            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE RIGHT!!!");
            //        }
            //    }
            //}

            // 6/23 Trfc Ctrl, detecting left mid-side collision with another car
            //if (Physics.Raycast(hitLeftRay, out hit, 1.5f))
            //{
            //    if (hit.transform.gameObject.tag == "Car")
            //    {
            //        //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
            //        if (i > 6 && i < 11)
            //        {
            //            isHittingLeftSide = true;
            //            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE LEFT!!!");
            //        }
            //    }
            //}

            //Combining my code, detecting  rear-half collision with another car
            if (Physics.Raycast(hitRearHalfRay, out hit, 1.5f))
            {
                isHittingRearHalf = true;
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the right (facing forward), 6 on the right
                            isHittingRightRear = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE RIGHT REAR!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            isHittingRear = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY BEHIND!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            isHittingLeftRear = true;
                            //Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE LEFT REAR!!!");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void OnApplicationQuit() // 7/30 Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
    {
        racingRaycasting = false;
    }

    private void OnDrawGizmosSelected() // For debug purpose only, disable upon compiling!
    {
        if (racingRaycasting == true) // 7/30 Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
        {
            for (int i = 1; i < numberOfRays; i++)
            {
                // Following vars are redundant with the ones in Update but needed to color code the rays accordingly
                var rotation = this.transform.rotation;

                var rotationFrontHalf = frontRotation.transform.rotation; // Rays for front half
                                                                          //var rotationFrontDown = frontRotation.transform.rotation; // 5/30 Traffic Control Test Codes, casting rays downward
                var rotationRearHalf = rearRotation.transform.rotation; // Rays for rear half
                var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * 25 - 13, this.transform.up); // For the front and back

                // 7/21 Trfc Ctrl, fixing null ref exception error
                var rotationAngleAxTrfc = Quaternion.AngleAxis(i / (float)numberOfRays * (ai_controller.targetAngleTrfc * 1 / 2), this.transform.up); // 6/05 Trfc Ctrl, the front long ray that angles to targetAngleTrfc
                //try
                //{
                //    rotationAngleAxTrfc = Quaternion.AngleAxis(i / (float)numberOfRays * (ai_controller.targetAngleTrfc * 1 / 2), this.transform.up); // 6/05 Trfc Ctrl, the front long ray that angles to targetAngleTrfc
                //}
                //catch (System.NullReferenceException err)
                //{
                //    //Debug.Log("NullReferenceException: " + err);
                //}

                //var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 160 - 80, this.transform.up); // 6/23 Trfc Ctrl, For the sides
                var rotationAngleAxFront = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, frontRotation.transform.up); // Rays for front half
                                                                                                                                  //var rotationAngleAxFrontDown = Quaternion.AngleAxis(1, frontRotation.transform.up); // 5/30 Traffic Control Test Codes, casting rays downward
                var rotationAngleAxRear = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, rearRotation.transform.up); // Rays for rear half

                var forwardVec3 = rotation * rotationAngleAx * Vector3.forward; // Adding the rays at the front
                var forwardVec3Trfc = rotation * rotationAngleAxTrfc * Vector3.forward; // 6/05 Trfc Ctrl
                var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
                                                                              //var rightVec3 = rotation * rotationAngleAxSides * Vector3.right; // 6/23 Trfc Ctrl, Adding the rays on the right
                                                                              //var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left
                var frontCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.forward; // Rays for front half
                                                                                                //var frontCubeDownVec3 = rotationFrontDown * rotationAngleAxFrontDown * Vector3.down; // 5/30 Traffic Control Test Codes, casting rays downward
                var rearCubeVec3 = rotationRearHalf * rotationAngleAxRear * Vector3.back; // Rays for rear half

                //Combining my code, color coding the rays
                switch (i)
                {
                    case int r when i < 7:
                        Gizmos.color = Color.green; // Starting from the left, 6 on the left
                        break;
                    case int r when i > 6 && i < 11:
                        Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                        break;
                    case int r when i > 10 && i < 18:
                        Gizmos.color = Color.red; // Ending at the right, 6 on the right
                        break;
                    default:
                        break;
                }

                //It's EITHER using Debug.DrawRay(), with which I can't color code properly, or Gizmos.DrawRay()
                Gizmos.DrawRay(this.transform.position, forwardVec3 * 5f); // Adding the rays at the front
                Gizmos.DrawRay(this.transform.position, forwardVec3Trfc * 8f); // 6/05 Trfc Ctrl, Adding longer rays at the front
                Gizmos.DrawRay(this.transform.position, backwardVec3 * 3.5f); // Adding the rays on the back
                                                                              //Gizmos.DrawRay(this.transform.position, rightVec3 * 1.5f); // 6/23 Trfc Ctrl, Adding the rays on the right
                                                                              //Gizmos.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
                Gizmos.DrawRay(frontRotation.transform.position, frontCubeVec3 * 2f); // Rays for front half
                                                                                      //Gizmos.DrawRay(frontRotation.transform.position, frontCubeDownVec3 * 0.8f); // 5/30 Traffic Control Test Codes, casting rays downward
                Gizmos.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half
            }
        }
    }
}
