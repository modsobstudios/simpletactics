using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempscript : MonoBehaviour
{
    AudioManager audioMan;
    Grid g;
    public GameObject c;
    public GameObject e;
    List<Character> party;
    List<Enemy> enemies;
    int partySize = 4;
    int enemyCt = 4;

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

    public List<Enemy> Enemies
    {
        get
        {
            return enemies;
        }

        set
        {
            enemies = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        // audioMan = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        party = new List<Character>();
        enemies = new List<Enemy>();
        for(int i = 0; i < partySize; i++)
        {
            party.Add(Instantiate(c, transform.position, Quaternion.identity).GetComponent<Character>());
        }
        for(int i = 0; i < enemyCt; i++)
        {
            enemies.Add(Instantiate(e, transform.position, Quaternion.identity).GetComponent<Enemy>());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
