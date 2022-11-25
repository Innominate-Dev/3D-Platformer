using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWindow : MonoBehaviour
{
    Rigidbody[] rbs;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("collided");

            rbs = GetComponentsInChildren<Rigidbody>();

            foreach (var window in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            {
                if (window.name == "pCube54.001")
                {
                    window.SetActive(false);
                }
            }
            //foreach (var pieces in FindObjectsOfType(typeof(GameObject)) as GameObject[])
            //{
            //    if (pieces.name == "BrokenPieces")
            //    {
            //        Debug.Log("pieces active");
            //        pieces.SetActive(true);
            //        Debug.Log("Enabled");
            //    }
            //}
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }

            //for (int i = 0; i < rbs.Length; i++)
            //{
            //    rbs[i].isKinematic = false;
            //    rbs[i].useGravity = true;
            //}
        }
    }
}
