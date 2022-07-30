using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to RaceMonitor gameobject
public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;

    // 7/30 Trfc Ctrl TEST
    public static bool racing = false;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (GameObject gameObj in countDownItems)
        //{
        //    gameObj.SetActive(false);
        //}

        //StartCoroutine(PlayCountDown());

        // 7/30 Trfc Ctrl TEST, avoiding NullReferenceException due to ai_controller.targetAngleTrfc not determined before Start()
        racing = true;
    }

    IEnumerator PlayCountDown()
    {
        yield return new WaitForSeconds(1);
        foreach (GameObject gameObj in countDownItems)
        {
            gameObj.SetActive(true);
            yield return new WaitForSeconds(1);
            gameObj.SetActive(false);
        }

        // 7/30 Trfc Ctrl TEST
        racing = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
