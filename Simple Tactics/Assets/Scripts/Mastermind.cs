using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mastermind is a grid/party/movement/combat manager. It works in conjunction with the Phase/Turn manager to
// handle world interaction between the character pawns and the grid system.

/// Comments in this format are placeholders for future content pipelines.

// Mastermind requires the following components:
[RequireComponent(typeof(Party))]
[RequireComponent(typeof(Grid))]
[RequireComponent(typeof(shoulderCamScript))]
[RequireComponent(typeof(godCamScript))]

public class Mastermind : MonoBehaviour
{
    // Required components
    Party partyObj;
    Grid gridObj;
    shoulderCamScript shoulderCam;
    godCamScript godCam;


    // Required components for required components
    public GameObject playerPrefab;
    public Material[] gridMaterials;
    public GameObject tileObj;
    public Camera mainCamera;

    // Variables
    List<character> characterList;
    List<Tile> mapGrid;

    int activeChar = -1;
    int activeTile = -1;
    int targetChar = -1;
    int targetTile = -1;

    public int gridHeight, gridWidth, numPlayers;
    public int target = 0;
    bool moveChar = false;
    void Start()
    {
        // Required components
        partyObj = GetComponent<Party>();
        gridObj = GetComponent<Grid>();
        shoulderCam = GetComponent<shoulderCamScript>();
        godCam = GetComponent<godCamScript>();
        godCam.cam = mainCamera;
        godCam.cam.transform.Rotate(new Vector3(45, 0, 0));
        shoulderCam.cam = mainCamera;
    }

    void Update()
    {
        // load the grid and players
        if(Input.GetKeyDown(KeyCode.P))
        {
            createCombat();
            setupCamera();
        }

        if(Input.GetButtonUp("Fire1"))
        {
            runSelection();
            if(activeChar != -1)
                focusTarget();
            if(moveChar)
            {
                if(activeTile != -1)
                {
                    teleportChar(activeTile);
                    moveChar = false;
                }
            }
        }
        
        if(Input.GetKeyDown(KeyCode.M))
        {
            beginMove();
        }

        #region Camera Keys
        // select next character
        //if(Input.GetKeyDown(KeyCode.H))
        //{
        //    // advance target
        //    target++;
        //    // reset if over limit
        //    if (target >= characterList.Count)
        //        target = 0;
        //    // call cameras
        //    switchTarget();
        //}
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    // decrease target
        //    target--;
        //    // reset if under limit
        //    if (target < 0)
        //        target = characterList.Count - 1;
        //    // call cameras
        //    switchTarget();
        //}
        #endregion
    }

    void teleportChar(int _tile)
    {
        characterList[activeChar].moveTo(mapGrid[_tile].getTileWorldPos());
    }

    public void beginMove()
    {
        activeTile = -1;
        foreach (Tile t in mapGrid)
        {
            t.selected = false;
        }
        moveChar = true;
    }
    void runSelection()
    {
        for(int i = 0; i < characterList.Count; i++)
        {
            if(characterList[i].selected && i != activeChar)
            {
                if(activeChar != -1)
                    characterList[activeChar].selected = false;
                activeChar = i;
            }
        }
        for (int i = 0; i < mapGrid.Count; i++)
        {
            if (mapGrid[i].selected && i != activeTile)
            {
                if (activeTile != -1)
                    mapGrid[activeTile].selected = false;
                activeTile = i;
            }
        }

    }

    void createCombat()
    {
        /// switch on saved or new map
        /// if new map, generate a new map
        /// if saved, load saved map
        // create the grid
            // create tile obj
            /// load tile info from saved map
            /// or new map
            // fill tile info
            // add to array
        // instantiate the grid
            // copy tile information
            // apply material
            // instantiate tile
        // modify the grid
            // place starting locations
            // define impassable terrain
            /// load info from saved map
            /// or new map
            /// define leylines
            /// define rooms
        createGrid();

        // create the characters
            // create character obj
            /// load character from saved party
            /// or generate new party
            // fill character obj
            // load into array
        // create the party
            // create party obj
            /// load info from saved party
            /// or new party
            /// fill item inventory
            /// fill currency
        createParty();

        // populate the grid
            /// load info from saved map
            /// or new map
            // instantiate player pawns
            // place player pawns
            /// instantiate enemies
            /// instantiate items
            /// instantiate lore
            /// place items
            /// place enemies
            /// place lore
    }

    // generates a list of characters and player objects
    void createParty()
    {
        partyObj.generateParty(gridObj, playerPrefab, numPlayers);
        characterList = partyObj.getCharList();
    }

    // generates the grid
    void createGrid()
    {
        gridObj.createGrid(gridHeight, gridWidth, tileObj, gridMaterials);
    //    gridObj.instantiateTheGrid(tileObj, gridMaterials);
        mapGrid = gridObj.getGrid();
    }

    // initializes the cameras
    void setupCamera()
    {
        godCam.charList = characterList;
        shoulderCam.charList = characterList;
        godCam.camerasOn = shoulderCam.camerasOn = true;
    }

    // changes target for both cameras 
    void switchTarget()
    {
        shoulderCam.target = godCam.target = target;
        if (shoulderCam.shoulderCamActive)
            shoulderCam.switchTarget();
        else
            godCam.switchTarget();
    }

    void focusTarget()
    {
        godCam.target = shoulderCam.target = activeChar;
        if (shoulderCam.shoulderCamActive)
            shoulderCam.switchTarget();
        else
            godCam.switchTarget();
    }

}
