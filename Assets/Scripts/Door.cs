using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    public DetectionCircle detectionCircle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            GameManager.Instance.door.text = "I cant open this door.....";
            GameManager.Instance.door.enabled = true;
        }
        else
        {
            GameManager.Instance.door.enabled = false;
        }
    }
}
