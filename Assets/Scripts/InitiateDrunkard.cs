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

public class InitiateDrunkard : MonoBehaviour
{
    public GameObject canvas;
    public GameObject battleCanvas;
    public Button option1;
    public Button option2;
    public Button punch;
    public Text drunkardText;
    public Text option1Text;
    public Text option2Text;
    public bool frozen = false;
    public Text questText;
    public GameObject player;
    public GameObject sleepToken;
    public Text STQuest;
    public Text STQuest1;
    public Text STQuest2;
    public GameObject barrier1;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public Button nextBtn;
    public Text battleText;
    public GameObject drunkardPlayerPos;
    public GameObject drunkardCamPos;
    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);
        sleepToken.gameObject.SetActive(false);
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {

    }

    // Update is called once per frame

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void preInteract()
    {
        canvas.gameObject.SetActive(true);
        player.transform.position = drunkardCamPos.transform.position;
        player.transform.rotation = drunkardCamPos.transform.rotation;
        drunkardText.text = "Oi! You there! Come here!";
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        questText.text = "Confront the drunkard.";
        StartCoroutine(Interact());
    }

    public IEnumerator Interact()
    {
        yield return new WaitForSeconds(1.5f);  
        drunkardText.text = "You stare what stare?";
        player.transform.position = drunkardPlayerPos.transform.position;
        player.transform.rotation = drunkardPlayerPos.transform.rotation;
        option1.gameObject.SetActive(true);
        option2.gameObject.SetActive(true);
        option1.onClick.AddListener(updateDrunkard1);
        option2.onClick.AddListener(updateDrunkard2);
    }

    public void updateDrunkard1()
    {
        drunkardText.text = "Drunkard: Nevermind, don't let me catch you again.";
        option1Text.text = "*You let out a sigh of relief*";
        option2.gameObject.SetActive(false);
        option1.onClick.AddListener(convoEnd);
    }

    public void updateDrunkard2()
    {
        drunkardText.text = "Drunkard: You stare at me again?";
        option1Text.text = "Oh yeah? Come at me!";
        option2Text.text = "I'm sorry, may I please pass?";
        option1.onClick.AddListener(fightDrunkard);
        option2.onClick.AddListener(updateDrunkard3);
    }

    public void updateDrunkard3()
    {
        drunkardText.text = "Drunkard: Nevermind, don't let me catch you again.";
        option2Text.text = "*You let out a sigh of relief*";
        option1.gameObject.SetActive(false);
        option2.onClick.AddListener(convoEnd);
    }

    public void convoEnd()
    {
        option2.gameObject.SetActive(true);
        canvas.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        updateQuestTextVendor();
        sleepToken.gameObject.SetActive(true);
        STQuest.gameObject.SetActive(true);
        STQuest1.gameObject.SetActive(true);
        STQuest2.gameObject.SetActive(true);
        barrier1.gameObject.SetActive(false);
        resetPlayer();
    }

    public void resetPlayer()
    {
        player.transform.GetComponent<SamplePlayer>().Unfreeze();
    }

    public void fightDrunkard()
    {
        canvas.gameObject.SetActive(false);
        battleCanvas.gameObject.SetActive(true);
        punch.gameObject.SetActive(false);
        nextBtn.gameObject.SetActive(true);
        nextBtn.onClick.AddListener(fightDrunkard1);
        Debug.Log(currentHealth);
    }

    public void fightDrunkard1()
    {
        nextBtn.gameObject.SetActive(false);
        punch.gameObject.SetActive(true);
        battleText.text = "Choose an action.";
        punch.onClick.AddListener(() => StartCoroutine(startBattle()));
        Debug.Log(currentHealth);
    }


    public IEnumerator startBattle()
    {
        punch.gameObject.SetActive(false);
        battleText.text = "You deal 20 damage to the Drunkard!"; 
        TakeDamage(20);
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            yield return new WaitForSeconds(2);
            battleText.text = "Drunkard: Alright! Alright! Please don't hurt me!";
            player.transform.GetComponent<SamplePlayer>().TakeDamage(-2);
            yield return new WaitForSeconds(2);
            battleText.text = "The drunkard has been defeated!";
            punch.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            convoEnd();
        } else {
            yield return new WaitForSeconds(2);
            battleText.text = "The drunkard deals 5 damage to you!";
            player.transform.GetComponent<SamplePlayer>().TakeDamage(5);
            yield return new WaitForSeconds(2);
            fightDrunkard1();
        }
        
    }

    private void updateQuestTextVendor()
    {
        questText.text = "Approach the fruit vendor";
    }
}
