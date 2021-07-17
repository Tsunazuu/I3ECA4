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

public class SamplePlayer : MonoBehaviour
{
    /// <summary>
    /// The distance this player will travel per second.
    /// </summary>
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float rotationSpeed;

    public bool frozen = false;

    [SerializeField]
    private float interactionDistance;

    /// <summary>
    /// The camera attached to the player model.
    /// Should be dragged in from Inspector.
    /// </summary>
    [SerializeField]
    private Camera playerCamera;

    private string currentState;

    private string nextState;

    //UI related stuff
    public Text questText;
    public Text STQuest;
    public Text STQuest1;
    public Text STQuest2;
    public Text belongingsQuest;
    public GameObject barrierUpdateQuest3;
    public GameObject cameraPos;
    public GameObject playerPos;
    public GameObject officeDoorExitPos;
    public GameObject officeDoorEnterPos;
    public GameObject houseDoorEnterPos;
    public GameObject houseDoorExitPos;
    public GameObject roomDoorEnterPos;
    public GameObject roomDoorExitPos;
    public GameObject canvasUI;
    public Button carButton;
    public Text playerText;
    public GameObject karenBarrier;
    public GameObject karenCamPos;
    public GameObject karenPlayerPos;
    public GameObject bedPlayerPos;
    public GameObject questCompleteCanvas;
    public GameObject pauseMenu;
    public Button resume;
    public Button exit;
    
    //Collectibles
    public int currentST;
    public int totalST;
    public int beer = 0;
    public int mango = 0;
    private int bCounter = 0;
    private int camChangeCounter = 0;

    //NPCs
    public GameObject drunkard;
    public GameObject vendor;
    public GameObject karen;
    private int cCounter = 0;
    private int vCounter = 0;
    private int startCounter1 = 0;

    //Health
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public HealthBar healthBar2;


    // Start is called before the first frame update
    void Start()
    {
        nextState = "Idle";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        STQuest.gameObject.SetActive(false);
        STQuest1.gameObject.SetActive(false);
        STQuest2.gameObject.SetActive(false);
        currentST = 0;
        totalST = 3;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar2.SetMaxHealth(maxHealth);
        canvasUI.gameObject.SetActive(false);
        belongingsQuest.gameObject.SetActive(false);
        questCompleteCanvas.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(false);
        StartCoroutine(onLoadText());
    }

    // Update is called once per frame
    void Update()
    { 
        if (frozen == true)
        {
            moveSpeed = 0;
            rotationSpeed = 0;
        } else {
            moveSpeed = 5;
            rotationSpeed = 170;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

        if (nextState != currentState)
        {
            SwitchState();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PauseGame();
        }

        CheckRotation();
        InteractionRaycast();
    }

    public void PauseGame ()
    {
        Time.timeScale = 0;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        healthBar2.SetHealth(currentHealth);
    }

    public void Unfreeze()
    {
        frozen = false;
    }

    private void InteractionRaycast()
    {
        Debug.DrawLine(playerCamera.transform.position,
                    playerCamera.transform.position + playerCamera.transform.forward * interactionDistance);

        int layermask = 1 << LayerMask.NameToLayer("Interactable");

        RaycastHit hitinfo;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
            out hitinfo, interactionDistance, layermask))
        {
            // if my ray hits something, if statement is true
            // do stuff here
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hitinfo.transform.tag == "Car" && cCounter == 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;  
                    hitinfo.transform.GetComponent<InitiateCar>().Interact();
                    cCounter += 1;
                }

