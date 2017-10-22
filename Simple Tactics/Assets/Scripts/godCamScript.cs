using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godCamScript : MonoBehaviour
{
    // Bool for which camera is active
    [SerializeField]
    bool godCamActive = true;

    // Reference to the camera object
    public Camera cam;

    // Temporary target objects. These will be passed in from the list of characters in the map.
    public GameObject tempTarget;
    public GameObject tempTarget2eb;

    // Vectors
    Vector3 newCamPos;

    // Default overhead cam position offset
    Vector3 camOffset = new Vector3(0, 10, -10);
    Vector3 camRotation = new Vector3(45, 0, 0);

    float camSpeed = 3.0f;

    // Use this for initialization
    void Start()
    {
        // Set default rotation (45 degrees on the X axis)
        cam.transform.Rotate(camRotation);

        // Connect with the list of characters in the map
        // If no characters are found, set target to Origin
        // else set target to active character

        // temp default starting spot
        TeleportFocus(cam.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Mirrors the code in shoulderCamScript, switches camera mode
        if(Input.GetKeyDown(KeyCode.F))
        {
            godCamActive = !godCamActive;
        }

        if (godCamActive)
        {
            // ensure the camera is at the correct rotation
            cam.transform.rotation = Quaternion.Euler(camRotation);

            // Called every frame to move the camera to the active target
            LerpFocus();
            DetectScroll();

            // Debug code
            if (Input.GetKey(KeyCode.A))
                SetNewCamPosition(tempTarget.transform);
            if (Input.GetKey(KeyCode.D))
                SetNewCamPosition(tempTarget2eb.transform);
        }
    }

    void DetectScroll()
    {

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            camOffset -= new Vector3(0, 1, -1) * Time.deltaTime * 100.0f;
            newCamPos -= new Vector3(0, 1, -1) * Time.deltaTime * 100.0f;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            camOffset += new Vector3(0, 1, -1) * Time.deltaTime * 100.0f;
            newCamPos += new Vector3(0, 1, -1) * Time.deltaTime * 100.0f;
        }
    }

    void TeleportFocus(Transform _trans)
    {
        cam.transform.position = _trans.position + camOffset;
    }

    void SetNewCamPosition(Transform _trans)
    {
        newCamPos = _trans.position + camOffset;
    }

    void LerpFocus()
    {
        if (cam.transform.position != newCamPos)
            cam.transform.position += (newCamPos - cam.transform.position) * (Time.deltaTime * camSpeed);
    }
}
