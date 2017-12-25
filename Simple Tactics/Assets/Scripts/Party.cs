using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class Party : MonoBehaviour
{
    List<character> charList;
    string[] nameList;

    // Use this for initialization
    void Start()
    {
        charList = new List<character>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            loadNames();
    }
    void loadNames()
    {
        StreamReader reader = new StreamReader("Assets/Resources/MOCK_DATA.csv");
        string temp = reader.ReadToEnd();
        nameList = temp.Split('\n');
    }

    string getRandomName()
    {
        int i = Random.Range(0, nameList.Length);
        return nameList[i];
    }

    static int SortBySpeed(character _c1, character _c2)
    {
        return _c2.getSpeed().CompareTo(_c1.getSpeed());
    }

    public List<character> getCharList()
    {
        return charList;
    }

    public void generateParty(Grid _g, GameObject _player, int _num)
    {
        // start from scratch
        charList.Clear();
        loadNames();
        // create character
        for (int i = 0; i < _num; i++)
        {
            character temp = new character();
            temp.initializeRandom();
            temp.worldPos = _g.getRandomTilePos();
            GameObject newP = Instantiate(_player, temp.worldPos, Quaternion.identity);
            newP.GetComponent<character>().initializeCopy(temp);
            newP.name = getRandomName();
            newP.GetComponent<character>().setCharMesh(newP);
            charList.Add(newP.GetComponent<character>());
        }
        charList.Sort(SortBySpeed);       
    }
}
