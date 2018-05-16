using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    AudioManager audioMan;
    Character c;
    Grid g;
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        audioMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        c = GameObject.Find("Character").GetComponent<Character>();
        g = GameObject.Find("Grid").GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //    audioMan.playExampleBGM();
        //if (Input.GetKeyDown(KeyCode.S))
        //    audioMan.playExampleSFX();
        //if (Input.GetKeyDown(KeyCode.D))
        //    audioMan.playExampleVox();

        if(Input.GetKeyDown(KeyCode.A))
        {
            c = GameObject.Find("Character").GetComponent<Character>();
            g = GameObject.Find("Grid").GetComponent<Grid>();
            c.setCharacterTile(g.getTileByRowCol(0, 0));
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            c.setCharacterTile(g.getTileByRowCol(5, 5));
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            c.setCharacterTile(g.getTileByRowCol(10, 0));
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            c.setCharacterTile(g.getTileByRowCol(Random.Range(0,g.Width), Random.Range(0, g.Height)));
        }
    }
}
