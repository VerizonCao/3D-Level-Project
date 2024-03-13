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

                GameManager.Instance.playerPosition = player.transform.position;


                GameManager.Instance.playerSpeaking.text = "Remember I used to do this a lot in summer night";
                GameManager.Instance.playerSpeaking.enabled = true;


                StartCoroutine(WaitAndExecute(() =>
                {
                    GameManager.Instance.playerSpeaking.enabled = false;
                    GameManager.Instance.SwitchSeason(GameManager.Season.Summer);
                }, 5f));

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
