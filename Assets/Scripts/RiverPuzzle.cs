using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RiverPuzzle : MonoBehaviour
{
    [SerializeField] private DetectionCircle detectionCircle;

    [SerializeField] private bool puzzleSolved = false;


    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            //Puzzle logic 2
            if (!puzzleSolved)
            {
                GameManager.Instance.Dialog("riverNotSolved");
            }
            if (puzzleSolved)
            {
                GameManager.Instance.Dialog("riverSolved");
                StartCoroutine(DestroyItem());
                return;
            }

        }
    }
    private IEnumerator DestroyItem()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    private void SolvePuzzle()
    {
        puzzleSolved = true;
    }

}
