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

    [SerializeField] private bool puzzleSolved = false;
    private bool photoCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {

            // for puzzle
            if (GameManager.Instance.getCurSeason() == GameManager.Season.Fall)
            {
                // show ui to interact.
                if (!puzzleSolved)
                {
                    uppertext.text = "Hi, I am Scarecrow, can you bring me a flower?";
                    uppertext.enabled = true;
                }
                if (puzzleSolved)
                {
                    uppertext.text = "Thank you!";
                    uppertext.enabled = true;
                    if (!photoCollected)
                    {
                        GameManager.Instance.AddItem("Photo1");
                        photoCollected = true;
                    }

                }
            }
  

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


    private void SolvePuzzle()
    {
        puzzleSolved = true;
    }

}
