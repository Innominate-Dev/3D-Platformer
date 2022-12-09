using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //////////////// VARIABLES ////////////////
    
    [Header("Movement")]
    public float moveSpeed; //////////////// GENERAL SPEED FOR PLAYER ////////////////////
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    bool readyToJump = true;

    public float wallRunSpeed;

    ///////// KEYBINDS /////////

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool isGrounded;

    [Header("Movement")]

    public Transform orientation; /////////////////// MOVE THE PLAYERS ORIENTATION ///////////////////
    public Transform Player;

    float playerrot; /// Player Rotation
    float horizontalInput; //////////////////// MOVEMENT OVERALL ////////////////
    float verticalInput; ////////////////////// HEIGHT OF JUMP //////////////////

    Vector3 moveDirection; ///////// DIRECTION THE PLAYER MOVES IN

    Rigidbody rb; /// PLAYER RIGID BODY
    Animator myAnim;

    /////////////////// MOVEMENT STATE ///////////////////


    public MovementState state;

    public enum MovementState
    {
        walking,
        sprinting,
        air,
        WallRunning
    }

    public bool WallRunning;



    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
        
    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        PlayerInput(); /////////// THIS IS TO CONSTANTLY CHECK ON THE PLAYER IF THEY PRESS DOWN A MOVEMENT KEY AND It is called every frame
        SpeedControl();
        StateHandler();
        //TiltWallRun();

        ////// Tilting the player if they wall run /////////////

        playerrot = Player.rotation.z;
        playerrot = 45f;


        ///// Getting the magnitude of the player for the animation /////////

        myAnim.SetFloat("speed", moveDirection.magnitude);
        Debug.Log(moveDirection.magnitude);

        // handle drag
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }

        if( WallRunning == false)
        {
            rb.useGravity = true;
        }
    }

    private void FixedUpdate() //// THIS METHOD IS BEING USED FOR THE PHYSICS CALCULATIONS SINCE FIXEDUPDATE CAN RUN SEVERAL TIMES In one frame.
    {
        MovePlayer();
    }

    /////////////// PLAYER INPUT METHOD ///////////////////////
    void PlayerInput()
    {
        /////////////// MOVEMENT SYSTEM //////////////////
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ////////////////// JUMP SYSTEM ///////////////////

        if(Input.GetKeyDown(jumpKey) && readyToJump == true && isGrounded == true) /// This checks if the player pressed space or what ever keybind they selected to jump
        {
            Debug.Log("Jumping");
            readyToJump = false; //Changes the bool status to false so the player can't spam 
            Jump(); // Calls the method

            Invoke(nameof(ResetJump), jumpCoolDown); // Calls the function Reset Jump and calls the variable JumpCoolDown.
        }
    }

    private void StateHandler()
    {


        // MODE - SPRINTING
        if(isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            myAnim.SetBool("Running", true);
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            myAnim.SetBool("Running", false);
        }
        // Mode - Wallrunning
        else if(WallRunning == true)
        {
            state = MovementState.WallRunning;
            moveSpeed = wallRunSpeed;
        }
        else if (!WallRunning)
        {
            state = MovementState.air;
            myAnim.SetBool("Running", false);
        }

    }

    private void MovePlayer()
    {
        // CALCULATES THE MOVEMENT DIRECTION

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        //WHEN ON THE GROUND
        if(isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed* 10f, ForceMode.Force); //// THIS ADDS FORCE TO THE RIGID BODY AND MAKES THE PLAYER MOVE IN THE DIRECTION PRESSED IN
        }

        else if(!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // IF THE PLAYER IS IN THE AIR AND NOT TOUCHING THE GROUND
        }
    }
    
    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if(flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        ///////// RESET Y VELOCITY /////////////////
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void TiltWallRun()
    {
        if (WallRunning == true)
        {
            Player.transform.rotation = Quaternion.Euler(new Vector3(playerrot, Player.transform.rotation.eulerAngles.y, Player.transform.rotation.eulerAngles.y));
        }
        
    }
}
