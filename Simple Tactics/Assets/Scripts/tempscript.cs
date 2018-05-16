using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    AudioManager audioMan;
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        audioMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            audioMan.playExampleBGM();
        if (Input.GetKeyDown(KeyCode.S))
            audioMan.playExampleSFX();
        if (Input.GetKeyDown(KeyCode.D))
            audioMan.playExampleVox();
    }
}
