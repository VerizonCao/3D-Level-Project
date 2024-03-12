using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

    [Header(" - Designer Variable - ")]
    [SerializeField] AnimationCurve heightToDistance;
    [SerializeField] float defaultDistance = 6;

    PlayerControls PIC;
    Vector2 movementInput;
    Vector2 movementSpeed = new Vector2(120f, 36f);

    [Header(" - Debug Info - ")]
    [SerializeField] private Vector2 spherePosition = new Vector2(-90, 26.25f);
    const float MAX_Y_ANGLE = 80f;
    const float MIN_Y_ANGLE = 25f;
    Vector2Int axisInversion = new Vector2Int(1, -1);

    private void Awake() {
        PIC = new PlayerControls();
        PIC.PlayerInput.CameraMove.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
    }

    private void OnEnable() {
        PIC.Enable();
    }
    private void OnDisable() {
        PIC.Disable();
    }
    
    void LateUpdate() {
        spherePosition += new Vector2(
            movementInput.x * movementSpeed.x * axisInversion.x * Time.deltaTime,
            movementInput.y * movementSpeed.y * axisInversion.y * Time.deltaTime
        );

        Debug.Log("camera speed, x: " + movementInput.x + " y : " + movementInput.y);

        // Clamps and Loops
        if (spherePosition.x > 180) //spherePosition += Vector2.right * -360;
            spherePosition = new Vector2(spherePosition.x - 360, spherePosition.y);
        if (spherePosition.x < -180)
            spherePosition = new Vector2(spherePosition.x + 360, spherePosition.y);
        spherePosition = new Vector2(
            spherePosition.x,
            Mathf.Clamp(spherePosition.y, MIN_Y_ANGLE, MAX_Y_ANGLE)
        );

        // Convert Sphere Position to a vector
        Vector3 actualPosition = Vector3.zero;
        float xzPlaneRad = Mathf.Deg2Rad * spherePosition.x;
        float yRad = Mathf.Deg2Rad * spherePosition.y;
        float latitudePercent = spherePosition.y / 90f;
        Vector3 offsetDirection = new Vector3(
            Mathf.Sin(xzPlaneRad) * Mathf.Cos(yRad),
            latitudePercent,
            Mathf.Cos(xzPlaneRad) * Mathf.Cos(yRad)
        ).normalized;

        Vector3 offsetVector = offsetDirection * defaultDistance
            * heightToDistance.Evaluate(spherePosition.y / MAX_Y_ANGLE);
        actualPosition += offsetVector;
        transform.localPosition = actualPosition;
        //transform.LookAt()
    }
}
