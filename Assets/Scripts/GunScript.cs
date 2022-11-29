using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject Barrel;
    public GameObject bulletPrefab;
    public GameObject gun;
    public Camera Cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.position = Barrel.transform.position + transform.forward;
            bulletObject.transform.forward = Barrel.transform.forward;
        }
    }
}
