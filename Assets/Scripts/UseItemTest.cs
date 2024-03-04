using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemTest : MonoBehaviour
{
    [SerializeField] private GameObject interactUI;
    [SerializeField] private Transform targetObject;

    //[SerializeField] private ItemStatus itemStatus;

    public DetectionCircle detectionCircle;
    private Camera mainCamera;

    public string itemName;

    private void Update()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;

            if (mainCamera == null)
            {
                Debug.LogError("Main camera not found.");
                return;
            }
        }

        if (detectionCircle != null && detectionCircle.isTriggered)
        {
            if (interactUI != null)
            {
                interactUI.SetActive(true);

                if (targetObject != null)
                {
                    Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.position);
                    interactUI.transform.position = screenPos;
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.Instance.UseItem(itemName);

                Destroy(gameObject);
            }
        }
        else
        {
            if (interactUI != null)
            {
                interactUI.SetActive(false);
            }
        }
    }
}
