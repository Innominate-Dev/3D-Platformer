using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameButton : MonoBehaviour
{
    public GameObject Ending;
    bool inArea;
    // Start is called before the first frame update
    void Start()
    {
        Ending.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && inArea == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Ending.SetActive(true);
            Debug.Log("Ending");
            
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("Ending1");
            inArea = true;

        }
    }
}
