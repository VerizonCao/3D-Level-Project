using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    enum playerState {
        grounded,
        jumping,
        falling
    }
    enum AnimState
    {
        idle = 0,
        walking = 1,
        jumping = 2,
        pickFruit = 3,
        petAnimal = 4,
        sitting = 5,
    }
    [SerializeField] playerState currentState = playerState.falling;
    [SerializeField] AnimState animState = AnimState.idle;

    public float currentMomentum = 0f;
    public float currentAcceleration = 0f;

    [Header(" - Designer Variables - ")]
    [SerializeField] private float baseMomentum = 6;
    [SerializeField] private float maxMomentum = 12;
    [SerializeField] private float groundAcceleration = 1;
    [SerializeField] private float gravityStrength = 32;
    [SerializeField] private float onRoofGravityStrength = 32;
    bool onRoof = false;
    [SerializeField] private float maxJumpForce = 150;
    [SerializeField] private float maxJumpForceOnRoof = 170;
    [SerializeField][Range(0f, 1f)] private float groundFriction = 0.98f;
    [SerializeField][Range(0f, 1f)] private float airFriction = 0.95f;

    [SerializeField][Range(0f, 1f)] private float groundFrictionOnRoof = 0.8f;
    [SerializeField][Range(0f, 1f)] private float airFrictionOnRoof = 0.8f;


    [Header(" - References - ")]
    [SerializeField] private GameObject playerCameraPrefab;
    [Space]
    [SerializeField] public Rigidbody rb;
    [SerializeField] private Transform phantomCamera;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform lookAtPoint;
    public Transform modelContainer;
    [Space]
    [SerializeField] LayerMask groundLayers; // just includes the Ground layer

    // Camera Variables
    private GameObject playerCamera;
    private PlayerCameraController pcc;

    // 1st Camera 
    // This is only used in certain game scenary.
    // dont allow move for now.
    [SerializeField] Camera FirstViewCamera;
    [SerializeField] AudioListener firstViewAudio;
    private Vector2 lookInput;
    private float lookSpeed = 2f;
    private float cameraPitch = 0f;
    public float firstCameraVerticalMax = 90f;
    public float firstCameraVerticalMin = -90f;
    [SerializeField] bool useFirstViewCamera = false;
    public bool lockFirstCamera = false;

    // Input Variables
    PlayerControls PIC;
    bool jumpHold = false;
    float jumpForce;
    Vector2 movementInput;
    Vector3 cameraForward;
    Vector3 cameraRight;
    Vector3 dirVector;

    [SerializeField] Animator animator;
    [SerializeField] AnimationClip jumpClip;


    [Header(" - Debug - ")]
    public bool stopGravity = false;


    private void Awake() {
        PIC = new PlayerControls();
        PIC.PlayerInput.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        PIC.PlayerInput.Jump.started += ctx => print("JUMP START");
        PIC.PlayerInput.CameraMove.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        SpawnPlayerCamera();
    }
    // Enable/Disable Input Gathering
    private void OnEnable() {
        PIC.Enable();
    }
    private void OnDisable() {
        PIC.Disable();
    }


    private void Start()
    {
        
    }

    public void switchCamera(bool isFirstView)
    {
        useFirstViewCamera = isFirstView;
        if (useFirstViewCamera)
        {
            FirstViewCamera.enabled = true;
            firstViewAudio.enabled = true;
            playerCamera.SetActive(false);
        }
        else
        {
            // rotate the player to 0,0,0
            transform.rotation = Quaternion.identity;
            FirstViewCamera.enabled = false;
            firstViewAudio.enabled = false;
            playerCamera.SetActive(true);
        }
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

        // switch between first view / 3rd view
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Switch to camera mode " + (useFirstViewCamera ? "firstVuew" : "thirdView"));
            useFirstViewCamera = !useFirstViewCamera;
            switchCamera(useFirstViewCamera);
        }


    }

    public void switchToAminNumber(int number)
    {
        animState = (AnimState)number;
        animator.SetInteger("PlayerState", (int)animState);
    }

    private void changePlayerState(playerState state)
    {
        currentState = state;
    }

  

    private void FixedUpdate() {

        if (useFirstViewCamera)
        {
            if (lockFirstCamera)
            {
                return;
            }
            // Horizontal rotation
            transform.Rotate(Vector3.up * lookInput.x * lookSpeed);


            // Vertical rotation
            cameraPitch -= lookInput.y * lookSpeed;
            cameraPitch = Mathf.Clamp(cameraPitch, firstCameraVerticalMin, firstCameraVerticalMax);
            FirstViewCamera.transform.localEulerAngles = Vector3.right * cameraPitch;

            return;
        }


        // Find the camera's forward and right vector on the XZ plane to figure out
        // how to move the player in the direction indicated by the input collected
        cameraForward = Vector3.ProjectOnPlane(playerCamera.transform.forward, transform.up);
        cameraRight = Vector3.ProjectOnPlane(playerCamera.transform.right, transform.up);
        dirVector = Vector3.Normalize(
            cameraForward * movementInput.y + 
            cameraRight * movementInput.x
        );
        Debug.DrawLine(
            playerCamera.transform.position,
            playerCamera.transform.position + dirVector * 10,
            Color.blue
        );

        if (dirVector != Vector3.zero && animState != AnimState.jumping)
        {
            if (animState == AnimState.jumping)
            {
                // still do the jump anim, don't switch to walk
            }
            else
            {
                // if the player is not jumping, we play walking anim
                animState = AnimState.walking;
                animator.SetInteger("PlayerState", (int)animState);
            }
            
        }
        else if (dirVector == Vector3.zero && animState != AnimState.jumping && animState != AnimState.sitting)
        {
            // if the player is not jumping, we play idle anim
            animState = AnimState.idle;
            animator.SetInteger("PlayerState", (int)animState);
        }

        //don't add force if the player is still in jump anim
        if (animState == AnimState.jumping && currentState == playerState.grounded)
        {

        }
        else
        {
            // Apply Movement
            rb.AddForce(dirVector * currentMomentum * 2, ForceMode.Force);
        }
        



        // Current State Logic
        switch (currentState) {
            case playerState.grounded:
                if (!IsOnGround()) {
                    changePlayerState(playerState.falling);
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
                    changePlayerState(playerState.falling);
                }
                break;
            case playerState.falling:
                if (IsOnGround()) {
                    changePlayerState(playerState.grounded);
                }
                break;
        }

        // Applying Gravity
        if (!stopGravity)
        {
            if (onRoof)
            {
                rb.AddForce(Vector3.down * onRoofGravityStrength, ForceMode.Force);
            }
            else
            {
                rb.AddForce(Vector3.down * gravityStrength, ForceMode.Force);
            }
            
        }
        

        // ----- Applying Friction Forces
        // Ground Friction
        if (currentState == playerState.grounded) {
            if (onRoof)
            {
                rb.velocity *= groundFrictionOnRoof;
            }
            else
            {
                rb.velocity *= groundFriction;
            }
            
        }
        // Air Friction
        else { // Only applying air friction to lateral velocity (not up or down, on the XZ plane instead)
            Vector3 lateralVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.up);
            Vector3 verticalVelocity = rb.velocity - lateralVelocity;

            if (onRoof)
            {
                lateralVelocity *= airFrictionOnRoof;
            }
            else
            {
                lateralVelocity *= airFriction;
            }
            
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

            int groundLayerIndex = LayerMask.NameToLayer("Ground");
            int roofLayerIndex = LayerMask.NameToLayer("Roof");

            // Check if the hit.collider's gameObject is in the ground layer
            if (hit.collider.gameObject.layer == groundLayerIndex)
            {
                // Do something specific for ground
                onRoof = false;
            }
            // Check if the hit.collider's gameObject is in the roof layer
            else if (hit.collider.gameObject.layer == roofLayerIndex)
            {
                onRoof = true;
                // Do something specific for roof
                // we  -0.2 as the distance to roof
                float roofDistance = hit.distance;
                roofDistance = Mathf.Max(0f, roofDistance - 1f);
                return roofDistance;
            }


            return hit.distance;
        }

        return maxDistance;
    }

    private bool IsOnGround() {
        //float groundDistanceCutoff = 0.05f;
        float groundDistanceCutoff = 0.3f;
        float dis2Ground = GetDistanceToGround();
        return dis2Ground <= groundDistanceCutoff;
    }

    private bool CanJump() {
        return currentState == playerState.grounded;
    }

    private void StartJump() {
        
        StartCoroutine(JumpDuration());
        StartCoroutine(EnforceJumpForce());
    }

    private IEnumerator EnforceJumpForce()
    {
        yield return new WaitForSeconds(0.4f);
        changePlayerState(playerState.jumping);
        jumpHold = true;
        if (onRoof)
        {
            jumpForce = maxJumpForceOnRoof;
        }
        else
        {
            jumpForce = maxJumpForce;
        }
        
    }

    private IEnumerator JumpDuration()
    {
        // change player anim to jump
        animState = AnimState.jumping;

        animator.SetInteger("PlayerState", (int)animState);
        yield return new WaitForSeconds(jumpClip.length / 2.2f);  //1.2f is hardcoded now. 

        animState = AnimState.idle;
        animator.SetInteger("PlayerState", (int)animState);
    }

    public bool ReceivingMovementInput() {
        return movementInput.magnitude != 0;
    }
}
