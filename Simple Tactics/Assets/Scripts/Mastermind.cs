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

    public int gridHeight, gridWidth, numPlayers;


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
        //moveScript = GetComponent<freeMovement>();
        //moveScript.gCam = godCam;
        //moveScript.sCam = shoulderCam;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            createCombat();
            setupCamera();
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

    void createParty()
    {
        partyObj.generateParty(gridObj, playerPrefab, numPlayers);
        int x;
        x = 2;
        x = x + x * x;
        characterList = partyObj.getCharList();
    }

    void createGrid()
    {
        gridObj.createGrid(gridHeight, gridWidth);
        gridObj.instantiateTheGrid(tileObj, gridMaterials);
    }

    void setupCamera()
    {
        godCam.charList = characterList;
        shoulderCam.charList = characterList;
        godCam.camerasOn = shoulderCam.camerasOn = true;
    }
}
