using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WarningPlace : MonoBehaviour
{
    [SerializeField] private string warnText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }

    private void OnTriggerEnter(Collider other)
    {


        GameManager.Instance.warning.text = warnText;
        GameManager.Instance.warning.enabled = true;


    }

    private void OnTriggerExit(Collider other)
    {
        GameManager.Instance.warning.enabled = false;
    }
}
