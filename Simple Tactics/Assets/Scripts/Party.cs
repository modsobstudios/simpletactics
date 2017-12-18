using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Party must be able to create new characters
[RequireComponent(typeof(character))]

public class Party : MonoBehaviour
{
    List<character> charList;
    character charObj;

    // Use this for initialization
    void Start()
    {
        charObj = GetComponent<character>();
        charList = new List<character>();
    }

    // Update is called once per frame
    void Update()
    {

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

        // create character
        for (int i = 0; i < _num; i++)
        {
            character temp = new character();
            temp.initializeRandom();
            temp.worldPos = _g.getRandomTilePos();
            temp.setCharMesh(Instantiate(_player, temp.worldPos, Quaternion.identity));
            charList.Add(temp);
        }
        charList.Sort(SortBySpeed);       
    }
}
