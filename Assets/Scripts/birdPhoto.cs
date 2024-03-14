using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class birdPhoto : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;
    [SerializeField] private Text prompt;

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
            if (Input.GetKeyDown(KeyCode.E))
            {
                // pick up the photo
                if (!photoCollected)
                {
                    GameManager.Instance.AddItem("SpringPhoto");
                    photoCollected = true;
                    GameManager.Instance.birdPhotoFind = true;
                    GameManager.Instance.PuzzlePhotoActive("picnic");
                    //GameManager.Instance.FamilyPhotoActive("dog");
                    //show ending camera
                    //GameManager.Instance.TurnOnAndCloseOnceReachEnd("dog");
                    Destroy(gameObject);
                }
            }
            prompt.enabled = true;
        }
        else
        {
            prompt.enabled = false;
        }
    }
}
