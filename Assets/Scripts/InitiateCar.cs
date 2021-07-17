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

public class InitiateCar : MonoBehaviour
{

    public GameObject canvas;
    public Button carButton;
    public GameObject player;
    public Text playerText;
    public Text questText;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Interact()
    {
        StartCoroutine(startCarText());
    }

    public IEnumerator startCarText()
    {
        canvas.gameObject.SetActive(true);
        carButton.gameObject.SetActive(false);
        var interactingPlayer = player.GetComponent<SamplePlayer>();
        playerText.text = "What?! The truck broke down...";
        yield return new WaitForSeconds(2);
        playerText.text = "Guess I should probably find another way home...";
        yield return new WaitForSeconds(2);
        playerText.text = "Maybe the alleyway is a good idea...";
        yield return new WaitForSeconds(2);
        questText.text = "Make your way to the alleyway.";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.gameObject.SetActive(false);
        resetPlayer();
    }

    public void resetPlayer()
    {
        player.transform.GetComponent<SamplePlayer>().Unfreeze();
    }

}
