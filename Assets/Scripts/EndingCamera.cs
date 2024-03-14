using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndingCamera : MonoBehaviour
{

    [SerializeField] Camera _camera;
    [SerializeField] Transform start;
    [SerializeField] Transform end;

    List<int> seasonsPuzzle = new List<int>();

    private bool isMoving = false;

    //public float rotationSpeed = 5.0f; // Speed of rotation

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            float speed = 2f;  //hardcode for now

            _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, end.position, Time.deltaTime * speed);

            //// Determine the target rotation. This could be towards a target object or a specific Quaternion
            //Quaternion targetRotation = Quaternion.LookRotation(end.position - _camera.transform.position);

            //// Rotate the camera towards the target rotation
            //_camera.transform.rotation = Quaternion.RotateTowards(_camera.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    IEnumerator WaitAndExecute(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    IEnumerator Work(string photo)
    {

        seasonsPuzzle.Add(1);

        bool gameFinish = false;

        if (seasonsPuzzle.Count == 4)
        {
            // finish all puzzles, light the photo and switch to lighting ending.
            gameFinish = true;
        }

        _camera.transform.position = start.position;
        //_camera.transform.rotation = start.rotation;

        PlayerController player = FindObjectOfType<PlayerController>();

        // switch to ending camera
        player.closePlayerCamera();
        _camera.enabled = true;

        isMoving = true;

        while (Vector3.Distance(_camera.transform.position, end.position) > 2f)
        {
            yield return new WaitForSeconds(0.2f);
        }

        GameManager.Instance.FamilyPhotoActive(photo);


        while (Vector3.Distance(_camera.transform.position, end.position) > 0.5f)
        {
            yield return new WaitForSeconds(0.2f);
        }


        isMoving = false;

        if (gameFinish)
        {
            GameManager.Instance.changePhotoEndingOpen();
        }
        yield return new WaitForSeconds(1.5f);
        if (gameFinish)
        {
            GameManager.Instance.changePhotoEndingClose();
        }
        yield return new WaitForSeconds(0.5f);
        // switch back to player camera
        player.openPlayerCamera();
        _camera.enabled = false;
        _camera.transform.position = start.position;
        //_camera.transform.rotation = start.rotation;
        GameManager.Instance.gameEndLight();
    }

    public void TurnOnAndCloseOnceReachEnd(string photo)
    {

        StartCoroutine(Work(photo));
    }
}
