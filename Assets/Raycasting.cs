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

        Debug.DrawRay(gameObject.transform.position, gameObject.transform.forward * 3f, Color.green);

        if (Physics.Raycast(hittingRay, out hit, 3f))
        {
            if (hit.transform.gameObject.tag == "Car")
            {
                Debug.Log("HIT THE " + hit.transform.gameObject.name + "!!!");
            }
        }
    }
}
