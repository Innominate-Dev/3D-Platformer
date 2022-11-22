using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wall Running")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer; 

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift; // For the player to climb up or down
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    public KeyCode jumpKey = KeyCode.Space;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    public bool exitingWall;
    private float exitWallTimer;
    public float exitWallTime;

    [Header("References")]
    public Transform orientation;
    private PlayerController pc;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }

    private void Update()
    {
        CheckForWall(); // Repeatedly calls the both of these methods to always check
        StateMachine();
    }

    private void FixedUpdate()
    {
        if (pc.WallRunning)
            WallRunningMovement(); // Constantly runs this since this is a physics calculation
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        ////////// GETS PLAYER INPUT //////////////
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey); //// To make the player go up or down the wall at the moment is Left Shift and Lft Ctrl

        // Phase 1 - Checks if we are wall running
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            if (!pc.WallRunning)
                StartWallRun();
            
            if(Input.GetKeyDown(jumpKey)) WallJump(); //If the player presses space then they will wall jump, it requests the method.
        }
        
        else if(exitingWall)
        {
            if(pc.WallRunning)
            {
                StopWallRun();
            }

            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }

            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }

        // Phase 3 - Checks if we are on the wall if not then it will stop
        else
        {
            if (pc.WallRunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pc.WallRunning = true;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        /////////////// THIS MAKES THE PLAYER ONLY GO FORWARD /////////////////
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        ///////////////////// Makes the player go up the wall /////////////////////////
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        ///////////////// THIS PUSHES THE PLAYER TOWARDS THE WALL /////////////////
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pc.WallRunning = false;
    }

    private void WallJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //////////// Jumping [Adding force] //////////
        rb.velocity = new Vector3(rb.velocity.x, 0f,rb.velocity.z); /// THIS RESETS THE Y VELOCITY SO the player doesn't go flying off the wall
        rb.AddForce(forceToApply, ForceMode.Impulse); // Creates a impulse/outburst of force for the player to jump off the wall
    }
}
