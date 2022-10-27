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
    public enum CameraStyle
    {
        Basic,
        Combat
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        combatMode = false;
    }

    private void Update()
    {
        // switch camera styles
        // if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == false)
        // {
        //     Debug.Log("ZOOOMMMMM");
        //     SwitchCameraStyle(CameraStyle.Combat);
        //     combatMode = true;
        // }
        // if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == true)
        // {
        //     Debug.Log("Zooming out");
        //     SwitchCameraStyle(CameraStyle.Basic);
        //     combatMode = false;
        // }
        if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == false)
        {
            Debug.Log("Moving to combat!");
            SwitchCameraStyle(CameraStyle.Combat);
            StartCoroutine(CombatModeActive());
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && combatMode == true)
        {
            Debug.Log("Moving to basic");
            SwitchCameraStyle(CameraStyle.Basic);
            StartCoroutine(BasicModeActive());
        }

        // rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // roate player object
        if(currentStyle == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }

        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    IEnumerator CombatModeActive()
    {
        print(Time.time);
        yield return new WaitForSeconds(5);
        combatMode = true;
        print("Combat Mode activated");
    }

    IEnumerator BasicModeActive()
    {
        print(Time.time);
        yield return new WaitForSeconds(5);
        combatMode = false;
        print("Switching to Basic mode");
    }

    private void SwitchCameraStyle(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);

        if (newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if (newStyle == CameraStyle.Combat) combatCam.SetActive(true);


        currentStyle = newStyle;
    }

}
