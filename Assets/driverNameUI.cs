using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class driverNameUI : MonoBehaviour
{
    public Text driverName;
    public Transform targetCar;
    CanvasGroup canvasGroup;
    public Renderer carRenderer;

    // Awake is called when the script instanced is being loaded
    private void Awake()
    {
        this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);
        driverName = this.GetComponent<Text>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
    }

    void LateUpdate()
    {
        // if (Camera.main.fieldOfView(Canvas)) 
        // https://answers.unity.com/questions/172721/render-only-what-the-camera-sees.html
        // OnBecameVisible 
        this.transform.position = Camera.main.WorldToScreenPoint(targetCar.position);
    }
}
