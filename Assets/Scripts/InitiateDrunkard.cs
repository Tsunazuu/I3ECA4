using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateDrunkard : MonoBehaviour
{
    public GameObject canvas;
    public Button option1;
    public Button option2;
    public Text drunkardText;
    public Text option1Text;
    public Text option2Text;
    public bool frozen = false;
    public Text questText;
    public GameObject player;
    public Camera playerCamera;
    public GameObject cameraPos;
    public GameObject sleepToken;
    public Text STQuest;
    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
        sleepToken.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Interact()
    {
        canvas.gameObject.SetActive(true);
        option1.onClick.AddListener(updateDrunkard1);
        option2.onClick.AddListener(updateDrunkard2);
    }

    public void updateDrunkard1()
    {
        drunkardText.text = "Nevermind, don't let me catch you again.";
        option1Text.text = "*You let out a sigh of relief*";
        option2.gameObject.SetActive(false);
        option1.onClick.AddListener(convoEnd);
    }

    public void updateDrunkard2()
    {
        drunkardText.text = "You stare at me again?";
        option1Text.text = "Oh yeah? Come at me!";
        option2Text.text = "I'm sorry, may I please pass?";
        option1.onClick.AddListener(fightDrunkard);
        option2.onClick.AddListener(updateDrunkard1);
    }

    public void convoEnd()
    {
        option2.gameObject.SetActive(true);
        canvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        frozen = false;
        updateQuestTextVendor();
        sleepToken.gameObject.SetActive(true);
        STQuest.gameObject.SetActive(true);
    }

    public void fightDrunkard()
    {
        canvas.gameObject.SetActive(false);
        playerCamera.transform.position = cameraPos.transform.position;
        playerCamera.transform.rotation = cameraPos.transform.rotation;
    }

    private void updateQuestTextVendor()
    {
        questText.text = "Approach the fruit vendor";
    }
}
