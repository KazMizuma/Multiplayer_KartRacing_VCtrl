using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class driverNameUI : MonoBehaviour
{
    public Text driverName;
    public Transform targetCar;

    // Awake is called when the script instanced is being loaded
    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        driverName = this.GetComponent<Text>();
    }

    void LateUpdate()
    {
        this.transform.position = Camera.main.WorldToScreenPoint(targetCar.position);
    }
}
