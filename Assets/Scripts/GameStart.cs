using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{

    [SerializeField] Image image1;
    [SerializeField] Image image2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Opening2());
    }

    IEnumerator Opening2()
    {
        yield return new WaitForSeconds(5);
        //switch to image 2
        image1.enabled = false;
        image2.enabled = true;
        StartCoroutine(SwitchToFall());
    }

    IEnumerator SwitchToFall()
    {
        yield return new WaitForSeconds(5);
        //switch to image 2
        image2.enabled = false;
        SceneManager.LoadScene("_Fall");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}