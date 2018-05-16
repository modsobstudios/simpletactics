using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Party : MonoBehaviour
{
    // temporary list of characters
    List<oldCharacter> charList;
    // list of randomized names
    string[] nameList;
    public delegate void MyDel(int _i);
    public MyDel myDel;

    // Use this for initialization
    void Start()
    {
        charList = new List<oldCharacter>();
        Debug.Log("Binding delegate in Party.cs...");
        myDel = GetComponentInParent<Mastermind>().checkActive;
        Debug.Log("Delegate bound!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Loads the nameList with names
    void loadNames()
    {
        // open file
        StreamReader reader = new StreamReader("Assets/Resources/MOCK_DATA.csv");
        // load the entire file in one read
        string temp = reader.ReadToEnd();
        // split by newline
        nameList = temp.Split('\n');
    }

    string getRandomName()
    {
        int i = Random.Range(0, nameList.Length);
        return nameList[i];
    }

    // delegate for Sort()
    static int SortBySpeed(oldCharacter _c1, oldCharacter _c2)
    {
        return _c2.getSpeed().CompareTo(_c1.getSpeed());
    }

    public List<oldCharacter> getCharList()
    {
        return charList;
    }

    // clears the party and loads a new random party.
    public void generateParty(oldGrid _g, GameObject _player, int _num)
    {
        // start from scratch
        // TODO: Cleanup/reset
        charList.Clear();
        // prep the name list
        loadNames();
        // create character
        for (int i = 0; i < _num; i++)
        {
            // make a character
            oldCharacter temp = new oldCharacter();
            // give it stats
            temp.initializeRandom();
            // put it somewhere
            temp.worldPos = _g.getRandomTilePos();
            // give it a mesh
            GameObject newP = Instantiate(_player, temp.worldPos, Quaternion.identity);
            // give it a name
            newP.name = getRandomName();
            // assign relations
            newP.GetComponent<oldCharacter>().initializeCopy(temp);
            newP.GetComponent<oldCharacter>().setCharMesh(newP);
            newP.transform.SetParent(this.transform);
            // put it in the list
            charList.Add(newP.GetComponent<oldCharacter>());
        }
        // initiative
        // TODO: abstract
        charList.Sort(SortBySpeed);       
    }
}
