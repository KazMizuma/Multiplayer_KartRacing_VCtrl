﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    //public float targetVelocity = 10.0f; // World of Zero's, I'm not using it
    public int numberOfRays = 17;
    public float angle = 30.0f;
    public GameObject frontRotation; // Rays for front half
    public GameObject rearRotation; // Rays for rear half 

    //AI_Controller ai_controller; // To call accel & brake values

    // Start is called before the first frame update
    void Start()
    {
        //ai_controller = this.transform.parent.GetComponent<AI_Controller>(); // To call accel & brake values
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

            Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 3.5f); //Combining my code to detect frontal collision
            Ray gettingHitRay = new Ray(this.transform.position, backwardVec3 * 2.75f); //Combining my code to detect rear end collision
            Ray hitRightRay = new Ray(this.transform.position, rightVec3 * 1.5f); // Combining my code to detect right side collision
            Ray hitLeftRay = new Ray(this.transform.position, leftVec3 * 1.5f); // Combining my code to detect left side collision
            Ray hitFrontHalfRay = new Ray(frontRotation.transform.position, frontCubeVec3 * 1.5f);
            Ray hitRearHalfRay = new Ray(rearRotation.transform.position, rearCubeVec3 * 1.5f);

            //Debug.DrawRay(this.transform.position, forwardVec3 * 3.5f);
            //Debug.DrawRay(this.transform.position, backwardVec3 * 2.75f); // Adding the rays on the back
            //Debug.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            //Debug.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            //Debug.DrawRay(frontRotation.transform.position, frontCubeVec3 * 1.5f); // Rays for front half
            //Debug.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half

            //Combining my code, detecting frontal collision with another car
            if (Physics.Raycast(hittingRay, out hit, 3.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the left, 6 on the left
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE LEFT AHEAD!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " DIRECTLY AHEAD!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " ON THE RIGHT AHEAD!!!");
                            break;
                        default:
                            break;
                    }
                }
            }

            //Combining my code, detecting rear-end collision with another car
            if (Physics.Raycast(gettingHitRay, out hit, 2.75f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the right (facing forward), 6 on the right
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " WITH THE RIGHT REAR!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " DIRECTLY BEHIND!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            Debug.Log(transform.gameObject.name + " IS ABOUT TO HIT " + hit.transform.gameObject.name + " WITH THE LEFT REAR!!!");
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
                        Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY ON THE LEFT!!!");
                    }
                }
            }

            //Combining my code, detecting frontal-half collision with another car
            if (Physics.Raycast(hitFrontHalfRay, out hit, 1.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the left, 6 on the left
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " ON THE LEFT FRONT!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight ahead (from relative point), 4 in the middle
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY FRONT!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the right, 6 on the right
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
                if (hit.transform.gameObject.tag == "Car")
                {
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                    switch (i)
                    {
                        case int r when i < 7: //Gizmos.color = Color.green; // Starting from the right (facing forward), 6 on the right
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE RIGHT REAR!!!");
                            break;
                        case int r when i > 6 && i < 11: //Gizmos.color = Color.white; // Straight behind (from relative point), 4 in the middle
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " DIRECTLY BEHIND!!!");
                            break;
                        case int r when i > 10 && i < 18: //Gizmos.color = Color.red; // Ending at the left (facing forward), 6 on the left
                            Debug.Log(transform.gameObject.name + " IS HITTING " + hit.transform.gameObject.name + " WITH THE LEFT REAR!!!");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void OnDrawGizmos() // Following vars are redundant with the ones in Update but needed to color code the rays accordingly
    {
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
            Gizmos.DrawRay(this.transform.position, forwardVec3 * 3.5f);
            Gizmos.DrawRay(this.transform.position, backwardVec3 * 2.75f); // Adding the rays on the back
            Gizmos.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            Gizmos.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            Gizmos.DrawRay(frontRotation.transform.position, frontCubeVec3 * 1.5f); // Rays for front half
            Gizmos.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half
        }
    }
}
