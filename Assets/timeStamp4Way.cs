using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeStamp4Way : MonoBehaviour
{
    public float timeAt81; // 6/30 Trfc Ctrl
    public float timeAt24;
    public float timeAt44;
    public float timeAt64;

    AI_Controller ai_Controller;

    // Start is called before the first frame update
    void Start()
    {
        timeAt81 = 0f;
        timeAt24 = 0f;
        timeAt44 = 0f;
        timeAt64 = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeAt81 = ai_Controller.timeAt81;
        timeAt24 = ai_Controller.timeAt24;
        timeAt44 = ai_Controller.timeAt44;
        timeAt64 = ai_Controller.timeAt64;
    }
}
