﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingLights2 : MonoBehaviour
{
    // Attached to Cube (24)
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

        RaycastHit hitTrfc;

        Ray leftRay = new Ray(gameObject.transform.position, gameObject.transform.forward * -20f); // 6/28 Trfc Ctrl, raycasting to the left
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * -20f, Color.green);

        Ray rightRay = new Ray(gameObject.transform.position, gameObject.transform.forward * 9f); // 7/17 Trfc Ctrl, raycasting to the right
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 9f, Color.green);

        if (Physics.Raycast(leftRay, out hitTrfc, 20f) || Physics.Raycast(rightRay, out hitTrfc, 9f))
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
