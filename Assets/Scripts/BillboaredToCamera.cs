using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboaredToCamera : MonoBehaviour
{
    [SerializeField] Transform cameraToFace;
    [SerializeField] RectTransform rect;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("here");
        if (cameraToFace == null)
        {
            try
            {
                cameraToFace = GameObject.FindGameObjectWithTag("MainCamera").transform;
            }
            catch {
                Debug.LogError("can't find a camera");
                gameObject.SetActive(false);
            }

        }
        
    }

    // Update is called once per frame
    void Update()
    {
        rect.LookAt(cameraToFace);
    }
}
