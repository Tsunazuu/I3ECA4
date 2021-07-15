using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateCar : MonoBehaviour
{

    public GameObject canvas;
    public GameObject player;
    public GameObject barrier;
    public Text playerText;
    public Text questText;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    public void Interact()
    {
        StartCoroutine(startCarText());
    }

    public IEnumerator startCarText()
    {
        canvas.gameObject.SetActive(true);
        var interactingPlayer = player.GetComponent<SamplePlayer>();
        yield return new WaitForSeconds(2);
        playerText.text = "Guess I should probably find another way home...";
        yield return new WaitForSeconds(2);
        playerText.text = "Maybe the alleyway is a good idea...";
        yield return new WaitForSeconds(2);
        questText.text = "Make your way to the alleyway.";
        barrier.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canvas.gameObject.SetActive(false);
        resetPlayer();
    }

    public void resetPlayer()
    {
        player.transform.GetComponent<SamplePlayer>().Unfreeze();
    }

}
