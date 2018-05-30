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
        g = GameObject.Find("Grid").GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            selectObject();

        if (Input.GetKeyDown(KeyCode.Mouse1))
            deselectObjects();

        if (Input.GetKeyDown(KeyCode.T))
            getAndHighlightRangedAtkRange();

        if(Input.GetKeyDown(KeyCode.Delete))
        {
            g.destroyGrid();
            g.buildGrid(20, 20);
            selectedCharacter.setCharacterTile(g.getTileByRowCol(0, 0));
            pf.initializePathfinding();
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
                foreach (Tile t in currentPath)
                    t.setDefaultColor();
                currentPath.Clear();
                getAndHighlightRangedAtkRange();
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
        if (!hasPath)
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
                selectedTile.setDefaultColor();
            }

            selectedTile = _hit.transform.gameObject.GetComponent<Tile>();
            selectedTile.setSelectedColor();
        }
        else if (selectedCharacter.MoveRangeTiles.Contains(_hit.transform.gameObject.GetComponent<Tile>()))
        {
            moveCharacter(_hit.transform.gameObject.GetComponent<Tile>());
        }

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
        getAndHighlightRangedAtkRange();
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
        if (selectedCharacter.MoveRangeTiles != null)
            foreach (Tile t in selectedCharacter.MoveRangeTiles)
                t.setDefaultColor();
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
        currentPath = pf.runLimitedAStar(selectedCharacter.MoveRangeTiles, selectedCharacter.Location, goal); // getPath(selectedCharacter.Location, goal);
        if (currentPath.Count > 0)
        {
            if (selectedCharacter.AtkRangeTiles != null)
                foreach (Tile t in selectedCharacter.AtkRangeTiles)
                    t.setDefaultColor();
            selectedCharacter.AtkRangeTiles.Clear();
            foreach (Tile t in selectedCharacter.MoveRangeTiles)
                t.setDefaultColor();
            foreach (Tile t in currentPath)
                t.setTemporaryColor(Color.yellow);
            currentPath.Reverse();
            hasPath = true;
            currentPathTile = currentPath[0];
            currentPathIndex = 0;
            selectedCharacter.transform.forward = (currentPathTile.transform.position - selectedCharacter.transform.position);
        }
    }

    public void getAndHighlightMoveRange()
    {
        // Clear the stored range
        if (selectedCharacter.MoveRangeTiles != null)
        {
            pf.MoveRange.Clear();
            selectedCharacter.MoveRangeTiles.Clear();
        }
        // Calculate and store the move range
        pf.getTestedMoveRange(selectedCharacter.MoveRange, selectedCharacter.Location);
        selectedCharacter.MoveRangeTiles = pf.MoveRange;

        // Highlight move range.
        foreach (Tile t in selectedCharacter.MoveRangeTiles)
            t.setTemporaryColor(Color.yellow);
    }

    public void getAndHighlightRangedAtkRange()
    {
        // Clear the stored range
        if (selectedCharacter.AtkRangeTiles != null)
        {
            selectedCharacter.AtkRangeTiles.Clear();
        }
        // Calculate and store the attack range
        pf.getOuterAtkRange(selectedCharacter.AtkRange, selectedCharacter.Location);
        selectedCharacter.AtkRangeTiles = pf.AtkRange;

        // Highlight the attack range
        foreach (Tile t in selectedCharacter.AtkRangeTiles)
            t.setTemporaryColor(Color.cyan);
    }
}
