using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timeStamp4Way : MonoBehaviour
{
    //public float[] timeStamps; // 7/06 Trfc Ctrl
    //public ArrayList timeStamps = new ArrayList(4);

    //public float[] timeStamps = new float[4]; // 7/08 Trfc Ctrl, Array

    //public List<float> timeStamps = new List<float>(); // 7/13 Trfc Ctrl, List
    //public List<float> timeStamps2 = new List<float>(); // 7/12 Trfc Ctrl, List

    public float timeAt81; // 6/30 Trfc Ctrl
    public float timeAt24;
    public float timeAt44;
    public float timeAt64;

    //AI_Controller ai_Controller;

    // Start is called before the first frame update
    void Start()
    {
        //ArrayList timeStamps = new ArrayList();

        //timeAt81 = 1000f; // 7/12 Trfc Ctrl Test Codes
        //timeAt24 = 1000f;
        //timeAt44 = 1000f;
        //timeAt64 = 1000f;

        timeAt81 = 0f; // 7/13 Trfc Ctrl Test Codes
        timeAt24 = 0f;
        timeAt44 = 0f;
        timeAt64 = 0f;

        //timeStamps = new List<float> { timeAt81, timeAt24, timeAt44, timeAt64 }; // 7/13 Trfc Ctrl, List
        //timeStamps.Sort(); // 7/12 Trfc Ctrl, List
    }

    // Update is called once per frame
    void Update()
    {
        //timeAt81 = ai_Controller.timeAt81; // 6/30 Trfc Ctrl
        //timeAt24 = ai_Controller.timeAt24;
        //timeAt44 = ai_Controller.timeAt44;
        //timeAt64 = ai_Controller.timeAt64;

        //timeStamps = new float[4] { timeAt81, timeAt24, timeAt44, timeAt64 }; // 7/08 Trfc Ctrl, Array

        //timeStamps = new List<float> { timeAt81, timeAt24, timeAt44, timeAt64 }; // 7/12 Trfc Ctrl, List

        //for (int i = 0; i < timeStamps.Length; i++)
        //{
        //    Debug.Log("timeStamps[" + i + "]: " + timeStamps[i]);
        //}

        //System.Array.Sort(timeStamps); // 7/08 Trfc Ctrl

        //timeStamps.Sort(); // 7/12 Trfc Ctrl, List

        //timeStamps2 = new List<float> { timeStamps[1], timeStamps[2], timeStamps[3] }; // 7/12 Trfc Ctrl, List
        //timeStamps2[0] = timeStamps[1];
        //timeStamps2[1] = timeStamps[2];
        //timeStamps2[2] = timeStamps[3];

        //timeStamps.Reverse(); // 7/12 Trfc Ctrl, List

        //System.Array.Reverse(timeStamps);
        //for (int i = 0; i < timeStamps.Length; i++)
        //{
        //    Debug.Log("timeStamps[" + i + "] after Sort / Reverse: " + timeStamps[i]);
        //}
    }
}
