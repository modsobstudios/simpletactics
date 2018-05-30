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
        // audioMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        c = GameObject.Find("Character").GetComponent<Character>();
        g = GameObject.Find("Grid").GetComponent<Grid>();
        c.setCharacterTile(g.getTileByRowCol(0, 0));

    }

    // Update is called once per frame
    void Update()
    {

    }
}
