using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        Ray hittingRay = new Ray(gameObject.transform.position, gameObject.transform.forward);
        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 2.5f, Color.green);

        Ray gettingHitRay = new Ray(gameObject.transform.position, -gameObject.transform.forward);
        Debug.DrawRay(gameObject.transform.position, -gameObject.transform.forward * 2.5f, Color.white);

        if (Physics.Raycast(hittingRay, out hit, 2.5f))
        {
            if (hit.transform.gameObject.tag == "Car")
            {
                Debug.Log(transform.gameObject.name + " HIT " + hit.transform.gameObject.name + "!!!");
            }
        }

        if (Physics.Raycast(gettingHitRay, out hit, 2.5f))
        {
            if (hit.transform.gameObject.tag == "Car")
            {
                Debug.Log(transform.gameObject.name + " GETTING HIT BY " + hit.transform.gameObject.name + "!!!");
            }
        }
    }
}
