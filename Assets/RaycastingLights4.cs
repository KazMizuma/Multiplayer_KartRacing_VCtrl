using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingLights4 : MonoBehaviour
{
    // Attached to Cube (64)
    public string nameIs;
    //public bool carOnRight = false;
    //public bool carOnLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        nameIs = this.transform.gameObject.name;
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if (other.transform.gameObject.tag == "Car")
    //    {
    //        Debug.Log(nameOfPoint + " OnTriggerStay, Car: " + other.transform.gameObject.tag);
    //        nameOfPoint = transform.gameObject.name;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        //carOnRight = false;
        //carOnLeft = false;
        nameIs = this.transform.gameObject.name;

        var rotation = this.transform.rotation;

        //var rotationAngleAx = Quaternion.AngleAxis(i / (float)numberOfRays * 25 - 13, this.transform.up);
        var rotationAngleAx = Quaternion.AngleAxis(-10, this.transform.up);

        var forwardVec3 = rotation * rotationAngleAx * Vector3.forward;

        //Ray hittingRay = new Ray(this.transform.position, forwardVec3 * 5f);

        //Ray backRay = new Ray(gameObject.transform.position, gameObject.transform.forward * -30f); // 6/28 Trfc Ctrl, raycasting to the left
        Ray backRay = new Ray(gameObject.transform.position, forwardVec3 * -20f); // 7/06 Trfc Ctrl Test Code

        Ray forwardRay = new Ray(gameObject.transform.position, gameObject.transform.forward * 9f); // 7/17 Trfc Ctrl

        RaycastHit hitTrfc;

        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 9f, Color.green); // 7/17 Trfc Ctrl
        Debug.DrawRay(gameObject.transform.position, forwardVec3 * -20f, Color.green);

        if (Physics.Raycast(backRay, out hitTrfc, 20f) || Physics.Raycast(forwardRay, out hitTrfc, 9f))
        {
            if (hitTrfc.transform.gameObject.tag == "Car")
            {
                //carOnRight = true;
                nameIs = hitTrfc.transform.parent.name;
                //Debug.Log("At " + nameOfPoint + ", " + hitTrfc.transform.parent.name + " is on the front!");
            }
        }

        //Ray leftRay = new Ray(gameObject.transform.position, gameObject.transform.forward * 25f); // 6/23 Trfc Ctrl, raycasting to the left
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 25f, Color.green);

        //if (Physics.Raycast(leftRay, out hitTrfc, 25f))
        //{
        //    if (hitTrfc.transform.gameObject.tag == "Car")
        //    {
        //        //carOnLeft = true;
        //        nameOfPoint = "Front Not Clear";
        //        Debug.Log("At " + nameOfPoint + ", " + hitTrfc.transform.parent.name + " is on the left/front/right!");
        //    }
        //}
    }
}
