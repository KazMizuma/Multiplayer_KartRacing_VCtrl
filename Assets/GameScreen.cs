using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to GameScreen Canvas Group under Canvas
public class GameScreen : MonoBehaviour
{
    public GameObject miniMap;
    int miniMapOnOff = -1; // 6/30 Trfc Ctrl, Disabling MiniMap Upon Start

    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("miniMapSwitch", miniMapOnOff);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            miniMapOnOff = miniMapOnOff * -1;
            PlayerPrefs.SetInt("miniMapSwitch", miniMapOnOff);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerPrefs.GetInt("miniMapSwitch") == 1)
        {
            miniMap.SetActive(true);
        }
        else
        {
            miniMap.SetActive(false);
        }
    }
}
