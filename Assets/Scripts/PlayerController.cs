using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed; //////////////// GENERAL SPEED FOR PLAYER ////////////////////

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool isGrounded;

    [Header("Movement")]

    public Transform orientation; /////////////////// MOVE THE PLAYERS ORIENTATION ///////////////////

    float horizontalInput; //////////////////// MOVEMENT OVERALL ////////////////
    float verticalInput; ////////////////////// HEIGHT OF JUMP //////////////////

    Vector3 moveDirection; ///////// DIRECTION THE PLAYER MOVES IN

    Rigidbody rb; /// PLAYER RIGID BODY

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
        
    // Update is called once per frame
    void Update()
    {
        //Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        PlayerInput(); /////////// THIS IS TO CONSTANTLY CHECK ON THE PLAYER IF THEY PRESS DOWN A MOVEMENT KEY AND It is called every frame
        
        // handle drag
        if(isGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void FixedUpdate() //// THIS METHOD IS BEING USED FOR THE PHYSICS CALCULATIONS SINCE FIXEDUPDATE CAN RUN SEVERAL TIMES In one frame.
    {
        MovePlayer();
    }

    /////////////// PLAYER INPUT METHOD ///////////////////////
    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        // CALCULATES THE MOVEMENT DIRECTION

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed* 10f, ForceMode.Force); //// THIS ADDS FORCE TO THE RIGID BODY AND MAKES THE PLAYER MOVE IN THE DIRECTION PRESSED IN
    }
    

}
