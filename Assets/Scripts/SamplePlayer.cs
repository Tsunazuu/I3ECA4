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


    // Start is called before the first frame update
    void Start()
    {
        nextState = "Idle";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        STQuest.gameObject.SetActive(false);
        currentST = 0;
        totalST = 3;
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
        if (nextState != currentState)
        {
            SwitchState();
        }

        CheckRotation();
        InteractionRaycast();
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
                if (hitinfo.transform.tag == "Car")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    updateQuestTextHome();
                }

                if (hitinfo.transform.tag == "Drunkard")
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;
                    hitinfo.transform.GetComponent<InitiateDrunkard>().Interact();
                    frozen = false;
                    Debug.Log("yes");
                }

                if (hitinfo.transform.tag == "SleepToken")
                {
                    hitinfo.transform.GetComponent<InteractableObject>().Interact();
                    currentST += 1;
                    updateQuestTextToken();
                }

                if (hitinfo.transform.tag == "Vendor")
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    frozen = true;
                    hitinfo.transform.GetComponent<InitiateVendor>().Interact(); 
                }

            }
        }
    }

    private void updateQuestTextHome()
    {
        questText.text = "Find another way home";
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