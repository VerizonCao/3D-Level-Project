using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour {

    [Header(" - Designer Variables - ")]
    //[SerializeField] [Range(0, 1)] private float lerpMoveSpeed = 0.5f;
    [SerializeField] LayerMask groundLayers; // just includes the Ground layer

    public Transform cameraTarget;
    public Transform lookAtPoint;
    public Transform player;
    
    void LateUpdate() {
        if (cameraTarget == null || lookAtPoint == null || player == null) 
            return;

        // ----- Camera Move to Target
        transform.position = cameraTarget.position;
        transform.LookAt(lookAtPoint.position, player.up);

        // ----- Simple Camera Collision
        RaycastHit hit;
        Debug.DrawLine(cameraTarget.position, lookAtPoint.position, Color.gray);
        if (Physics.Linecast(lookAtPoint.position, cameraTarget.position, out hit, groundLayers, QueryTriggerInteraction.Ignore)) {
            Debug.DrawLine(hit.point, lookAtPoint.position, Color.red);
            transform.position = hit.point;
            transform.LookAt(lookAtPoint.position, player.up);
        }
    }
}
