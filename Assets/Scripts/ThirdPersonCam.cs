using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public bool combatMode = false;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;

    public CameraStyle currentStyle;

    public float CameraSpeed;
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //// Makes the cursor invisible to not obstruct the players view.
        combatMode = false;
    }

    private void Update()
    {
        //////////////////////// SWITCHES CAMERA MODES ////////////////////////
        if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == false)
        {
            Debug.Log("Moving to combat!");
            SwitchCameraStyle(CameraStyle.Combat);
            StartCoroutine(CombatModeActive());
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == true)
        {
            Debug.Log("Moving to basic");
            SwitchCameraStyle(CameraStyle.Basic); // THIS CALLS ANOTHER METHOD AND SWITCHES BETWEEN THE TWO CAMERA IN THE SCENE NAMED COMBAT AND BASIC
            StartCoroutine(BasicModeActive());// THIS CALLS THE METHOD/ENUMERATOR AND ASKS FOR INSTRUCTIONS. IT THEN WAITS 5 SECONDS THEN CHANGES THE BOOL TO FALSE AS THE PLAYER SWITCHES TO BASIC MODE
        }

        ///////////////////////// ROTATATION ORIENTATION ///////////////////////////////////
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //////////////////////////////////////// ROTATES PLAYER OBJECT /////////////////////////////
        if(currentStyle == CameraStyle.Basic) /// This code rotates the player object to follow the players mouse
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, CameraSpeed * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    IEnumerator CombatModeActive() /////// THIS Enumerator IS LIKE A METHOD THAT ALLOWS ME TO DELAY THE FUNCTION WHEN THE PLAYER CALLS IT IN VOID UPDATE. So the function of what it is, in this case 
    {                     /////  it switch camera modes. To prevent any code breaking I put waitforseconds to delay the combat mode status so the player can't spam it
        print(Time.time); ///// It also prevents the camera just infinitely staying in basic mode as it was doing before
        yield return new WaitForSeconds(1);
        combatMode = true;
        print("Combat Mode activated");
    }

    IEnumerator BasicModeActive()
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
        combatMode = false;
        print("Switching to Basic mode");
    }

    /////////////////// SWITCH CAMERA STYLE /////////////////////////

    private void SwitchCameraStyle(CameraStyle newStyle) //// THIS IS A PRIVATE METHOD JUST TO SMOOTHLY TRANSITION THE CAMERAS
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);


        currentStyle = newStyle;
    }

}
