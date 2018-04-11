using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerMove : MonoBehaviour
{
    public Mastermind mm;
    // Use this for initialization
    void Start()
    {
        Button.OnClicked += moveOn;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void moveOn(GameObject _button)
    {
        if(_button.tag == "Button")
        {
            mm.beginMove();
        }
    }
}
