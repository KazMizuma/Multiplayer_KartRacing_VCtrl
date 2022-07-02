using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Attached to Main Camera
public class SmoothFollow : MonoBehaviour
{
    Transform[] target;
    public float distance = 4.0f;
    public float height = 2.0f;
    public float heightOffset = 0f;
    public float heightDamping = 4.0f;
    public float rotationDamping = 2.0f;
    public RawImage rearCamView;
    int index = 0; 

    int FP = -1;

    void Start()
    {
        if (PlayerPrefs.HasKey("FP"))
        {
            FP = PlayerPrefs.GetInt("FP");
        }

        //if (PlayerPrefs.HasKey("TP")) // 3/25 added for Target Player PlayerPrefs
        //{
        //    index = PlayerPrefs.GetInt("TP");
        //}

        switch (PlayerPrefs.GetInt("CarSelection"))
        {
            case 0: // Red
                index = 0;
                Debug.Log("SmoothFollow index " + index);
                break;
            case 1: // Yellow
                index = 4;
                Debug.Log("SmoothFollow index " + index);
                break;
            case 2: // Purple
                index = 3;
                Debug.Log("SmoothFollow index " + index);
                break;
            case 3: // Green
                index = 2;
                Debug.Log("SmoothFollow index " + index);
                break;
            default:
                index = 0;
                Debug.Log("SmoothFollow index " + index);
                break;
        }
    }

    void LateUpdate()
    {
        if (target == null)
        {
            GameObject[] cars = GameObject.FindGameObjectsWithTag("Car");
            target = new Transform[cars.Length];
            for (int i = 0; i < cars.Length; i++)
            {
                target[i] = cars[i].transform;
                //Debug.Log(cars[i].name + " " + i); // 7/01 Trfc Ctrl Test Code
                Debug.Log(cars[i].transform.parent.name + "'s index is " + i);
            }
            target[index].Find("RearViewCam").gameObject.GetComponent<Camera>().targetTexture = 
                                                            (rearCamView.texture as RenderTexture);
            return;
        }

        if (FP == 1)
        {
            transform.position = target[index].position + target[index].forward * 0.4f + target[index].up;
            transform.LookAt(target[index].position + target[index].forward * 3f);
        }
        else
        {
            float wantedRotationAngle = target[index].eulerAngles.y;
            float wantedHeight = target[index].position.y + height;

            float currentRotationAngle = transform.eulerAngles.y;
            float currentHeight = transform.position.y;

            currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
            currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

            Quaternion currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

            transform.position = target[index].position;
            transform.position -= currentRotation * Vector3.forward * distance;

            transform.position = new Vector3(transform.position.x,
                                    currentHeight + heightOffset,
                                    transform.position.z);

            transform.LookAt(target[index]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FP *= -1;
            PlayerPrefs.SetInt("FP", FP);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            target[index].Find("RearViewCam").gameObject.GetComponent<Camera>().targetTexture = null;
            index++;
            //PlayerPrefs.SetInt("TP", index); // 3/25 added for Target Player PlayerPrefs
            if (index > target.Length - 1) index = 0;
            target[index].Find("RearViewCam").gameObject.GetComponent<Camera>().targetTexture = 
                                                              (rearCamView.texture as RenderTexture);
        }
    }
}
