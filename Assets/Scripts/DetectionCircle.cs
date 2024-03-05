using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCircle : MonoBehaviour
{

    public bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("here triggered");
            isTriggered = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("here leave trigger");
            isTriggered = false;
        }
    }
}
