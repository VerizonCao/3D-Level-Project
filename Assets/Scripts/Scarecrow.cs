using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scarecrow : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] TextMeshProUGUI uppertext;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            // show ui to interact.
            uppertext.text = "Hi, I am Scarecrow";
            uppertext.enabled = true;

        }
        else
        {
            uppertext.enabled = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}
