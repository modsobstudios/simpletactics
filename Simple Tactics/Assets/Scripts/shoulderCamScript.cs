using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoulderCamScript : MonoBehaviour
{
    // Bool for which camera is active
    public bool shoulderCamActive = false;
    bool switchMode = false;

    // Target character to attach the camera to
    public GameObject tempTarget, tempTarget2eb;
    public GameObject currTarget;

    // The camera object
    public Camera cam;

    // Camera vectors
    [SerializeField]
    Vector3 newCamPosition;
    Vector3 targetPos;

    // Quaternion for rotation storage
    Quaternion lookAt;

    // Magic Numbers
    float shoulderOffset = 3.0f; // multiply against object forward to place the camera 'behind' it
    float camTransLerpRate = 10.0f; // rate at which the camera moves when changing targets
    float camRotLerpRate = 0.1f; // rate at which the camera rotates when changing targets
    float camZoomRate = 100.0f; // zoom rate
    float camZoomMin = 2.0f; // minimum distance from player object
    float camZoomMax = 5.0f; // max distance from player object
    float camRotViewRate = 100.0f; // rate at which player can rotate the camera

    Vector3 camPosOffset = new Vector3(0, 1, 0); // add to camera position to place it at shoulder level rather than waist level


    // Use this for initialization
    void Start()
    {
        // Target must be set before running
        currTarget = tempTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            shoulderCamActive = !shoulderCamActive;
            if (shoulderCamActive)
            {
                AttachToTarget(currTarget.transform);
                switchMode = true;
            }
        }

        if (shoulderCamActive)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                float dist = Vector3.Distance(currTarget.transform.position, cam.transform.position);
                Debug.Log(dist);
            }

            if (switchMode)
            {
                LerpFocus();
            }
            else
            {
                MouseYRotation();
                ZoomTarget();
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                AttachToTarget(tempTarget.transform);
                currTarget = tempTarget;
                switchMode = true;
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                AttachToTarget(tempTarget2eb.transform);
                currTarget = tempTarget2eb;
                switchMode = true;
            }

            AttachToTarget(currTarget.transform);
            LerpFocus();
        }
    }

    void AttachToTarget(Transform _trans)
    {
        newCamPosition = (_trans.position - (_trans.forward * shoulderOffset)) + camPosOffset;
        lookAt = Quaternion.LookRotation(_trans.forward);
        targetPos = _trans.position;
    }

    void MouseYRotation()
    {
        if (Input.GetButton("Fire2"))
        {

            if (Input.GetAxisRaw("Mouse X") > 0)
            {
                cam.transform.RotateAround(targetPos, cam.transform.up, Time.deltaTime * camRotViewRate);
                lookAt = cam.transform.rotation;
            }

            else if (Input.GetAxisRaw("Mouse X") < 0)
            {
                cam.transform.RotateAround(targetPos, cam.transform.up, Time.deltaTime * -camRotViewRate);
                lookAt = cam.transform.rotation;
            }
        }
        currTarget.transform.rotation = lookAt;
        newCamPosition = cam.transform.position;
    }

    void LerpFocus()
    {
        if (cam.transform.position != newCamPosition)
            cam.transform.position += (newCamPosition - cam.transform.position) * (Time.deltaTime * camTransLerpRate);
        if (cam.transform.rotation != lookAt)
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, lookAt, Time.time * camRotLerpRate);

        if (cam.transform.position == newCamPosition && cam.transform.rotation == lookAt)
            switchMode = false;
    }

    void ZoomTarget()
    {
        float dist = Vector3.Distance(currTarget.transform.position, cam.transform.position);

        // Zoom Out
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (dist <= camZoomMax)
            {
                newCamPosition -= currTarget.transform.forward * (Time.deltaTime * camZoomRate);
                cam.transform.position = newCamPosition;
            }
        }
        // Zoom In
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (dist >= camZoomMin)
            {
                newCamPosition += currTarget.transform.forward * (Time.deltaTime * camZoomRate);
                cam.transform.position = newCamPosition;
            }
        }
    }
}
