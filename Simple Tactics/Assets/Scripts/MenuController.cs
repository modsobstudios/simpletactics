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
            if (hit.transform != null && hit.transform.GetComponent<Button>().Selectable)
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
                                if (!hit.transform.GetComponent<Button>().getIsClicked())
                                {
                                    hit.transform.GetComponent<Button>().setIsClicked(true);
                                }

                                if (Selected == null)
                                {
                                    Selected = hit.transform.gameObject;
                                    hit.transform.GetComponent<Button>().setIsSelected(true);
                                }
                                else if (Selected != hit.transform.gameObject && Selected != null)
                                {
                                    Selected.GetComponent<Button>().setIsSelected(false);
                                    hit.transform.GetComponent<Button>().setIsSelected(true);
                                    Selected = hit.transform.gameObject;
                                }
                                else if (Selected == hit.transform.gameObject)
                                {
                                    hit.transform.GetComponent<Button>().setIsSelected(false);
                                    Selected = null;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 1000.0f))
                    {
                        if (hit.transform.tag == "Button" || hit.transform.tag == "Scene")
                        {
                            {
                                Selected = hit.transform.gameObject;
                                Debug.Log("You held " + hit.transform.name);
                                if (!hit.transform.GetComponent<Button>().getIsClicked())
                                {
                                    hit.transform.GetComponent<Button>().setIsClicked(true);
                                }

                                if (!hit.transform.GetComponent<Button>().getIsHeld())
                                {
                                    hit.transform.GetComponent<Button>().setIsHeld(true);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Selected != null)
                        {
                            Selected.GetComponent<Button>().setIsHeld(false);
                        }
                        Selected = null;
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (Selected != null)
                    {
                        Selected.GetComponent<Button>().setIsHeld(false);
                        Selected = null;
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
                            hitRef.GetComponent<Button>().setIsHighlighted(false);
                        }
                        Debug.Log("You Highlighted " + hit.transform.name);
                        lastHit = hit.transform.name;
                        hit.transform.GetComponent<Button>().setIsHighlighted(true);
                        hitRef = hit.transform;
                    }
                }
            }
            else
            {
                if (hitRef != null)
                {
                    Debug.Log("Stopped Highlighting " + hitRef.name);
                    hitRef.GetComponent<Button>().setIsHighlighted(false);
                    hitRef = null;
                    lastHit = "";
                }
            }
        }
    }
}
