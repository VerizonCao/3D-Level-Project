using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    enum playerState {
        grounded,
        jumping,
        falling
    }
    playerState currentState = playerState.falling;
    float currentMomentum = 0f;
    float currentAcceleration = 0f;

    [Header(" - Designer Variables - ")]
    [SerializeField] private float baseMomentum = 6;
    [SerializeField] private float maxMomentum = 12;
    [SerializeField] private float groundAcceleration = 1;
    [SerializeField] private float gravityStrength = 32;
    [SerializeField] private float maxJumpForce = 350;
    [SerializeField][Range(0f, 1f)] private float groundFriction = 0.97f;
    [SerializeField][Range(0f, 1f)] private float airFriction = 0.95f;

    [Header(" - References - ")]
    [SerializeField] private GameObject playerCameraPrefab;
    [Space]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform phantomCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform lookAtPoint;
    [SerializeField] private Transform modelContainer;
    [Space]
    [SerializeField] LayerMask groundLayers; // just includes the Ground layer

    // Camera Variables
    private GameObject playerCamera;
    private PlayerCameraController pcc;

    // Input Variables
    PlayerControls PIC;
    bool jumpHold = false;
    float jumpForce;
    Vector2 movementInput;
    Vector3 cameraForward;
    Vector3 cameraRight;
    Vector3 dirVector;

    private void Awake() {
        PIC = new PlayerControls();
        PIC.PlayerInput.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        PIC.PlayerInput.Jump.started += ctx => print("JUMP START");
        SpawnPlayerCamera();
    }
    // Enable/Disable Input Gathering
    private void OnEnable() {
        PIC.Enable();
    }
    private void OnDisable() {
        PIC.Disable();
    }

    void Update() {
        // Jump Start Press
        if (PIC.PlayerInput.Jump.WasPressedThisFrame() && CanJump()) {
            StartJump();
        }
        // Jump Release
        if (jumpHold && PIC.PlayerInput.Jump.WasReleasedThisFrame()) {
            jumpHold = false;
        }
    }

    private void FixedUpdate() {
        // Find the camera's forward and right vector on the XZ plane to figure out
        // how to move the player in the direction indicated by the input collected
        cameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, transform.up);
        cameraRight = Vector3.ProjectOnPlane(playerCamera.transform.right, transform.up);
        dirVector = Vector3.Normalize(
            cameraForward * movementInput.y + 
            cameraRight * movementInput.x
        );
        //Debug.DrawLine(
        //    playerCamera.transform.position,
        //    playerCamera.transform.position + dirVector * 10,
        //    Color.blue
        //);

        // Apply Movement
        rb.AddForce(dirVector * currentMomentum * 2, ForceMode.Force);

        // Current State Logic
        switch(currentState) {
            case playerState.grounded:
                if (!IsOnGround()) {
                    currentState = playerState.falling;
                }

                if (ReceivingMovementInput()) {
                    // Increase Momentum when Moving
                    currentAcceleration = groundAcceleration;
                    currentMomentum = Mathf.Clamp(currentMomentum + (currentAcceleration * movementInput.magnitude),
                        baseMomentum, maxMomentum);

                    // ----- Calculate Relevant Turning Angles
                    // cameraFacingAngle is relative to the world, hence the angle between Vector3.forward and cameraForward
                    float cameraFacingAngle = cameraForward.x > 0 ? Vector3.Angle(Vector3.forward, cameraForward) : -Vector3.Angle(Vector3.forward, cameraForward);
                    // playerFacingAngle is relative to the camera, hence the angle between cameraForward and dirVector
                    float playerFacingAngle = movementInput.x > 0 ? Vector3.Angle(cameraForward, dirVector) : -Vector3.Angle(cameraForward, dirVector);

                    // Actually Turn the Player to Face the Correct Direction
                    modelContainer.localRotation = Quaternion.Euler(0, cameraFacingAngle + playerFacingAngle, 0);
                }
                break;
            case playerState.jumping:
                float divideFactor = jumpHold ? 6 : 2;
                jumpForce = Mathf.Max(jumpForce - maxJumpForce/divideFactor, 0);
                rb.AddForce(transform.up * jumpForce, ForceMode.Force);
                // Jump End
                float minimumForce = 1;
                if (jumpForce < minimumForce) {
                    currentState = playerState.falling;
                }
                break;
            case playerState.falling:
                if (IsOnGround()) {
                    currentState = playerState.grounded;
                }
                break;
        }

        // Applying Gravity
        rb.AddForce(Vector3.down * gravityStrength, ForceMode.Force);

        // ----- Applying Friction Forces
        // Ground Friction
        if (currentState == playerState.grounded) {
            rb.velocity *= groundFriction;
        }
        // Air Friction
        else { // Only applying air friction to lateral velocity (not up or down, on the XZ plane instead)
            Vector3 lateralVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
            Vector3 verticalVelocity = rb.velocity - lateralVelocity;

            lateralVelocity *= airFriction;
            rb.velocity = lateralVelocity + verticalVelocity;
        }

        // ----- Velocity Deadzone - stop moving if moving super slowly and the player doesn't want to move
        if (!ReceivingMovementInput() && rb.velocity.sqrMagnitude < 0.35f) {
            rb.velocity = Vector3.zero;
        }

    }

    private void SpawnPlayerCamera() {
        // Redundancy Check
        if (playerCamera != null) {
            Debug.LogWarning("Trying to spawn a camera when we already have one!  Aborting!");
            return;
        }
        // Spawn Camera
        else {
            playerCamera = Instantiate(playerCameraPrefab, cameraTarget.position, transform.rotation);
            pcc = playerCamera.GetComponent<PlayerCameraController>();
            pcc.cameraTarget = cameraTarget;
            pcc.lookAtPoint = lookAtPoint;
            pcc.player = transform;
        }
    }

    private float GetDistanceToGround(float maxDistance = 5) {
        Vector3 startPosition = transform.position + Vector3.down * -0.01f;

        RaycastHit hit;
        if (Physics.Raycast(startPosition, Vector3.down, out hit, maxDistance, groundLayers, QueryTriggerInteraction.Ignore)) {
            return hit.distance;
        }

        return maxDistance;
    }

    private bool IsOnGround() {
        float groundDistanceCutoff = 0.05f;
        return GetDistanceToGround() <= groundDistanceCutoff;
    }

    private bool CanJump() {
        return currentState == playerState.grounded;
    }

    private void StartJump() {
        currentState = playerState.jumping;
        jumpHold = true;
        jumpForce = maxJumpForce;
    }

    public bool ReceivingMovementInput() {
        return movementInput.magnitude != 0;
    }
}