                if (hitinfo.transform.tag == "BeerBottle")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    updateQuestTextBB();
                }

                if (hitinfo.transform.tag == "Phone")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    bCounter += 1;
                    updateQuestTextBelongings();
                    checkBelongings();
                }

                if (hitinfo.transform.tag == "Keys")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    bCounter += 1;
                    updateQuestTextBelongings();
                    checkBelongings();
                }

                if (hitinfo.transform.tag == "Vendor" && vCounter == 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;
                    hitinfo.transform.GetComponent<InitiateVendor>().Interact();
                    hitinfo.transform.GetComponent<InitiateVendor>().Interact();
                    vCounter += 1;
                }

                if (hitinfo.transform.tag == "Bed")
                {
                    frozen = true;
                    this.transform.position = bedPlayerPos.transform.position;
                    this.transform.rotation = bedPlayerPos.transform.rotation;
                    hitinfo.transform.GetComponent<InitiateBed>().Interact();
                }

                if (hitinfo.transform.tag == "OfficeDoor")
                {
                    this.transform.position = officeDoorExitPos.transform.position;
                    this.transform.rotation = officeDoorExitPos.transform.rotation;
                    questText.text = "Get to the car.";
                }

                if (hitinfo.transform.tag == "OfficeDoorEnter")
                {
                    this.transform.position = officeDoorEnterPos.transform.position;
                    this.transform.rotation = officeDoorEnterPos.transform.rotation;
                }

                if (hitinfo.transform.tag == "HouseDoorEnter")
                {
                    if (bCounter > 1 && beer == 1)
                    {
                        this.transform.position = houseDoorEnterPos.transform.position;
                        this.transform.rotation = houseDoorEnterPos.transform.rotation;
                        Debug.Log(bCounter);
                        Debug.Log(beer);
                        Debug.Log(mango);
                    } else {
                    StartCoroutine(noCollectibles());
                    }
                }

                if (hitinfo.transform.tag == "HouseDoorExit")
                {
                    this.transform.position = houseDoorExitPos.transform.position;
                    this.transform.rotation = houseDoorExitPos.transform.rotation;
                }
                    
                if (hitinfo.transform.tag == "RoomDoorEnter")
                {
                    this.transform.position = roomDoorEnterPos.transform.position;
                    this.transform.rotation = roomDoorEnterPos.transform.rotation;
                }

                if (hitinfo.transform.tag == "RoomDoorExit")
                {
                    this.transform.position = roomDoorExitPos.transform.position;
                    this.transform.rotation = roomDoorExitPos.transform.rotation;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "UpdateQuestLogArea1" && startCounter1 == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            frozen = true;
            drunkard.transform.GetComponent<InitiateDrunkard>().preInteract();
            startCounter1 += 1;
        }

        if (other.tag == "BarrierQuest3" && camChangeCounter == 0)
        {
            this.transform.position = cameraPos.transform.position;
            this.transform.rotation = cameraPos.transform.rotation;
            questText.text = "Enter the narrow alley.";
            canvasUI.gameObject.SetActive(true);
            StartCoroutine(startPlayerText());
            camChangeCounter += 1;
        }

        if (other.tag == "BarrierKaren")
        {
            canvasUI.gameObject.SetActive(true);
            StartCoroutine(startKarenText());
        }
    }

    public IEnumerator noCollectibles()
    {
        canvasUI.gameObject.SetActive(true);
        frozen = true;
        playerText.text = "Hmm... did I forget something?";
        yield return new WaitForSeconds(2);
        canvasUI.gameObject.SetActive(false);
        Unfreeze();
    }
    
    public void updateQuestTextBelongings()
    {
        belongingsQuest.text = "Pick up belongings: " + bCounter + "/" + "2";
    }

    public IEnumerator onLoadText()
    {
        frozen = true;
        questText.text = "Leave the office.";
        canvasUI.gameObject.SetActive(true);
        carButton.gameObject.SetActive(false);
        playerText.text = "Arghh... I hate this job... Always working overtime...";
        yield return new WaitForSeconds(2);
        playerText.text = "Well, it's time to head home...";
        yield return new WaitForSeconds(2);
        belongingsQuest.gameObject.SetActive(true);
        canvasUI.gameObject.SetActive(false);
        Unfreeze();
    }

    public IEnumerator startPlayerText()
    {
        frozen = true;
        canvasUI.gameObject.SetActive(true);
        playerText.text = "This should be the right way home...";
        yield return new WaitForSeconds(2);
        playerText.text = "... if I remember correctly...";
        yield return new WaitForSeconds(2);
        canvasUI.gameObject.SetActive(false);
        Unfreeze();
        this.transform.position = playerPos.transform.position;
        this.transform.rotation = playerPos.transform.rotation;
    }

    public IEnumerator startKarenText()
    {
        frozen = true;
        canvasUI.gameObject.SetActive(true);
        carButton.gameObject.SetActive(false);
        playerText.text = "*CRACK*";
        yield return new WaitForSeconds(2);
        playerText.text = "You step on a wooden plank on the floor";
        yield return new WaitForSeconds(2);
        playerText.text = "Karen: Oi! Who's making all that noise?!";
        this.transform.position = karenCamPos.transform.position;
        this.transform.rotation = karenCamPos.transform.rotation;
        yield return new WaitForSeconds(2);
        playerText.text = "Karen: You there! Get over here!";
        questText.text = "Confront the Karen.";
        yield return new WaitForSeconds(2);
        this.transform.position = karenPlayerPos.transform.position;
        this.transform.rotation = karenPlayerPos.transform.rotation;
        playerText.text = "Karen: Why are you making so much noise at night when everyone is sleeping?!";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        carButton.gameObject.SetActive(true);
        carButton.onClick.AddListener(() => StartCoroutine(startKarenFight()));
    }

    public IEnumerator startKarenFight()
    {
        playerText.text = "Karen: You... you...! I'll teach you a lesson!";
        questText.text = "Defeat the Karen";
        carButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        canvasUI.gameObject.SetActive(false);
        karen.transform.GetComponent<InitiateKaren>().Interact();

    }

    private void checkBelongings()
    {
        if (bCounter == 2)
        {
            StartCoroutine(questComplete());
        }
    }

    private void updateQuestTextBB()
    {
        STQuest1.text = "Beer: 1/1";
        beer += 1;
        StartCoroutine(questComplete());
    }

    public void updateQuestTextMango()
    {
        STQuest2.text = "Mango: 1/1";
        mango += 1;
        StartCoroutine(questComplete());
    }

    public IEnumerator questComplete()
    {
        questCompleteCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        questCompleteCanvas.gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the current state of the player
    /// and starts the correct coroutine.
    /// </summary>
    private void SwitchState()
    {
        StopCoroutine(currentState);

        currentState = nextState;
        StartCoroutine(currentState);
    }

    private IEnumerator Idle()
    {
        while(currentState == "Idle")
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                nextState = "Moving";
            }
            yield return null;
        }
    }

    private IEnumerator Moving()
    {
        while (currentState == "Moving")
        {
            if (!CheckMovement())
            {
                nextState = "Idle";
            }
            yield return null;
        }
    }

    private void CheckRotation()
    {
        Vector3 playerRotation = transform.rotation.eulerAngles;
        playerRotation.y += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(playerRotation);

        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        cameraRotation.x -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    /// <summary>
    /// Checks and handles movement of the player
    /// </summary>
    /// <returns>True if user input is detected and player is moved.</returns>
    private bool CheckMovement()
    {
        Vector3 newPos = transform.position;

        Vector3 xMovement = transform.right * Input.GetAxis("Horizontal");
        Vector3 zMovement = transform.forward * Input.GetAxis("Vertical");

        Vector3 movementVector = xMovement + zMovement;

        if(movementVector.sqrMagnitude > 0)
        {
            movementVector *= moveSpeed * Time.deltaTime;
            newPos += movementVector;

            transform.position = newPos;
            return true;
        }
        else
        {
            return false;
        }

    }
}