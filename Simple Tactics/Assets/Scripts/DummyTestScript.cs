using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTestScript : MonoBehaviour
{
    [SerializeField]
    int x, y, z;

    public int X
    {
        get
        {
            return x;
        }

        set
        {
            x = value;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }

        set
        {
            y = value;
        }
    }

    public int Z
    {
        get
        {
            return z;
        }

        set
        {
            z = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        X = Y = Z = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
