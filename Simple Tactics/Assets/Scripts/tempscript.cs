using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    AudioManager audioMan;
    Grid g;
    public GameObject c;
    List<Character> party;
    int partySize = 4;

    public List<Character> Party
    {
        get
        {
            return party;
        }

        set
        {
            party = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        // audioMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        party = new List<Character>();
        for(int i = 0; i < partySize; i++)
        {
            party.Add(Instantiate(c, transform.position, Quaternion.identity).GetComponent<Character>());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
