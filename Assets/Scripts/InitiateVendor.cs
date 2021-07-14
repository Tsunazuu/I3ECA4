using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitiateVendor : MonoBehaviour
{

    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void Interact()
    {
        canvas.gameObject.SetActive(true);
    }
}
