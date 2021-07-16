using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateBed : MonoBehaviour
{

    public GameObject canvas;
    public GameObject fadeToBlack;
    public GameObject white;
    public GameObject textBox;
    public GameObject sleepDemonSprite;
    public Text battleText;
    public Button attack;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void Interact()
    {
        canvas.gameObject.SetActive(true);
        battleText.gameObject.SetActive(false);
        attack.gameObject.SetActive(false);
        white.gameObject.SetActive(false);
        textBox.gameObject.SetActive(false);
        sleepDemonSprite.gameObject.SetActive(false);
        StartCoroutine(startBedText());
    }

    public IEnumerator startBedText()
    {
        yield return new WaitForSeconds(2);
        battleText.gameObject.SetActive(true);
        textBox.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        white.gameObject.SetActive(true);
        sleepDemonSprite.gameObject.SetActive(true);
        battleText.text = "???: Fear... I smell fear...";
        yield return new WaitForSeconds(2);
        battleText.text = "wha- what is that??";
        yield return new WaitForSeconds(2);
        battleText.text = "Sleep Demon: I am the gatekeeper to your sleep...";
        yield return new WaitForSeconds(2);
        battleText.text = "Sleep Demon: Now... FEAR ME!";
        yield return new WaitForSeconds(2);
        startBedBattle();
    }

    public void startBedBattle()
    {
        attack.gameObject.SetActive(true);
        battleText.text = "Choose an action.";
    }
}
