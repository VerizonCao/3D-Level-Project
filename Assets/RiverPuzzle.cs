using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiverPuzzle : MonoBehaviour
{
    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] Text uppertext;

    [SerializeField] private bool puzzleSolved = false;

    //[SerializeField] GameObject RiverWallToDestroy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            //Puzzle logic 2
            if (!puzzleSolved)
            {
                uppertext.text = "You need winter boots to path through the river";
                uppertext.enabled = true;
            }
            if (puzzleSolved)
            {
                uppertext.text = "You wear on the boots, you can go through the river now!";
                uppertext.enabled = true;
                StartCoroutine(DestroyItem());
                return;
            }

        }
        else
        {
            uppertext.enabled = false;
        }
    }
    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(2);
        uppertext.enabled = false;
        Destroy(gameObject);
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;
    }
    private void OnDestroy()
    {
        
    }
}
