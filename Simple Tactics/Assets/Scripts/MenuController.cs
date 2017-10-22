using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    public bool isPaused = false;
    [SerializeField]
    bool isMenu = false;

    RaycastHit hit;
    Ray ray;

    Transform hitRef;

    string lastHit;

    public GameObject Selected;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused || isMenu)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    if (hit.transform.tag == "Button" || hit.transform.tag == "Scene")
                    {
                        {
                            Debug.Log("You selected " + hit.transform.name);
                            if (!hit.transform.GetComponent<Button>().isClicked)
                            {
                                hit.transform.GetComponent<Button>().isClicked = true;
                            }

                            if(Selected == null)
                            {
                                Selected = hit.transform.gameObject;
                                hit.transform.GetComponent<Button>().isSelected = true;
                            }
                            if (Selected != hit.transform.gameObject && Selected != null)
                            {
                                Selected.GetComponent<Button>().isSelected = false;
                                hit.transform.GetComponent<Button>().isSelected = true;
                                Selected = hit.transform.gameObject;
                            }
                        }
                    }
                }
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Button" || hit.transform.tag == "Scene")
                {
                    if (hit.transform.name != lastHit)
                    {
                        if (hitRef != null)
                        {
                            hitRef.GetComponent<Button>().isHighlighted = false;
                        }
                        Debug.Log("You Highlighted " + hit.transform.name);
                        lastHit = hit.transform.name;
                        hit.transform.GetComponent<Button>().isHighlighted = true;
                        hitRef = hit.transform;
                    }
                }
            }
            else
            {
                if (hitRef != null)
                {
                    Debug.Log("Stopped Highlighting " + hitRef.name);
                    hitRef.GetComponent<Button>().isHighlighted = false;
                    hitRef = null;
                    lastHit = "";
                }
            }
        }
    }
}
