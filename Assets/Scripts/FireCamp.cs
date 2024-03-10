using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FireCamp : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;
    [SerializeField] private Text prompt;
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    IEnumerator WaitAndExecute(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {

            // player enter the 1st camera and switch scene
            if (Input.GetKeyDown(KeyCode.E))
            {
                //move to 1st Camera
                player.switchCamera(true);

                // switch to winter
                // still use spring as we 
                StartCoroutine(WaitAndExecute(() => GameManager.Instance.SwitchSeason(GameManager.Season.Spring), 5f));
            }

            prompt.enabled = true;
        }
        else
        {
            prompt.enabled = false;
        }

    }
}