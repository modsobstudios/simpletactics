using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerPrefTestScript : MonoBehaviour
{
    public float storedFloat;
    public int storedInt;
    public string storedString;

    // Use this for initialization
    void Start()
    {
        storedFloat = PlayerPrefs.GetFloat("Stored Float");
        storedInt = PlayerPrefs.GetInt("Stored Int");
        storedString = PlayerPrefs.GetString("Stored String");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            storedFloat = 0.001f;
            storedInt = 2;
            storedString = "You Pressed T.";
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            storedFloat = 0.144f;
            storedInt = 456;
            storedString = "You Pressed Y.";
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            storedFloat = 35.777f;
            storedInt = 32;
            storedString = "You Pressed U.";
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.SetFloat("Stored Float", storedFloat);
            PlayerPrefs.SetInt("Stored Int", storedInt);
            PlayerPrefs.SetString("Stored String", storedString);
        }
    }
}
