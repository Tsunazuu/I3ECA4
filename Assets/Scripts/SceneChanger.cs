/******************************************************************************
Author: Aaron Tan Wei Heng & Royden Lim Yong Chee

Name of Class: DemoPlayer

Description of Class: This class will control the movement and actions of a 
                        player avatar based on user input.

Date Created: 09/06/2021
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    
    public int sceneIndex;
    public Canvas HTPcanvas;
    public Canvas RespawnCanvas;

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("quit");
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void ConfirmHTP()
    {
        HTPcanvas.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level One");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
