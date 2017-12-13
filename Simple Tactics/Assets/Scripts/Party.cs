using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{

    int numPlayers;
    List<GameObject> playerList;
    public GameObject player;
    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<GameObject> getParty()
    {
        return playerList;
    }
    
    public void setParty(List<GameObject> _p)
    {
        playerList = _p;
    }
}
