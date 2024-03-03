using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;

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
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        
    }
}
