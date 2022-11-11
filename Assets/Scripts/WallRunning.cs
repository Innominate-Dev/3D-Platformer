using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wall Running")]

    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallRunForce;
    public float MaxWallRunTime;

    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;
    public KeyCode jumpKey = KeyCode.Space;


    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;

    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

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
        CheckForWall();
        StateMachine();
    }

    private void FixedUpdate()
    {
        if(pc.WallRunning)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallhit, wallCheckDistance, whatIsWall); //Invisible ray to check if there is a wall
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallhit, wallCheckDistance, whatIsWall);

        Debug.DrawLine(transform.position, transform.position + (orientation.right * wallCheckDistance), Color.green);
        Debug.DrawLine(transform.position, transform.position - (orientation.right * wallCheckDistance), Color.blue);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround); // If the player is high enough to do a wall run
    }

    private void StateMachine()
    {
        ///////// INPUT FROM THE PLAYER //////////////

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Phase 1 - Wall running

        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround() && !exitingWall)
        {
            /////////////// WALL RUNNING ///////////////
            if(!pc.WallRunning)
            {
                if(!pc.WallRunning)
                {
                    StartWallRun();
                }

                if(Input.GetKeyDown(jumpKey)) WallJump();
            }

            //Phase 2 - Jumping from wall state
            else if(exitingWall == true)
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
                    Debug.Log("Wall is false");
                }
            }

            else
            {  
                if(pc.WallRunning)
                {
                    StopWallRun();
                }
            }
        }
    }

    private void StartWallRun()
    {
        Debug.Log("Beginning Wall RUN!");
        pc.WallRunning = true;
    }

    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal; //Cheap way of doing a if statement e.g if WallRight is true make right wall hit.normal true

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up); //Makes you go automatically forward

        ////////////// FORWARD FORCE ////////////
        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
            Debug.Log("MOVING FORWARDS!");

        }

        rb.AddForce(wallForward * wallRunForce, ForceMode.Force); // This adds a force to bring the player closer

        if(!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force); // This adds a force that 
            Debug.Log("MAKING YOU STICK TO THE SIDE");
        }
    }

    private void StopWallRun()
    {
        Debug.Log("Stopping Run");
        pc.WallRunning = false;
    }

    private void WallJump()
    {
        // Entering wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;
        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force to the player so they can jump
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
