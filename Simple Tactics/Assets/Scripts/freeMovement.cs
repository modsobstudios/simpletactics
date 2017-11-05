using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeMovement : MonoBehaviour
{

    [SerializeField]
    GameObject characterObject;

    Transform characterTransform;
    // Use this for initialization
    void Start()
    {
        characterTransform = characterObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            characterTransform.position += characterTransform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.S))
        {
            characterTransform.position -= characterTransform.forward * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.A))
        {
            characterTransform.position -= characterTransform.right * Time.deltaTime * 10;
        }
        if (Input.GetKey(KeyCode.D))
        {
            characterTransform.position += characterTransform.right * Time.deltaTime * 10;
        }
    }
}
