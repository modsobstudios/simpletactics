﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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

    public enum NEIGHBORS { UP, DOWN, LEFT, RIGHT };

    private AudioManager aud;

    // Use this for initialization
    void Start()
    {
        aud = FindObjectOfType<AudioManager>();
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
                            Debug.Log("You selected " + hit.transform.name);
                            if (!hit.transform.GetComponent<Button>().getIsClicked())
                            {
                                hit.transform.GetComponent<Button>().setIsClicked(true);
                                aud.PlayAudio("Menu_Click", AudioManager.AudioType.SFX);
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
            //Case of using key controls
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (Selected != null)
                {
                    if (Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.DOWN) != null)
                    {
                        Selected.GetComponent<Button>().setIsHighlighted(false);
                        Selected.GetComponent<Button>().setIsSelected(false);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.DOWN).GetComponent<Button>().setIsSelected(true);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.DOWN).GetComponent<Button>().setIsHighlighted(true);
                        Selected = Selected.GetComponent<Button>().getNeighbor((int)(NEIGHBORS.DOWN));
                        aud.PlayAudio("Menu_Dink1", AudioManager.AudioType.SFX);
                    }
                    else
                    {
                        aud.PlayAudio("Menu_Dink2", AudioManager.AudioType.SFX);

                    }
                }
                else
                {
                    Selected = FindObjectOfType<Button>().gameObject;
                    Selected.GetComponent<Button>().setIsHighlighted(true);
                    Selected.GetComponent<Button>().setIsSelected(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (Selected != null)
                {

                    if (Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.UP) != null)
                    {
                        Selected.GetComponent<Button>().setIsHighlighted(false);
                        Selected.GetComponent<Button>().setIsSelected(false);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.UP).GetComponent<Button>().setIsSelected(true);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.UP).GetComponent<Button>().setIsHighlighted(true);
                        Selected = Selected.GetComponent<Button>().getNeighbor((int)(NEIGHBORS.UP));
                        aud.PlayAudio("Menu_Dink1", AudioManager.AudioType.SFX);
                    }
                    else
                    {
                        aud.PlayAudio("Menu_Dink2", AudioManager.AudioType.SFX);
                    }
                }
                else
                {
                    Selected = FindObjectOfType<Button>().gameObject;
                    Selected.GetComponent<Button>().setIsHighlighted(true);
                    Selected.GetComponent<Button>().setIsSelected(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Selected != null)
                {
                    if (Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.LEFT) != null)
                    {
                        Selected.GetComponent<Button>().setIsHighlighted(false);
                        Selected.GetComponent<Button>().setIsSelected(false);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.LEFT).GetComponent<Button>().setIsSelected(true);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.LEFT).GetComponent<Button>().setIsHighlighted(true);
                        Selected = Selected.GetComponent<Button>().getNeighbor((int)(NEIGHBORS.LEFT));
                        aud.PlayAudio("Menu_Dink1", AudioManager.AudioType.SFX);
                    }
                    else
                    {
                        aud.PlayAudio("Menu_Dink2", AudioManager.AudioType.SFX);
                    }
                }
                else
                {
                    Selected = FindObjectOfType<Button>().gameObject;
                    Selected.GetComponent<Button>().setIsHighlighted(true);
                    Selected.GetComponent<Button>().setIsSelected(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Selected != null)
                {
                    if (Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.RIGHT) != null)
                    {
                        Selected.GetComponent<Button>().setIsHighlighted(false);
                        Selected.GetComponent<Button>().setIsSelected(false);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.RIGHT).GetComponent<Button>().setIsSelected(true);
                        Selected.GetComponent<Button>().getNeighbor((int)NEIGHBORS.RIGHT).GetComponent<Button>().setIsHighlighted(true);
                        Selected = Selected.GetComponent<Button>().getNeighbor((int)(NEIGHBORS.RIGHT));
                        aud.PlayAudio("Menu_Dink1", AudioManager.AudioType.SFX);
                    }
                    else
                    {
                        aud.PlayAudio("Menu_Dink2", AudioManager.AudioType.SFX);
                    }
                }
                else
                {
                    Selected = FindObjectOfType<Button>().gameObject;
                    Selected.GetComponent<Button>().setIsHighlighted(true);
                    Selected.GetComponent<Button>().setIsSelected(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (Selected != null)
                {
                    Selected.GetComponent<Button>().setIsClicked(true);
                    aud.PlayAudio("Menu_Click", AudioManager.AudioType.SFX);
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
                        aud.PlayAudio("Menu_Dink1", AudioManager.AudioType.SFX);
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
