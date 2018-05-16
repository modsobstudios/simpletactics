using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    Grid g;
    Character selectedCharacter;
    Tile selectedTile;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            selectObject();

        if(Input.GetKeyDown(KeyCode.Mouse1))
            deselectObjects();

    }

    // Performs the raycasting to detect objects and place them in selection
    private void selectObject()
    {
        // Do raycasting from camera to world mouse point
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        // Null check
        if (hit.collider == null)
        {
            deselectObjects();
        }
        // Parse tags 
        else if (hit.collider.tag == "Character")
        {
            selectCharacter(hit);
        }
        else if (hit.collider.tag == "Tile")
        {
            selectTile(hit);
        }
        // If not selectable, deselect any selected objects.
        else
        {
            deselectObjects();
        }

    }

    // Perform tile-specific selection logic
    // Currently also performs character movement
    // TODO: Abstract character movement. (Likely to come with UI and combat system)
    public void selectTile(RaycastHit _hit)
    {
        if (selectedCharacter == null)
        {
            // Reset color of previously selected tile
            if (selectedTile != null)
                selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();

            selectedTile = _hit.transform.gameObject.GetComponent<Tile>();
            selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
        }
        else
        {
            selectedCharacter.setCharacterTile(_hit.transform.gameObject.GetComponent<Tile>());
        }
    }

    // Perform character-specific selection logic
    public void selectCharacter(RaycastHit _hit)
    {
        deselectTile();
        // Reset color of previously selected character
        if (selectedCharacter != null)
            selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.white;

        selectedCharacter = _hit.transform.gameObject.GetComponent<Character>();
        selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.magenta;
    }

    // Deselect all selected objects
    public void deselectObjects()
    {
        deselectTile();
        deselectCharacter();
    }

    // Deselect character only
    public void deselectCharacter()
    {
        if (selectedCharacter != null)
            selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.white;
        selectedCharacter = null;
    }

    // Deselect character only
    public void deselectTile()
    {
        if (selectedTile != null)
            selectedTile.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();
        selectedTile = null;
    }
}
