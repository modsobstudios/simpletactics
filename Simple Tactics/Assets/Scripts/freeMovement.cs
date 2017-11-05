using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeMovement : MonoBehaviour
{

    [SerializeField]
    GameObject characterObject;

    [SerializeField]
    Camera cam;
    godCamScript gCam;
    shoulderCamScript sCam;

    Transform characterTransform;
    // Use this for initialization
    void Start()
    {
        characterTransform = characterObject.transform;
        gCam = cam.GetComponent<godCamScript>();
        sCam = cam.GetComponent<shoulderCamScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((gCam.currTarget == characterObject && gCam.godCamActive) || (sCam.currTarget == characterObject && sCam.shoulderCamActive))
        {
            if (gCam.godCamActive)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    characterTransform.position += (new Vector3(0,0,1)) * Time.deltaTime * 10;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    characterTransform.position -= (new Vector3(0, 0, 1)) * Time.deltaTime * 10;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    characterTransform.position -= (new Vector3(1, 0, 0)) * Time.deltaTime * 10;
                }
                if (Input.GetKey(KeyCode.D))
                {
                    characterTransform.position += (new Vector3(1, 0, 0)) * Time.deltaTime * 10;
                }
            }
            else if (sCam.shoulderCamActive)
            {

                if (Input.GetKey(KeyCode.W))
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
    }
}
