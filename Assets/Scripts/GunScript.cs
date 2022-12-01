using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GunScript : MonoBehaviour
{
    public GameObject Barrel;
    public GameObject bulletPrefab;
    public GameObject gun;
    CinemachineFreeLook Cam;
    float gunrot;
    float camrot;
    
    // Start is called before the first frame update
    void Start()
    {
       Cam = GameObject.Find("ThirdPersonCam").GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        gunmove();
        Debug.Log(gunrot);
        gunrot = gun.transform.rotation.x;
        camrot = Cam.m_YAxis.Value;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bulletObject = Instantiate(bulletPrefab);
            bulletObject.transform.position = Barrel.transform.position + transform.forward;
            bulletObject.transform.forward = Barrel.transform.forward;
        }
    }
    void gunmove()
    {
        gunrot = camrot * 100f;//rotation multiplier

        gun.transform.rotation = Quaternion.Euler(new Vector3(gunrot, gun.transform.rotation.eulerAngles.y, gun.transform.rotation.eulerAngles.z));
    }
}
