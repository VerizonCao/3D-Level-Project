using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{

    public DetectionCircle detectionCircle;
    public Text prompt;

    // Start is called before the first frame update
    void Start()
    {
        prompt.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            if (GameManager.Instance.gameEnd)
            {
                GameManager.Instance.door.text = "Hi, kid, welcome home";

                //show end button 
                prompt.enabled = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //ending

                    //switch to scene end 
                    GameManager.Instance.disableAll();
                    SceneManager.LoadScene("end");
                }
            }
            else
            {
                GameManager.Instance.door.text = "I cant open this door.....";
            }
            
            GameManager.Instance.door.enabled = true;
        }
        else
        {
            GameManager.Instance.door.enabled = false;
        }
    }
}
