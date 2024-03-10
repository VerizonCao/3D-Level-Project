using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SummerPoint : MonoBehaviour
{

    [SerializeField] private Text prompt;
    [SerializeField] private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        prompt.enabled = false;
    }

    IEnumerator WaitAndExecute(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    // Update is called once per frame
    void Update()
    {
        if (prompt.enabled)
        {
            // press E to switch season
            // player enter the 1st camera and switch scene
            if (Input.GetKeyDown(KeyCode.E))
            {
                //move to 1st Camera
                player.switchCamera(true);

                // switch to winter
                // still use spring as we 
                StartCoroutine(WaitAndExecute(() => GameManager.Instance.SwitchSeason(GameManager.Season.Spring), 5f));
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        // show UI press E
        prompt.enabled = true;

    }

    private void OnTriggerExit(Collider other)
    {
        prompt.enabled = false;
    }
}
