using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public int Jump = 5;
    private PlayerController pc;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")

        other.gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
        other.gameObject.GetComponentInParent<Rigidbody>().AddForce(Vector3.up * Jump, ForceMode.Impulse);
    }


}
