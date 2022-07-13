using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStampTrafficLight : MonoBehaviour
{
    public float timeAt6; // 6/30 Trfc Ctrl
    public GameObject Light0; // 7/09 Trfc Ctrl, North
    public float timeAt18; // South
    public GameObject Light2;
    public float timeAt36; // East
    public GameObject Light3;
    public float timeAt46; // West
    public GameObject Light1;

    //AI_Controller ai_Controller;

    // Start is called before the first frame update
    void Start()
    {
        //Light0.GetComponent<MeshRenderer>().materials[0].color = Color.gray; // 7/09 Trfc Ctrl
        //Debug.Log("trafficLight " + Light0.GetComponent<MeshRenderer>().materials[0].color);

        timeAt6 = 0f; // North
        timeAt18 = 0f; // South
        timeAt36 = 0f; // East
        timeAt46 = 0f; // West

        //Light0 = GameObject.Find("Light0Yellow");
        //Light0.GetComponent<Light>().enabled = false;
        //Light0 = GameObject.Find("Light0Red");
        //Light0.GetComponent<Light>().enabled = false;
        Light0 = GameObject.Find("Light0Green");
        Light0.GetComponent<Light>().enabled = true;
        Debug.Log("Light0Green: " + Light0.GetComponent<Light>().enabled);
        //Light2 = GameObject.Find("Light2Yellow");
        //Light2.GetComponent<Light>().enabled = false;
        //Light2 = GameObject.Find("Light2Red");
        //Light2.GetComponent<Light>().enabled = false;
        Light2 = GameObject.Find("Light2Green");
        Light2.GetComponent<Light>().enabled = true;
        Debug.Log("Light2Green: " + Light2.GetComponent<Light>().enabled);
        //Light3 = GameObject.Find("Light3Yellow");
        //Light3.GetComponent<Light>().enabled = false;
        Light3 = GameObject.Find("Light3Red");
        Light3.GetComponent<Light>().enabled = true;
        //Light3 = GameObject.Find("Light3Green");
        //Light3.GetComponent<Light>().enabled = false;
        Debug.Log("Light3Red: " + Light3.GetComponent<Light>().enabled);
        //Light1 = GameObject.Find("Light1Yellow");
        //Light1.GetComponent<Light>().enabled = false;
        Light1 = GameObject.Find("Light1Red");
        Light1.GetComponent<Light>().enabled = true;
        //Light1 = GameObject.Find("Light1Green");
        //Light1.GetComponent<Light>().enabled = false;
        Debug.Log("Light1Red: " + Light1.GetComponent<Light>().enabled);
    }

    // Update is called once per frame
    void Update()
    {


        //timeAt6 = ai_Controller.timeAt6;
        //timeAt18 = ai_Controller.timeAt18;
        //timeAt36 = ai_Controller.timeAt36;
        //timeAt46 = ai_Controller.timeAt46;
    }
}