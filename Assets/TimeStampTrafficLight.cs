using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStampTrafficLight : MonoBehaviour
{
    public GameObject Light0; // 7/16 Trfc Ctrl, North
    public string LightAt6;
    public float timeAt6;

    public GameObject Light2; // South
    public string LightAt18;
    public float timeAt18;

    public GameObject Light3; // East
    public string LightAt36;
    public float timeAt36;

    public GameObject Light1; // West
    public string LightAt46;
    public float timeAt46;

    public float trafficLightTime = 0.0f;
    //public float trafficLightTime2 = 0.0f;

    //private string green;
    //private string yellow;
    //private string red;

    //AI_Controller ai_Controller;

    // Start is called before the first frame update
    void Start()
    {
        //Light0.GetComponent<MeshRenderer>().materials[0].color = Color.gray; // 7/09 Trfc Ctrl
        //Debug.Log("trafficLight " + Light0.GetComponent<MeshRenderer>().materials[0].color);

        timeAt6 = 0f; // Light0; North
        timeAt18 = 0f; // Light2; South
        timeAt36 = 0f; // Light3; East
        timeAt46 = 0f; // Light1; West

        //Light0 = GameObject.Find("Light0Yellow");
        //Light0.GetComponent<Light>().enabled = false;
        //Light0 = GameObject.Find("Light0Red");
        //Light0.GetComponent<Light>().enabled = false;
        //Light0 = GameObject.Find("Light0Green");
        //Light0.GetComponent<Light>().enabled = true;
        //Debug.Log("Light0Green: " + Light0.GetComponent<Light>().enabled);

        //Light2 = GameObject.Find("Light2Yellow");
        //Light2.GetComponent<Light>().enabled = false;
        //Light2 = GameObject.Find("Light2Red");
        //Light2.GetComponent<Light>().enabled = false;
        //Light2 = GameObject.Find("Light2Green");
        //Light2.GetComponent<Light>().enabled = true;
        //Debug.Log("Light2Green: " + Light2.GetComponent<Light>().enabled);

        //Light3 = GameObject.Find("Light3Yellow");
        //Light3.GetComponent<Light>().enabled = false;
        //Light3 = GameObject.Find("Light3Red");
        //Light3.GetComponent<Light>().enabled = true;
        //Light3 = GameObject.Find("Light3Green");
        //Light3.GetComponent<Light>().enabled = false;
        //Debug.Log("Light3Red: " + Light3.GetComponent<Light>().enabled);

        //Light1 = GameObject.Find("Light1Yellow");
        //Light1.GetComponent<Light>().enabled = false;
        //Light1 = GameObject.Find("Light1Red");
        //Light1.GetComponent<Light>().enabled = true;
        //Light1 = GameObject.Find("Light1Green");
        //Light1.GetComponent<Light>().enabled = false;
        //Debug.Log("Light1Red: " + Light1.GetComponent<Light>().enabled);

        //InvokeRepeating("At6At18Green", 20f, 20f);
        //InvokeRepeating("At36At46Green", 20f, 20f);
        //InvokeRepeating("At6At18Yellow", 2f, 40f);
        //InvokeRepeating("At36At46Yellow", 2f, 40f);
        //InvokeRepeating("At6At18Red", 20f, 40f);       
        //InvokeRepeating("At36At46Red", 20f, 40f);
    }

    public void At6At18Green()
    {
        //Debug.Log("At6At18Green()");
        Light0 = GameObject.Find("Light0Red");
        Light0.GetComponent<Light>().enabled = false;
        Light2 = GameObject.Find("Light2Red");
        Light2.GetComponent<Light>().enabled = false;

        Light0 = GameObject.Find("Light0Green");
        Light0.GetComponent<Light>().enabled = true;
        LightAt6 = "Green";
        Light2 = GameObject.Find("Light2Green");
        Light2.GetComponent<Light>().enabled = true;
        LightAt18 = "Green";
    }

    public void At6At18Yellow()
    {
        //Debug.Log("At6At18Yellow()");
        Light0 = GameObject.Find("Light0Green");
        Light0.GetComponent<Light>().enabled = false;
        Light2 = GameObject.Find("Light2Green");
        Light2.GetComponent<Light>().enabled = false;

        Light0 = GameObject.Find("Light0Yellow");
        Light0.GetComponent<Light>().enabled = true;
        LightAt6 = "Yellow";
        Light2 = GameObject.Find("Light2Yellow");
        Light2.GetComponent<Light>().enabled = true;
        LightAt18 = "Yellow";
    }

    public void At6At18Red()
    {
        //Debug.Log("At6At18Red()");
        Light0 = GameObject.Find("Light0Yellow");
        Light0.GetComponent<Light>().enabled = false;
        Light2 = GameObject.Find("Light2Yellow");
        Light2.GetComponent<Light>().enabled = false;

        Light0 = GameObject.Find("Light0Red");
        Light0.GetComponent<Light>().enabled = true;
        LightAt6 = "Red";
        Light2 = GameObject.Find("Light2Red");
        Light2.GetComponent<Light>().enabled = true;
        LightAt18 = "Red";
    }

    public void At36At46Green()
    {
        //Debug.Log("At36At46Green()");
        Light3 = GameObject.Find("Light3Red");
        Light3.GetComponent<Light>().enabled = false;
        Light1 = GameObject.Find("Light1Red");
        Light1.GetComponent<Light>().enabled = false;

        Light3 = GameObject.Find("Light3Green");
        Light3.GetComponent<Light>().enabled = true;
        LightAt36 = "Green";
        Light1 = GameObject.Find("Light1Green");
        Light1.GetComponent<Light>().enabled = true;
        LightAt46 = "Green";
    }

    public void At36At46Yellow()
    {
        //Debug.Log("At36At46Yellow()");
        Light3 = GameObject.Find("Light3Green");
        Light3.GetComponent<Light>().enabled = false;
        Light1 = GameObject.Find("Light1Green");
        Light1.GetComponent<Light>().enabled = false;

        Light3 = GameObject.Find("Light3Yellow");
        Light3.GetComponent<Light>().enabled = true;
        LightAt36 = "Yellow";
        Light1 = GameObject.Find("Light1Yellow");
        Light1.GetComponent<Light>().enabled = true;
        LightAt46 = "Yellow";
    }

    public void At36At46Red()
    {
        //Debug.Log("At36At46Red()");
        Light3 = GameObject.Find("Light3Yellow");
        Light3.GetComponent<Light>().enabled = false;
        Light1 = GameObject.Find("Light1Yellow");
        Light1.GetComponent<Light>().enabled = false;

        Light3 = GameObject.Find("Light3Red");
        Light3.GetComponent<Light>().enabled = true;
        LightAt36 = "Red";
        Light1 = GameObject.Find("Light1Red");
        Light1.GetComponent<Light>().enabled = true;
        LightAt46 = "Red";
    }

    public void trafficLightAt6At18(string light)
    {
        switch (light)
        {
            case ("green"):
                At6At18Green();
                //Debug.Log("case (green): At6At18Green()");
                //yield return new WaitForSecondsRealtime(20f);
                break;

            case ("yellow"):
                At6At18Yellow();
                //yield return new WaitForSecondsRealtime(2f);
                break;

            case ("red"):
                At6At18Red();
                //yield return new WaitForSecondsRealtime(20f);
                break;

            default:

                break;
        }
    }

    public void trafficLightAt36At46(string light)
    {
        switch (light)
        {
            case ("green"):
                At36At46Green();
                //yield return new WaitForSecondsRealtime(20f);
                break;

            case ("yellow"):
                At36At46Yellow();
                //yield return new WaitForSecondsRealtime(2f);
                break;

            case ("red"):
                At36At46Red();
                //Debug.Log("case (red): At36At46Red()");
                //yield return new WaitForSecondsRealtime(20f);
                break;

            default:

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        trafficLightTime += Time.deltaTime;
        if (trafficLightTime >= 0 && trafficLightTime < 20)
        {
            //Debug.Log("if (trafficLightTime0 >= 0)");
            trafficLightAt6At18("green"); 
        }
        if (trafficLightTime >= 20 && trafficLightTime < 22)
        {
            trafficLightAt6At18("yellow");
        }
        if (trafficLightTime >= 22 && trafficLightTime < 42)
        {
            trafficLightAt6At18("red");
        }
        if (trafficLightTime >= 42)
        {
            trafficLightTime = 0;
        }

        //trafficLightTime2 += Time.deltaTime;
        if (trafficLightTime >= 0 && trafficLightTime < 20)
        {
            //Debug.Log("if (trafficLightTime2 >= 0)");
            trafficLightAt36At46("red");
        }
        if (trafficLightTime >= 20 && trafficLightTime < 40)
        {
            trafficLightAt36At46("green");
        }
        if (trafficLightTime >= 40 && trafficLightTime < 42)
        {
            trafficLightAt36At46("yellow");
        }
        //if (trafficLightTime >= 42)
        //{
        //    trafficLightTime2 = 0;
        //}

        //At6At18Green
        //At36At46Green
        //At6At18Yellow
        //At36At46Yellow
        //At6At18Red       
        //At36At46Red

        //timeAt6 = ai_Controller.timeAt6;
        //timeAt18 = ai_Controller.timeAt18;
        //timeAt36 = ai_Controller.timeAt36;
        //timeAt46 = ai_Controller.timeAt46;
    }
}