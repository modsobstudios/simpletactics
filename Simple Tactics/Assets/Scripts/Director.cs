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
    Vector3 moveDir;

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

        if (Input.GetKeyDown(KeyCode.T))
            getFullRange(selectedCharacter);
    }

    private void FixedUpdate()
    {
        if (hasPath)
        {
            lerpCharacter();
        }
    }

    public void getFullRange(Character _c)
    {
        Debug.Log("Getting full range...");
        pf.MoveRange.Clear();
        pf.AtkRange.Clear();
        pf.innerAtkExtents.Clear();
        pf.moveRangeExtents.Clear();
        pf.getMoveAndRangedAttackRange(_c.MinAtkRange, _c.AtkRange, _c.MoveRange, _c.Location);
        foreach (Tile t in pf.MoveRange)
            t.setTemporaryColor(Color.yellow);
        foreach (Tile t in pf.AtkRange)
            t.setTemporaryColor(Color.cyan);
    }

    private void lerpCharacter()
    {
        if (moveDir == Vector3.zero)
        {
            moveDir = (currentPathTile.transform.position - selectedCharacter.transform.position).normalized;
        }
        if (selectedCharacter.transform.position == currentPathTile.transform.position)
        {
            if (currentPathIndex == currentPath.Count - 1)
            {
                selectedCharacter.setCharacterTile(currentPathTile);
                hasPath = false;
                moveDir = Vector3.zero;
                getAndHighlightMoveRange();
            }
            else
            {
                currentPathTile = currentPath[++currentPathIndex];
                selectedCharacter.transform.forward = moveDir = (currentPathTile.transform.position - selectedCharacter.transform.position).normalized;
            }
        }
        else
        {
            selectedCharacter.transform.position += (moveDir * Time.deltaTime * 3);
            if (Vector3.Distance(currentPathTile.transform.position, selectedCharacter.transform.position) <= 0.05f)
                selectedCharacter.transform.position = currentPathTile.transform.position;
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
        if (selectedTile != null)
            selectedTile.selected = false;
        _hit.transform.gameObject.GetComponent<Tile>().selected = true;
        if (selectedCharacter == null)
        {
            // Reset color of previously selected tile
            if (selectedTile != null)
            {
                //selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();
                selectedTile.setDefaultColor();
            }

            selectedTile = _hit.transform.gameObject.GetComponent<Tile>();
            //selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
            selectedTile.setSelectedColor();
        }
        else if (!hasPath && selectedCharacter.MoveRangeTiles.Contains(_hit.transform.gameObject.GetComponent<Tile>()))
        {
            foreach (Tile t in selectedCharacter.MoveRangeTiles)
                t.setDefaultColor();
            moveCharacter(_hit.transform.gameObject.GetComponent<Tile>());
        }
        //Debug.Log(_hit.transform.gameObject.GetComponent<Tile>().getColor());
    }

    // Perform character-specific selection logic
    public void selectCharacter(RaycastHit _hit)
    {
        deselectTile();
        // Reset color of previously selected character
        if (selectedCharacter != null)
            selectedCharacter.setDefaultColor();

        selectedCharacter = _hit.transform.gameObject.GetComponent<Character>();
        selectedCharacter.setCurrentColor(Color.magenta);
        getAndHighlightMoveRange();
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
            selectedCharacter.setDefaultColor();
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

    public void getAndHighlightMoveRange()
    {
        if (selectedCharacter.MoveRangeTiles != null)
        {
            foreach (Tile t in selectedCharacter.MoveRangeTiles)
                t.setDefaultColor();
            selectedCharacter.MoveRangeTiles.Clear();
        }
        pf.getMoveAndAttackRange(selectedCharacter.MoveRange, selectedCharacter.AtkRange, selectedCharacter.Location);
        selectedCharacter.AtkRangeTiles = pf.AtkRange;
        selectedCharacter.MoveRangeTiles = pf.MoveRange;
        foreach (Tile t in selectedCharacter.MoveRangeTiles)
            t.setTemporaryColor(Color.yellow);
    }
}
