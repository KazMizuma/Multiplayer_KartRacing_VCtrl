using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastingTrfc9 : MonoBehaviour
{
    // Attached to Cube (19)
    public string nameOfPoint;
    //public bool carOnRight = false;
    //public bool carOnLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        nameOfPoint = this.transform.gameObject.name;
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
        nameOfPoint = this.transform.gameObject.name;

        RaycastHit hitTrfc;

        //Ray rightRay = new Ray(gameObject.transform.position, gameObject.transform.forward * -10f); // 6/23 Trfc Ctrl, raycasting to the right
        //Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * -10f, Color.green);

        //if (Physics.Raycast(rightRay, out hitTrfc, 10f))
        //{
        //    if (hitTrfc.transform.gameObject.tag == "Car")
        //    {
        //        //carOnLeft = true;
        //        nameOfPoint = transform.gameObject.name;
        //        Debug.Log("At " + nameOfPoint + ", " + hitTrfc.transform.parent.name + " is on the left/front!");
        //    }
        //}

        Ray leftRay = new Ray(gameObject.transform.position, gameObject.transform.forward * 30f); // 6/23 Trfc Ctrl, raycasting to the left
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 30f, Color.green);

        if (Physics.Raycast(leftRay, out hitTrfc, 30f))
        {
            if (hitTrfc.transform.gameObject.tag == "Car")
            {
                //carOnLeft = true;
                nameOfPoint = "Left Not Clear";
                Debug.Log("At " + nameOfPoint + ", " + hitTrfc.transform.parent.name + " is on the left!");
            }
        }
    }
}
