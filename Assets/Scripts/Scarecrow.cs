using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scarecrow : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] Text uppertext;

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
                if(!photoCollected)
                {
                    GameManager.Instance.AddItem("Photo1");
                    photoCollected = true;
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
