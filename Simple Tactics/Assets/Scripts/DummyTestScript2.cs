using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestScript2 : MonoBehaviour
{
    public GameObject dummy;
    bool chk = false;
    List<GameObject> goList;
    List<DummyTestScript> scriptList;

    // Use this for initialization
    void Start()
    {
        goList = new List<GameObject>();
        scriptList = new List<DummyTestScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (chk)
        {
            chk = false;
            readValues("scriptList[0] on first update", scriptList[0]);
            readValues("goList[0] on first update", goList[0].GetComponent<DummyTestScript>());

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            scriptList.Clear();
            goList.Clear();
            GameObject savedGO = Instantiate(dummy, new Vector3(0, 0, 0), Quaternion.identity);
            readValues("savedGO at Instantiation", savedGO.GetComponent<DummyTestScript>());
            savedGO.GetComponent<DummyTestScript>().X = 3;
            savedGO.GetComponent<DummyTestScript>().Y = 2;
            savedGO.GetComponent<DummyTestScript>().Z = 1;
            readValues("savedGO after value set", savedGO.GetComponent<DummyTestScript>());

            DummyTestScript savedScript = savedGO.GetComponent<DummyTestScript>();
            readValues("savedScript at initialization", savedScript);
            savedScript.X = 1;
            savedScript.Y = 2;
            savedScript.Z = 3;
            readValues("savedScript after value set", savedScript);

            scriptList.Add(savedScript);
            readValues("scriptList[0] after Add", scriptList[0]);
            goList.Add(savedGO);
            readValues("goList[0] after Add", goList[0].GetComponent<DummyTestScript>());
            chk = true;
            // Before dropping scope, scriptList[0] values == (1,2,3)
            // In the editor, DummyObject spawns with values (0,0,0)
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Changes DummyObject's, goList[0]'s, and scriptList[0]'s values
            goList[0].GetComponent<DummyTestScript>().X = 9;
            goList[0].GetComponent<DummyTestScript>().Y = 9;
            goList[0].GetComponent<DummyTestScript>().Z = 9;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Changes DummyObject's, goList[0]'s, and scriptList[0]'s values
            scriptList[0].X = 7;
            scriptList[0].Y = 7;
            scriptList[0].Z = 7;
        }
        if (Input.GetKeyDown(KeyCode.Semicolon))
        {
            // Changes DummyObject's, goList[0]'s, and scriptList[0]'s values
            DummyTestScript tmp = goList[0].GetComponent<DummyTestScript>();
            tmp.X = 5;
            tmp.Y = 5;
            tmp.Z = 5;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            readValues("scriptList[0]", scriptList[0]);
            readValues("goList[0]", goList[0].GetComponent<DummyTestScript>());
        }
    }

    void readValues(string _s, DummyTestScript _t)
    {
        Debug.Log(_s + ": " + _t.X + ", " + _t.Y + ", " + _t.Z);
    }
}
