using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Attached to each Choice# gameobject under MainMenu scene
public class SpinPlatform : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 1, 0);
    }
}
