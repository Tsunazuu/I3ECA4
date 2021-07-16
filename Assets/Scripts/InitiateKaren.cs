using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateKaren : MonoBehaviour
{   
    
    public GameObject battleCanvas;
    public Button punch;
    public Text battleText;
    public int maxHealth = 100;
    public int playerMaxHealth = 100;
    public int currentHealth;
    public int playerCurrentHealth;
    public HealthBar healthBar;
    public HealthBar playerHealthBar;
    public GameObject player;
    public GameObject sleepToken;
    public Text STQuest;

    
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        playerCurrentHealth = playerMaxHealth - 10;
        healthBar.SetMaxHealth(maxHealth);
        playerHealthBar.SetMaxHealth(playerCurrentHealth);
        battleCanvas.gameObject.SetActive(false);
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
        battleCanvas.gameObject.SetActive(true);
        punch.gameObject.SetActive(true);
        battleText.text = "Choose an action.";
        punch.onClick.AddListener(() => StartCoroutine(karenFight()));
    }

    public IEnumerator karenFight()
    {
        punch.gameObject.SetActive(false);
        battleText.text = "You deal 20 damage to the Karen!"; 
        TakeDamage(20);
        Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            yield return new WaitForSeconds(2);
            battleText.text = "Ugh... don't let me catch you again or you'll pay!";
            playerTakeDamage(-2);
            yield return new WaitForSeconds(2);
            battleText.text = "The Karen has been defeated!";
            punch.gameObject.SetActive(false);
            yield return new WaitForSeconds(2);
            convoEnd();
        } else {
            yield return new WaitForSeconds(2);
            battleText.text = "The Karen deals 5 damage to you!";
            playerTakeDamage(5);
            Debug.Log(playerCurrentHealth);
            yield return new WaitForSeconds(2);
            Interact();
        }
    }

    public void convoEnd()
    {
        battleCanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        resetPlayer();
    }

    public void resetPlayer()
    {
        player.transform.GetComponent<SamplePlayer>().Unfreeze();
    }
}
