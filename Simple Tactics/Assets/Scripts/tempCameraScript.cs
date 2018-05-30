using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCameraScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse2))
        {
            transform.position += new Vector3(-Input.GetAxisRaw("Mouse X"), 0, 0) * Time.deltaTime * 10;
            transform.position += new Vector3(0, 0, -Input.GetAxisRaw("Mouse Y")) * Time.deltaTime * 10;
        }
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
            transform.position += new Vector3(0, -1, 0);
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            transform.position += new Vector3(0, 1, 0);
    }
}
