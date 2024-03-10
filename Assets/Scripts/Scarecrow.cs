using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scarecrow : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] Text uppertext;

    [SerializeField] private PlayerController player;

    [SerializeField] private bool isActivated;
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
            uppertext.text = "Hi, I am Scarecrow, can you bring me a flower?";
            uppertext.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {
                // for puzzle
                if (GameManager.Instance.getCurSeason() == GameManager.Season.Fall)
                {

                }
                else   // switch season
                {
                    // switch to 1D
                    player.switchCamera(true);
                    // Anim
                    // switch season
                    GameManager.Instance.SwitchSeason(GameManager.Season.Fall);
                }
            }
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
