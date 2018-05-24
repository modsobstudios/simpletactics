using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    Grid g;
    Character selectedCharacter;
    Tile selectedTile;
    Pathfinder pf;
    List<Tile> currentPath, currentAtkRange, currentMoveRange;
    Tile currentPathTile;
    int currentPathIndex;
    bool hasPath = false;

    // Use this for initialization
    void Start()
    {
        pf = GameObject.Find("ScriptTester").GetComponent<Pathfinder>();
        pf.initializePathfinding();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            selectObject();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            deselectObjects();

        if (Input.GetKeyDown(KeyCode.V))
        {
            pf = GameObject.Find("ScriptTester").GetComponent<Pathfinder>();
            pf.initializePathfinding();
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Getting Range...");
            pf.MoveRange.Clear();
            pf.recursivelyAddTilesInMoveRange(6, selectedCharacter.Location);
            currentMoveRange = pf.MoveRange;
            foreach(Tile t in currentMoveRange)
            {
                t.setTemporaryColor(Color.white);
            }
        }
        if(Input.GetKey(KeyCode.Y))
        {
            Debug.Log("Getting full range...");
            pf.MoveRange.Clear();
            pf.AtkRange.Clear();
            pf.moveRangeExtents.Clear();
            pf.getMoveAndAttackRange(3, 4, selectedCharacter.Location);
            foreach (Tile t in pf.AtkRange)
                t.setTemporaryColor(Color.cyan);
            foreach (Tile t in pf.MoveRange)
                t.setTemporaryColor(Color.yellow);

        }

        if(Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("Getting ranged attack range...");
            pf.AtkRange.Clear();
            pf.recursivelyAddTilesInAtkRangeFromMinimum(2, 6, selectedCharacter.Location);
            foreach (Tile t in pf.AtkRange)
                t.setTemporaryColor(Color.yellow);
        }
    }

    private void FixedUpdate()
    {
        if (hasPath)
        {
            lerpCharacter();
        }
    }

    private void lerpCharacter()
    {
        if (selectedCharacter.transform.position == currentPathTile.transform.position)
        {
            if (currentPathIndex == currentPath.Count - 1)
            {
                selectedCharacter.setCharacterTile(currentPathTile);
                hasPath = false;
            }
            else
            {
                currentPathTile = currentPath[++currentPathIndex];
                selectedCharacter.transform.forward = (currentPathTile.transform.position - selectedCharacter.transform.position);
            }
        }
        else
        {
            selectedCharacter.transform.position += ((currentPathTile.transform.position - selectedCharacter.transform.position) * Time.deltaTime * 25);
        }
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
            {
                selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();
                selectedTile.resetBaseColor();
            }

            selectedTile = _hit.transform.gameObject.GetComponent<Tile>();
            selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
            selectedTile.setBaseColor(Color.blue);
        }
        else
        {
            moveCharacter(_hit.transform.gameObject.GetComponent<Tile>());
        }
        Debug.Log(_hit.transform.gameObject.GetComponent<Tile>().getColor());
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

    public void moveCharacter(Tile goal)
    {
        currentPath = pf.getPath(selectedCharacter.Location, goal);
        currentPath.Reverse();
        hasPath = true;
        currentPathTile = currentPath[0];
        currentPathIndex = 0;
        selectedCharacter.transform.forward = (currentPathTile.transform.position - selectedCharacter.transform.position);
        //selectedCharacter.setCharacterTile(goal);
    }
}
