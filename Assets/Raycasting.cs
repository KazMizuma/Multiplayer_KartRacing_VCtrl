using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    public float targetVelocity = 10.0f;
    public int numberOfRays = 17;
    public float angle = 30.0f;
    public GameObject frontRotation; // Rays for front half
    public GameObject rearRotation; // Rays for rear half 

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

            var rotationFrontHalf = frontRotation.transform.rotation; // Rays for front half
            var rotationRearHalf = rearRotation.transform.rotation; // Rays for rear half

            var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * angle - 15, this.transform.up); // For the front and back
            var rotationAngleAxSides = Quaternion.AngleAxis(i / (float)numberOfRays * 160 - 80, this.transform.up); // For the sides

            var rotationAngleAxFront = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, frontRotation.transform.up); // Rays for front half
            var rotationAngleAxRear = Quaternion.AngleAxis(i / (float)numberOfRays * 240 - 120, rearRotation.transform.up); // Rays for rear half

            var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;
            var backwardVec3 = rotation * rotationAngleAx * Vector3.back; // Adding the rays on the back
            var rightVec3 = rotation * rotationAngleAxSides * Vector3.right ; // Adding the rays on the right
            var leftVec3 = rotation * rotationAngleAxSides * Vector3.left; // Adding the rays on the left

            var frontCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.forward; // Rays for front half
            var rearCubeVec3 = rotationFrontHalf * rotationAngleAxFront * Vector3.back; // Rays for rear half

            // Color coding the rays
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
            Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 3.5f); //Combining my code to detect frontal collision

            Gizmos.DrawRay(this.transform.position, backwardVec3 * 2.75f); // Adding the rays on the back
            Ray gettingHitRay = new Ray(this.transform.position, backwardVec3 * 2.75f); //Combining my code to detect rear end collision

            Gizmos.DrawRay(this.transform.position, rightVec3 * 1.5f); // Adding the rays on the right
            Ray hitRightRay = new Ray(this.transform.position, rightVec3 * 1.5f); // Combining my code to detect right side collision

            Gizmos.DrawRay(this.transform.position, leftVec3 * 1.5f); // Adding the rays on the left
            Ray hitLeftRay = new Ray(this.transform.position, leftVec3 * 1.5f); // Combining my code to detect left side collision

            Gizmos.DrawRay(frontRotation.transform.position, frontCubeVec3 * 1.5f); // Rays for front half
            Ray hitFrontHalfRay = new Ray(frontRotation.transform.position, frontCubeVec3 * 1.5f);

            Gizmos.DrawRay(rearRotation.transform.position, rearCubeVec3 * 1.5f); // Rays for rear half
            Ray hitRearHalfRay = new Ray(rearRotation.transform.position, rearCubeVec3 * 1.5f);

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
            if (Physics.Raycast(gettingHitRay, out hit, 2.75f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " GETTING HIT BY " + hit.transform.gameObject.name + "!!!");
                }
            }

            //Combining my code, detecting right side collision with another car
            if (Physics.Raycast(hitRightRay, out hit, 1.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + " ON MY RIGHT!!!");
                    //Debug.Log(this.transform.position + " HIT " + hit.transform.position + " ON MY RIGHT!!!");
                }
            }

            //Combining my code, detecting left side collision with another car
            if (Physics.Raycast(hitLeftRay, out hit, 1.5f))
            {
                if (hit.transform.gameObject.tag == "Car")
                {
                    Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + " ON MY LEFT!!!");
                }
            }
        }
    }
}
