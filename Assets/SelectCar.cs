using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //added 3/22/22

public class SelectCar : MonoBehaviour
{
    public GameObject[] cars;
    int currentCar; 
    // Start is called before the first frame update
    void Start() 
    {
        if (PlayerPrefs.HasKey("CarSelection"))
        {
            currentCar = PlayerPrefs.GetInt("CarSelection");
        }
        else
        {
            currentCar = 0;
        }

        this.transform.LookAt(cars[currentCar].transform.position);
        //Debug.Log("cars.Length: " + cars.Length);
        //Debug.Log("currentCar: " + currentCar);
    }

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentCar == cars.Length - 1)
            {
                //Debug.Log("currentCar == cars.Length True");
                currentCar = 0;
                //Debug.Log("currentCar = 0: " + currentCar);
            }
            else
            {
                currentCar++;
                //Debug.Log("currentCar++: " + currentCar);
            }

            PlayerPrefs.SetInt("CarSelection", currentCar);
        }

        //this.transform.LookAt(cars[currentCar].transform.position);
        Quaternion lookDir = Quaternion.LookRotation(cars[currentCar].transform.position - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, Time.deltaTime);
    }
}
