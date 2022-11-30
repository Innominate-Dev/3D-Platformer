using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3); //Will load the next build scene whichw e set by going into build and set a hieracrcy for scenes e.g main Menu is lvl 0 which means its the first thing the player sees.
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit(); 
    }

    public void LoadCreditScene()
    {
        Debug.Log("LOADINGGG CREDITS!!!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToHomeScreen()
    {
        SceneManager.LoadScene("Menu");
    }

    public void Options()
    {
        SceneManager.LoadScene("Settings");
    }
}