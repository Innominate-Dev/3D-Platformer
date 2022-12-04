using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunScript : MonoBehaviour
{
    public GameObject Barrel;
    public GameObject bulletPrefab;
    public GameObject gun;
    //CinemachineFreeLook Cam;
    public GameObject Cam;
    float gunrot;
    float camrot;

    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
       //Cam = GameObject.Find("ThirdPersonCam").GetComponent<CinemachineFreeLook>();
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        gunmove();

        Debug.Log(gunrot);
        gunrot = gun.transform.rotation.x;
        //camrot = Cam.m_YAxis.Value;
        camrot = Cam.transform.rotation.x;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.position = Barrel.transform.position + transform.forward;
            bulletObject.transform.forward = Barrel.transform.forward;
        }
    }
    void gunmove()
    {

        gunrot = camrot * 300f;//rotation multiplier

        if (90f >= gunrot && -90f < gunrot)
        {
            gun.transform.rotation = Quaternion.Euler(new Vector3(gunrot, gun.transform.rotation.eulerAngles.y, gun.transform.rotation.eulerAngles.z));
            Debug.Log("Mooovinggg");
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        //gun.transform.rotation = Quaternion.Euler(new Vector3(gunrot, gun.transform.rotation.eulerAngles.y, gun.transform.rotation.eulerAngles.z));
    }
}
