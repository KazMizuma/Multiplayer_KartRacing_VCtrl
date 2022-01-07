using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    public float targetVelocity = 10.0f;
    public int numberOfRays = 17;
    public float angle = 90.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }

    private void OnDrawGizmos()
    {
        // World of Zero's Raycasting and Object Avoidance Algorithm  https://youtu.be/SVazwHyfB7g?t=944

        RaycastHit hit; // Combining my code
        //Ray hittingRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 4f, Color.green);
        //Ray gettingHitRay = new Ray(gameObject.transform.position, -gameObject.transform.forward);
        //Debug.DrawRay(gameObject.transform.position, -gameObject.transform.forward * 2.5f, Color.white);

        for (int i = 1; i < numberOfRays; i++)
        {
            var rotation = this.transform.rotation;
            var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * angle - 45, this.transform.up); // For the front and back
            var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 110 - 55, this.transform.up); // Just for the sides

            var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;
            var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
            var rightVec3 = rotation * rotationAngleAxSides * Vector3.right ; // Adding the rays on the right
            var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left

            Gizmos.DrawRay(this.transform.position, forwardVec3 * 3.5f);
            Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 3.5f); //Combining my code to detect frontal collision

            Gizmos.DrawRay(this.transform.position, backwardVec3 * 3.5f); // Adding the rays on the back
            Ray gettingHitRay = new Ray(this.transform.position, backwardVec3 * 3.5f); //Combining my code to detect rear end collision

            Gizmos.DrawRay(this.transform.position, rightVec3 * 2.25f); // Adding the rays on the right
            Ray hitRightRay = new Ray(this.transform.position, rightVec3 * 2.25f); // Combining my code to detect right side collision

            Gizmos.DrawRay(this.transform.position, leftVec3 * 2.25f); // Adding the rays on the left
            Ray hitLeftRay = new Ray(this.transform.position, leftVec3 * 2.25f); // Combining my code to detect left side collision

            //Combining my code, detecting frontal collision with another car
            if (Physics.Raycast(hittingRay, out hit, 3.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + "!!!");
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + "!!!");
                }
            }

            //Combining my code, detecting rear end collision with another car
            if (Physics.Raycast(gettingHitRay, out hit, 3.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " GETTING HIT BY " + hit.transform.gameObject.name + "!!!");
                }
            }

            //Combining my code, detecting right side collision with another car
            if (Physics.Raycast(hitRightRay, out hit, 2.25f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + " ON MY RIGHT!!!");
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + " ON MY RIGHT!!!");
                }
            }

            //Combining my code, detecting left side collision with another car
            if (Physics.Raycast(hitLeftRay, out hit, 2.25f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + " ON MY LEFT!!!");
                }
            }
        }
    }
}
