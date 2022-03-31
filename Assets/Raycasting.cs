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

    // For AI_Controller to grab
    public bool aboutToHitAhead = false;
    public bool aboutToHitLeftAhead = false; 
    public bool aboutToHitDirectlyAhead = false;
    public bool aboutToHitRightAhead = false;
    //
    public bool aboutToGetHitRightRear = false;
    public bool aboutToGetHitRear = false;
    public bool aboutToGetHitLeftRear = false;
    //
    public bool isHittingRightSide = false;
    public bool isHittingLeftSide = false;
    //
    public bool isHittingFrontHalf = false;
    public bool isHittingLeft = false;
    public bool isHittingFront = false;
    public bool isHittingRight = false;
    //
    public bool isHittingRearHalf = false;
    public bool isHittingRightRear = false;
    public bool isHittingRear = false;
    public bool isHittingLeftRear = false;

    //AI_Controller ai_controller; // To access AI_Controller

    //public Vector3 deltaPosition; // Trying World of Zero's avoidance method

    // Start is called before the first frame update
    void Start()
    {
        //ai_controller = this.transform.parent.GetComponent<AI_Controller>(); // To access AI_Controller
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

        for (int i = 1; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;

            var rotationFrontHalf = frontRotation.transform.rotation; // Rays for front half
            var rotationRearHalf = rearRotation.transform.rotation; // Rays for rear half
            var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * angle - 15, this.transform.up); // For the front and back
            var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 160 - 80, this.transform.up); // For the sides
            var rotationAngleAxFront = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, frontRotation.transform.up); // Rays for front half
            var rotationAngleAxRear = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, rearRotation.transform.up); // Rays for rear half

            var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;
            var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
            var rightVec3 = rotation * rotationAngleAxSides * Vector3.right; // Adding the rays on the right
            var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left
            var frontCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.forward; // Rays for front half
            var rearCubeVec3 = rotationRearHalf * rotationAngleAxRear * Vector3.back; // Rays for rear half

            Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 4f); //Combining my code to detect frontal collision
            Ray gettingHitRay = new Ray(this.transform.position, backwardVec3 * 3.5f); //Combining my code to detect rear end collision
            Ray hitRightRay = new Ray(this.transform.position, rightVec3 * 1.5f); // Combining my code to detect right side collision
            Ray hitLeftRay = new Ray(this.transform.position, leftVec3 * 1.5f); // Combining my code to detect left side collision
            Ray hitFrontHalfRay = new Ray(frontRotation.transform.position, frontCubeVec3 * 2f);
            Ray hitRearHalfRay = new Ray(rearRotation.transform.position, rearCubeVec3 * 1.5f);

            //It's EITHER using Debug.DrawRay(), with which I can't color code properly, or Gizmos.DrawRay()
            //Debug.DrawRay(this.transform.position, forwardVec3 * 4f);
            //Debug.DrawRay(this.transform.position, backwardVec3 * 3.5f); // Adding the rays on the back
            //Debug.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            //Debug.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            //Debug.DrawRay(frontRotation.transform.position, frontCubeVec3 * 1.5f); // Rays for front half
            //Debug.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half

            //Combining my code, detecting frontal collision with another car
            if (Physics.Raycast(hittingRay, out hit, 4f))
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
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE LEFT AHEAD!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            aboutToHitDirectlyAhead = true; 
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " DIRECTLY AHEAD!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                            aboutToHitRightAhead = true;
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE RIGHT AHEAD!!!");
                            break;
                        default:
                            break;
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
                            Debug.Log(transform.gameObject.name + "'S RIGHT REAR IS ABOUT TO GET HIT BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            aboutToGetHitRear = true;
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO GET REAR ENDED BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            aboutToGetHitLeftRear = true;
                            Debug.Log(transform.gameObject.name + "'S LEFT REAR IS ABOUT TO GET HIT BY " + hit.transform.gameObject.name + "!!!");
                            break;
                        default:
                            break;
                    }
                }
            }

            //Combining my code, detecting right mid-side collision with another car
            if (Physics.Raycast(hitRightRay, out hit, 1.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    if (i > 6 && i < 11)
                    {
                        isHittingRightSide = true;
                        Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE RIGHT!!!");
                    }
                }
            }

            //Combining my code, detecting left mid-side collision with another car
            if (Physics.Raycast(hitLeftRay, out hit, 1.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    if (i > 6 && i < 11)
                    {
                        isHittingLeftSide = true;
                        Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE LEFT!!!");
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
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " ON THE LEFT FRONT!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            isHittingFront = true;
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY FRONT!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                            isHittingRight = true;
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " ON THE RIGHT FRONT!!!");
                            break;
                        default:
                            break;
                    }
                }
            }

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
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE RIGHT REAR!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            isHittingRear = true;
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY BEHIND!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            isHittingLeftRear = true;
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE LEFT REAR!!!");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() // For debug purpose only, disable upon compiling!
    {
        for (int i = 1; i < numberOfRays; i++)
        {
            // Following vars are redundant with the ones in Update but needed to color code the rays accordingly
            var rotation = this.transform.rotation;

            var rotationFrontHalf = frontRotation.transform.rotation; // Rays for front half
            var rotationRearHalf = rearRotation.transform.rotation; // Rays for rear half
            var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * angle - 15, this.transform.up); // For the front and back
            var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 160 - 80, this.transform.up); // For the sides
            var rotationAngleAxFront = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, frontRotation.transform.up); // Rays for front half
            var rotationAngleAxRear = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, rearRotation.transform.up); // Rays for rear half

            var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;
            var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
            var rightVec3 = rotation * rotationAngleAxSides * Vector3.right; // Adding the rays on the right
            var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left
            var frontCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.forward; // Rays for front half
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
            Gizmos.DrawRay(this.transform.position, forwardVec3 * 4f);
            Gizmos.DrawRay(this.transform.position, backwardVec3 * 3.5f); // Adding the rays on the back
            Gizmos.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            Gizmos.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            Gizmos.DrawRay(frontRotation.transform.position, frontCubeVec3 * 2f); // Rays for front half
            Gizmos.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half
        }
    }
}
