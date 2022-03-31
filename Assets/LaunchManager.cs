using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Attached to LaunchManager gameobject under MainMenu scene
public class LaunchManager : MonoBehaviour
{
    public InputField playerName;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start() PlayerPrefs.GetString(PlayerName) " + PlayerPrefs.GetString("PlayerName"));

        if (PlayerPrefs.HasKey("PlayerName"))
        {
            //Debug.Log("Start() if() PlayerPrefs.GetString(PlayerName) " + PlayerPrefs.GetString("PlayerName"));
            playerName.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetName(string name)
    {
        //Debug.Log("SetName() playerName.text " + playerName.text);
        name = playerName.text;
        //Debug.Log("SetName() name " + name);
        PlayerPrefs.SetString("PlayerName", name);
        //Debug.Log("SetName() PlayerPrefs.GetString(PlayerName) " + PlayerPrefs.GetString("PlayerName"));
    }

    public void ConnectSinglePlayer()
    {
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
