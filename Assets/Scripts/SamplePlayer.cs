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
    
    //Collectibles
    public int currentST;
    public int totalST;

    //NPCs
    public GameObject drunkard;
    public GameObject vendor;
    private int cCounter = 0;
    private int dCounter = 0;
    private int vCounter = 0;

    //Health
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;


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
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "UpdateQuestLogArea1")
        {
            questText.text = "Confront the drunkard";
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        CollisionFunction(collision);
    }

    protected virtual void CollisionFunction(Collision collision)
    {
        Debug.Log("hi");
    }
}