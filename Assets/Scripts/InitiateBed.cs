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

public class InitiateBed : MonoBehaviour
{

    public GameObject canvas;
    public GameObject battleCanvas;
    public GameObject endCanvas;
    public GameObject gameEndCanvas;
    public GameObject playerUIcanvas;
    public GameObject fadeToBlack;
    public GameObject white;
    public GameObject textBox;
    public GameObject sleepDemonSprite;
    public Text text;
    public Text endText;
    public Button cleanse;
    public Text battleText;
    public int maxHealth = 100;
    public int playerMaxHealth = 100;
    public int currentHealth;
    public int playerCurrentHealth;
    public HealthBar healthBar;
    public HealthBar playerHealthBar;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(false);
        gameEndCanvas.gameObject.SetActive(false);
        currentHealth = maxHealth;
        playerCurrentHealth = playerMaxHealth - 25;
        healthBar.SetMaxHealth(maxHealth);
        playerHealthBar.SetMaxHealth(playerCurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void playerTakeDamage(int damage)
    {
        playerCurrentHealth -= damage;
        playerHealthBar.SetHealth(playerCurrentHealth);
    }

    public void Interact()
    {
        canvas.gameObject.SetActive(true);
        text.gameObject.SetActive(false);
        white.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
        sleepDemonSprite.gameObject.SetActive(false);
        StartCoroutine(startBedText());
    }

    public IEnumerator startBedText()
    {
        yield return new WaitForSeconds(2);
        text.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        white.gameObject.SetActive(true);
        sleepDemonSprite.gameObject.SetActive(true);
        text.text = "???: Fear... I smell fear...";
        yield return new WaitForSeconds(2);
        text.text = "Wha- what is that??";
        yield return new WaitForSeconds(2);
        text.text = "Sleep Demon: I am the gatekeeper to your sleep...";
        yield return new WaitForSeconds(2);
        text.text = "Sleep Demon: Now... FEAR ME!";
        yield return new WaitForSeconds(2);
        startBedBattle();
        fadeToBlack.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void startBedBattle()
    {
        battleText.text = "Choose an action.";
        cleanse.gameObject.SetActive(true);
        cleanse.onClick.AddListener(() => StartCoroutine(bedFight()));
    }

    public IEnumerator bedFight()
    {   
        cleanse.gameObject.SetActive(false);
        battleText.text = "You deal 20 damage to the Sleep Demon!"; 
        TakeDamage(20);
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            yield return new WaitForSeconds(2);
            battleText.text = "Sleep Demon: NOOO! This... can't... be...!";
            playerTakeDamage(-2);
            yield return new WaitForSeconds(2);
            battleText.text = "The Sleep Demon has been defeated!";
            cleanse.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            StartCoroutine(convoEnd());
        } else {
            yield return new WaitForSeconds(2);
            battleText.text = "The Sleep Demon deals 10 damage to you!";
            playerTakeDamage(10);
            Debug.Log(playerCurrentHealth);
            yield return new WaitForSeconds(2);
            startBedBattle();
        }
    }

    public IEnumerator convoEnd()
    {
        canvas.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerUIcanvas.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        endText.text = "I should really get some sleep now...";
        yield return new WaitForSeconds(2);
        endText.text = "*Yawn*";
        yield return new WaitForSeconds(2);
        endText.text = "...";
        yield return new WaitForSeconds(2);
        gameEnd();
    }

    public void gameEnd()
    {
        canvas.gameObject.SetActive(false);
        gameEndCanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
