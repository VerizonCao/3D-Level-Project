using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] private PlayerController player;

    [SerializeField] private bool puzzleSolved = false;
    private bool photoCollected = false;

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            // for puzzle
            if (!puzzleSolved)
            {
                GameManager.Instance.Dialog("birdNotSolved");
            }
            if (puzzleSolved)
            {
                GameManager.Instance.Dialog("birdSolved");
                if (!photoCollected)
                {
                    GameManager.Instance.AddItem("WinterPhoto");
                    photoCollected = true;
                    GameManager.Instance.PuzzlePhotoActive("bird");
                    //GameManager.Instance.FamilyPhotoActive("dog");
                }

            }
        }
    }
    private void SolvePuzzle()
    {
        puzzleSolved = true;
    }
}
