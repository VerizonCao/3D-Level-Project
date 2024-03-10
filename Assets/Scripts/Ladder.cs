using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ladder : MonoBehaviour
{

    [SerializeField] private DetectionCircle detectionCircle;
    [SerializeField] private Text prompt;
    [SerializeField] private Transform ladderStart;
    [SerializeField] private Transform ladderEnd;
    [SerializeField] private PlayerController player;

    private bool isMoving = false;

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
                // switch to 1d camera
                player.switchCamera(true);
                // climb to the roof
                player.transform.localPosition = ladderStart.position;
                isMoving = true;
                //return to 3d
            }

            prompt.enabled = true;
        }
        else
        {
            prompt.enabled = false;
        }

        if (isMoving)
        {
            float speed = 2f;  //hardcode for now
            player.transform.localPosition = Vector3.MoveTowards(player.transform.localPosition, ladderEnd.position, Time.deltaTime * speed);
            if (Vector3.Distance(player.transform.position, ladderEnd.position) < 1f)
            {
                // we now arrive
                isMoving = false;
                player.switchCamera(false);
            }
        }
    }
}
