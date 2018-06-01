using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Director : MonoBehaviour
{
    AudioManager au;
    Grid g;
    Character selectedCharacter;
    Tile selectedTile;
    Pathfinder pf;
    List<Tile> currentPath, currentAtkRange, currentMoveRange;
    Tile currentPathTile;
    int currentPathIndex;
    bool hasPath = false;
    public bool paused = false;
    Vector3 moveDir;
    List<Character> party;
    List<Enemy> enemies;
    Tactician t;

    public List<Character> Party
    {
        get
        {
            return party;
        }

        set
        {
            party = value;
        }
    }

    public List<Enemy> Enemies
    {
        get
        {
            return enemies;
        }

        set
        {
            enemies = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        pf = GameObject.Find("ScriptTester").GetComponent<Pathfinder>();
        pf.initializePathfinding();
        g = GameObject.Find("Grid").GetComponent<Grid>();
        au = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        t = GameObject.Find("ScriptTester").GetComponent<Tactician>();
        // au.playExampleBGM();
        party = GameObject.Find("ScriptTester").GetComponent<tempscript>().Party;
        enemies = GameObject.Find("ScriptTester").GetComponent<tempscript>().Enemies;
        for (int i = 0; i < party.Count; i++)
        {
            Tile allyTile = null;
            do { allyTile = g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)); } while (allyTile.thisTileTerrType == Tile.terrainType.environment);
            party[i].setCharacterTile(allyTile);
            party[i].name = "Char " + (i + 1);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            Tile enemyTile = null;
            do { enemyTile = g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)); } while (enemyTile.thisTileTerrType == Tile.terrainType.environment);
            enemies[i].setEnemyTile(g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)));
            enemies[i].name = "Enemy " + (i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {

            if (Input.GetKeyDown(KeyCode.Mouse0))
                selectObject();

            if (Input.GetKeyDown(KeyCode.Mouse1))
                deselectObjects();

            if (Input.GetKeyDown(KeyCode.E))
                runEnemyTurn();

            if (Input.GetKeyDown(KeyCode.R))
                findTargets();

            if (party.Count == 0 || enemies.Count == 0)
                SceneManager.LoadScene("mainMenu_01");
        }
    }

    private void FixedUpdate()
    {
        if (hasPath)
        {
            lerpCharacter();
        }
    }

    public void restartGame()
    {
        g.destroyGrid();
        g.buildGrid(g.Width, g.Height);
        pf.initializePathfinding();
        party = GameObject.Find("ScriptTester").GetComponent<tempscript>().Party;
        enemies = GameObject.Find("ScriptTester").GetComponent<tempscript>().Enemies;

        for (int i = 0; i < party.Count; i++)
        {
            Tile allyTile = null;
            do { allyTile = g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)); } while (allyTile.thisTileTerrType == Tile.terrainType.environment);
            party[i].setCharacterTile(allyTile);
            party[i].name = "Char " + (i + 1);
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            Tile enemyTile = null;
            do { enemyTile = g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)); } while (enemyTile.thisTileTerrType == Tile.terrainType.environment);
            enemies[i].setEnemyTile(g.getTileByRowCol(Random.Range(0, g.Height), Random.Range(0, g.Width)));
            enemies[i].name = "Enemy " + (i + 1);
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
                getAndHighlightAtkRange();
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
            else if (hit.collider.tag == "Enemy")
            {
                if (selectedCharacter != null)
                    attackEnemy(selectedCharacter, hit.collider.gameObject.GetComponent<Enemy>());
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
        else if (selectedCharacter.MoveRangeTiles.Contains(_hit.transform.gameObject.GetComponent<Tile>()) && !_hit.transform.gameObject.GetComponent<Tile>().occupied)
        {
            moveCharacter(_hit.transform.gameObject.GetComponent<Tile>());
        }

    }

    // Perform character-specific selection logic
    public void selectCharacter(RaycastHit _hit)
    {
        if (selectedCharacter != null)
            deselectCharacter();
        deselectTile();
        // Reset color of previously selected character
        if (selectedCharacter != null)
            selectedCharacter.setDefaultColor();

        selectedCharacter = _hit.transform.gameObject.GetComponent<Character>();
        selectedCharacter.setCurrentColor(Color.magenta);
        getAndHighlightAtkRange();
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
        {
            selectedCharacter.setDefaultColor();
            if (selectedCharacter.MoveRangeTiles != null)
                foreach (Tile t in selectedCharacter.MoveRangeTiles)
                    t.setDefaultColor();
            selectedCharacter.MoveRangeTiles.Clear();
            if (selectedCharacter.AtkRangeTiles != null)
                foreach (Tile t in selectedCharacter.AtkRangeTiles)
                    t.setDefaultColor();
            selectedCharacter.AtkRangeTiles.Clear();
        }
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

    public void getAndHighlightAtkRange()
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

    public void attackEnemy(Character _c, Enemy _e)
    {
        if (_c.AtkRangeTiles.Contains(_e.Location))
        {
            _e.CurrentHP -= _c.CurrentAttack;
        }
    }

    void runEnemyTurn()
    {
        party.RemoveAll(c => c == null);
        enemies.RemoveAll(e => e == null);
        foreach (Enemy e in enemies)
        {
            List<Tile> tempath = new List<Tile>();
            e.Target = t.setTarget(party, e);
            if (e.CurrentHP >= e.MaxHP * 0.3f)
            {

                if (e.Target != null)
                {
                    tempath = pf.getPath(e.Location, e.Target.Location);
                    if (tempath.Count > e.AtkRange)
                    {
                        tempath.Reverse();
                        Debug.Log("Path found!");
                        tempath.RemoveRange(e.AtkRange, tempath.Count - e.AtkRange);
                        foreach (Tile t in tempath)
                            if (!t.occupied)
                                e.setEnemyTile(t);
                    }
                    else
                    {
                        pf.getOuterAtkRange(e.AtkRange, e.Location);
                        e.AtkRangeTiles = pf.AtkRange;
                        if (e.AtkRangeTiles.Contains(e.Target.Location))
                            t.attack(e, e.Target);
                    }
                }
                else
                {
                    Debug.Log("Cannot find target!");
                }
            }
            else
            {
                pf.resetLists();
                pf.getMoveRange(e.MoveRange, e.Location);
                e.MoveRangeTiles = pf.MoveRange;
                int distance = 0;
                Tile target = e.Location;
                foreach (Tile t in e.MoveRangeTiles)
                {
                    if (!t.occupied)
                    {
                        tempath = pf.getPath(e.Target.Location, t);
                        if (tempath.Count > distance)
                        {
                            target = t;
                            distance = tempath.Count;
                        }
                    }
                }
                e.setEnemyTile(target);
            }
        }
        party.RemoveAll(c => c == null);
        enemies.RemoveAll(e => e == null);

    }

    void findTargets()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            //pf.getPath(enemies[i].Location, party[i].Location);
            enemies[i].Target = t.setTarget(party, enemies[i]);
        }
    }
}
