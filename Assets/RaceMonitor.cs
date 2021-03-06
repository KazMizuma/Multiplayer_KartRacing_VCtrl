using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to RaceMonitor gameobject
public class RaceMonitor : MonoBehaviour
{
    public GameObject[] countDownItems;
    public static bool racing = false;

    // Start is called before the first frame update
    void Start()
    {
        //foreach (GameObject gameObj in countDownItems)
        //{
        //    gameObj.SetActive(false);
        //}

        //StartCoroutine(PlayCountDown());
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
        racing = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
