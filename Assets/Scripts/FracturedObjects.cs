using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FracturedObjects : MonoBehaviour
{
    Rigidbody[] rbs;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("collided");

            rbs = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rbs.Length; i++)
            {
                rbs[i].isKinematic = false;
                rbs[i].useGravity = true;
            }
        }
    }
}
