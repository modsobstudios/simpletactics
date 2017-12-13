using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{


    List<Tile> mapGrid;
    public Material[] materials;
    public GameObject tileMarker;
    public GameObject player;
    List<GameObject> playerList;
    int width, height, numPlayers;
    // Use this for initialization
    void Start()
    {
        width = 25;
        height = 25;
        mapGrid = new List<Tile>();
        createGrid(width, height);
        instantiateTheGrid();
        spawnCharacters();
    }
 
    // Update is called once per frame
    void Update()
    {

    }

    void spawnCharacters()
    {
        numPlayers = 5;
        playerList = new List<GameObject>();
        for (int i = 0; i < numPlayers; i++)
        {
            Instantiate(player, getRandomTilePos(), Quaternion.identity);
            playerList.Add(player);
        }

    }

    Vector3 getRandomTilePos()
    {
        int randMax = mapGrid.Count;
        int index = Random.Range(0, randMax);
        return mapGrid[index].getTileWorldPos();
    }

    void createGrid(int rowNum, int columnNum)
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < columnNum; j++)
            {
                Tile newTile = new Tile();
                newTile.setTileRowAndColumnNum(i, j);
                Vector3 worldPos = new Vector3(0, 0, 0);
                worldPos.x += i;
                worldPos.z += j;
                newTile.setTileWorldPos(worldPos);
                int tileEnergy = Random.Range(0, 4);
                int tileType = Random.Range(0, 2);
                newTile.setTilesEnergy(tileEnergy);
                newTile.setTileType(tileType);
                mapGrid.Add(newTile);
            }
        }
    }

    void instantiateTheGrid()
    {
        for (int i = 0; i < mapGrid.Count; i++)
        {
            Vector3 worldPos = mapGrid[i].getTileWorldPos();
            tileMarker.GetComponent<Tile>().copyTile(mapGrid[i]);
            if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.heat)
            {
                tileMarker.GetComponent<SpriteRenderer>().material = materials[0];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.cold)
            {
                tileMarker.GetComponent<SpriteRenderer>().material = materials[1];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.death)
            {
                tileMarker.GetComponent<SpriteRenderer>().material = materials[2];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.life)
            {
                tileMarker.GetComponent<SpriteRenderer>().material = materials[3];
            }

            Instantiate(tileMarker, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
        }

    }


}
