using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateVendor : MonoBehaviour
{

    public GameObject canvas;
    public Button option1;
    public Button option2;
    public Button continueBtn;
    public Text vendorText;
    public Text option1Text;
    public Text option2Text;
    public bool frozen = false;
    public Text questText;
    public GameObject player;
    public Text STQuest;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Interact()
    {
        canvas.gameObject.SetActive(true);
        option1.onClick.AddListener(updateVendor1);
    }

    public void updateVendor1()
    {
        vendorText.text = "Vendor: Okay! I'll go get you some. Wait right there!";
        continueBtn.gameObject.SetActive(true);
        option1.gameObject.SetActive(false);
        continueBtn.onClick.AddListener(updateVendor2);
    }
    
    public void updateVendor2()
    {
        vendorText.text = "...";
        continueBtn.onClick.AddListener(updateVendor3);
    }

    public void updateVendor3()
    {
        vendorText.text = "Vendor: Here you are! Enjoy your mangoes!";
        option1Text.text = "Thank you!";
        option1.gameObject.SetActive(true);
        continueBtn.gameObject.SetActive(false);
        option1.onClick.AddListener(updateVendor4);
    }

    public void updateVendor4()
    {
        vendorText.text = "Vendor: Wait! Take this with you! I have no idea what it is, or what to do with it anyway.";
        option2Text.text = "Oh... okay...";
        option1.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(false);
        option2.gameObject.SetActive(true);
        option2.onClick.AddListener(updateVendor5);
    }

    public void updateVendor5()
    {
        vendorText.text = "You received a Sleep Token!";
        option1.gameObject.SetActive(false);
        option2.gameObject.SetActive(false);
        continueBtn.gameObject.SetActive(true);
        continueBtn.onClick.AddListener(convoEnd);
    }

    public void convoEnd()
    {
        canvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        updateQuestTextCont();
        STQuest.gameObject.SetActive(true);
        resetPlayer();
    }

    public void updateQuestTextCont()
    {
        questText.text = "Carry on forward.";
    }

    public void resetPlayer()
    {
        player.transform.GetComponent<SamplePlayer>().Unfreeze();
    }

}
