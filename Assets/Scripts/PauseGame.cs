using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public bool GameIsPaused = false;
    public GameObject pauseMenuUI;


    public void pause_system()
    {
        if (Time.timeScale == 0)
        {
            pauseMenuUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            GameIsPaused = false;
        }
        else
        {
            GameIsPaused = true;
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause_system();
        }
    }
}



