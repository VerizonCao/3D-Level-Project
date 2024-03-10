using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Swing : MonoBehaviour
{
    [SerializeField] private DetectionCircle detectionCircle;
    [SerializeField] private Text prompt;
    [SerializeField] private PlayerController player;
    [SerializeField] private Transform playerSitPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {

            // player enter the 1st camera and switch scene
            if (Input.GetKeyDown(KeyCode.E))
            {
                setPlayerOnSwing();
            }

            prompt.enabled = true;
        }
        else
        {
            prompt.enabled = false;
        }
    }

    IEnumerator WaitAndExecute(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    public void setPlayerOnSwing()
    {
        player.transform.SetParent(transform);
        player.stopGravity = true;
        player.transform.localPosition = playerSitPosition.localPosition;
        player.modelContainer.transform.localRotation = playerSitPosition.localRotation;

        player.firstCameraVerticalMax = 20;
        player.switchCamera(true);
        //switch player anim to sit
        // for which reason, bugged, so ignore for now
        //player.switchToAminNumber(5);
        GameManager.Instance.playerPosition = player.transform.position;
        StartCoroutine(WaitAndExecute(() => GameManager.Instance.SwitchSeason(GameManager.Season.Spring), 2f));
        
    }

    public void removePlayerOnSwing()
    {
        player.transform.SetParent(null);
        player.stopGravity = false;
        //player.transform.localPosition = playerSitPosition.localPosition;
        //player.modelContainer.transform.localRotation = playerSitPosition.localRotation;

        player.firstCameraVerticalMax = 90;
        player.switchCamera(false);
        //switch player anim to sit
        // for which reason, bugged, so ignore for now
        //player.switchToAminNumber(5);
    }
}
