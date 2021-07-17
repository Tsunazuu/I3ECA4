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
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void PauseGame ()
    {
        Time.timeScale = 0;
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ResumeGame ()
    {
        Time.timeScale = 1;
        canvas.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

}
