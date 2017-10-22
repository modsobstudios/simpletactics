using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoulderCamScript : MonoBehaviour
{
    // Bool for which camera is active
    [SerializeField]
    bool shoulderCamActive = false;
    bool switchMode = false;

    // Target character to attach the camera to
    public GameObject tempTarget, tempTarget2eb;
    GameObject currTarget;

    // The camera object
    public Camera cam;

    // Camera vectors
    [SerializeField]
    Vector3 newCamPosition;
    Vector3 targetPos;

    // Quaternion for rotation storage
    Quaternion lookAt;



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
            if (Input.GetKey(KeyCode.A))
            {
                AttachToTarget(tempTarget.transform);
                currTarget = tempTarget;
                switchMode = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                AttachToTarget(tempTarget2eb.transform);
                currTarget = tempTarget2eb;
                switchMode = true;
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
        }
    }

    void AttachToTarget(Transform _trans)
    {
        newCamPosition = _trans.position - (cam.transform.forward * 3.0f) + new Vector3(0, 1, 0);
        //cam.transform.position = newCamPosition;
        lookAt = Quaternion.LookRotation(_trans.forward);
        //cam.transform.rotation = lookAt;
        targetPos = _trans.position;
    }

    void MouseYRotation()
    {
        if (Input.GetButton("Fire2"))
        {

            if (Input.GetAxisRaw("Mouse X") > 0)
            {
                cam.transform.RotateAround(targetPos, cam.transform.up, Time.deltaTime * 100.0f);
                //cam.transform.Rotate(cam.transform.up * (-Time.deltaTime * 100.0f));
                lookAt = cam.transform.rotation;
            }

            else if (Input.GetAxisRaw("Mouse X") < 0)
            {
                cam.transform.RotateAround(targetPos, cam.transform.up, Time.deltaTime * -100.0f);
                //cam.transform.Rotate(cam.transform.up * (Time.deltaTime * 100.0f));
                lookAt = cam.transform.rotation;
            }
        }
        currTarget.transform.rotation = lookAt;
        newCamPosition = cam.transform.position;
    }

    void LerpFocus()
    {
        if (cam.transform.position != newCamPosition)
            cam.transform.position += (newCamPosition - cam.transform.position) * (Time.deltaTime * 10.0f);
        if (cam.transform.rotation != lookAt)
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, lookAt, Time.time * 0.1f);

        if (cam.transform.position == newCamPosition && cam.transform.rotation == lookAt)
            switchMode = false;
    }

    void ZoomTarget()
    {
        if(Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            newCamPosition -= currTarget.transform.forward * (Time.deltaTime * 100.0f);
            cam.transform.position = newCamPosition;
        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            newCamPosition += currTarget.transform.forward * (Time.deltaTime * 100.0f);
            cam.transform.position = newCamPosition;
        }
    }
}
