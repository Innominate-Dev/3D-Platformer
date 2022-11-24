using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWindow : MonoBehaviour
{
    Rigidbody[] rbs;

    // Update is called once per frame
    void Update()
    {



        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                Debug.Log("collided");

                rbs = GetComponentsInChildren<Rigidbody>();

                for (int i = 0; i < rbs.Length; i++)
                {
                    rbs[i].isKinematic = false;
                    rbs[i].useGravity = true;
                }

                foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
                {
                    if (gameObj.name == "BrokenPieces")
                    {
                        gameObj.SetActive(true);
                    }
                }
            }
        }

    }
}
