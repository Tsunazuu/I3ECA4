/******************************************************************************
Author: Elyas Chua-Aziz

Name of Class: DemoPlayer

Description of Class: This class will control the movement and actions of a 
                        player avatar based on user input.

Date Created: 09/06/2021
******************************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject barrierUpdateQuest3;
    public GameObject cameraPos;
    public GameObject playerPos;
    public GameObject canvasUI;
    public Button carButton;
    public Text playerText;
    public GameObject karenBarrier;
    public GameObject karenCamPos;
    public GameObject karenPlayerPos;
    public GameObject bedPlayerPos;
    
    //Collectibles
    public int currentST;
    public int totalST;

    //NPCs
    public GameObject drunkard;
    public GameObject vendor;
    public GameObject karen;
    private int cCounter = 0;
    private int dCounter = 0;
    private int vCounter = 0;

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
        currentST = 0;
        totalST = 3;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar2.SetMaxHealth(maxHealth);
        canvasUI.gameObject.SetActive(false);
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

        CheckRotation();
        InteractionRaycast();
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

                if (hitinfo.transform.tag == "Drunkard" && dCounter == 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;
                    hitinfo.transform.GetComponent<InitiateDrunkard>().Interact();
                    dCounter += 1;
                }

                if (hitinfo.transform.tag == "SleepToken")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    currentST += 1;
                    updateQuestTextToken();
                }

                if (hitinfo.transform.tag == "Vendor" && vCounter == 0)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;
                    hitinfo.transform.GetComponent<InitiateVendor>().Interact();
                    currentST += 1;
                    updateQuestTextToken();
                    vCounter += 1;
                }

                if (hitinfo.transform.tag == "Bed")
                {
                    frozen = true;
                    this.transform.position = bedPlayerPos.transform.position;
                    this.transform.rotation = bedPlayerPos.transform.rotation;
                    hitinfo.transform.GetComponent<InitiateBed>().Interact();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "UpdateQuestLogArea1")
        {
            questText.text = "Confront the drunkard";
        }

        if (other.tag == "BarrierQuest3")
        {
            this.transform.position = cameraPos.transform.position;
            this.transform.rotation = cameraPos.transform.rotation;
            canvasUI.gameObject.SetActive(true);
            StartCoroutine(startPlayerText());
        }

        if (other.tag == "BarrierKaren")
        {
            canvasUI.gameObject.SetActive(true);
            StartCoroutine(startKarenText());
        }
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
        playerText.text = "*CRACK*";
        yield return new WaitForSeconds(2);
        playerText.text = "You step on a wooden plank on the floor";
        yield return new WaitForSeconds(2);
        playerText.text = "Karen: Oi! Who's making all that noise?!";
        this.transform.position = karenCamPos.transform.position;
        this.transform.rotation = karenCamPos.transform.rotation;
        yield return new WaitForSeconds(2);
        playerText.text = "Karen: You there! Get over here!";
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
        carButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        canvasUI.gameObject.SetActive(false);
        karen.transform.GetComponent<InitiateKaren>().Interact();

    }

    private void updateQuestTextToken()
    {
        STQuest.text = "Collect Sleep Tokens: " + currentST +  "/" + totalST;
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