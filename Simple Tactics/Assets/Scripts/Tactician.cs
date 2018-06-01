using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tactician : MonoBehaviour
{

    Pathfinder pf;

    // Use this for initialization
    void Start()
    {
        pf = GameObject.Find("ScriptTester").GetComponent<Pathfinder>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Character setTarget(List<Character> party, Enemy e)
    {
        int distance = 10000;
        Character temp = null;
        foreach(Character c in party)
        {
            Debug.Log("Checking " + c); 
            List<Tile> tempath = pf.getPath(e.Location, c.Location);
            if(tempath.Count < distance)
            {
                temp = c;
                distance = tempath.Count;
            }
        }
        Debug.Log("Chose " + temp.name);
        return temp;
    }

    public List<Tile> chooseAction(Character c, Enemy e, List<Tile> atkRange)
    {
        return null;
    }

    public List<Tile> move(Enemy e)
    {
        return null;
    }

    public void attack(Enemy e, Character c)
    {
        c.CurrentHP -= e.CurrentAttack;
    }
}
