using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    List<Tile> mapGrid;
    public GameObject tileMarker;
	// Use this for initialization
	void Start ()
    {
        mapGrid = new List<Tile>();
        createGrid(5, 5);
        instantiateTheGrid();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void createGrid(int rowNum, int columnNum)
    {
        for(int i=0; i< rowNum; i++)
        {
            for(int j=0; j< columnNum; j++)
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
        for(int i= 0; i< mapGrid.Count; i++)
        {
            Vector3 worldPos = mapGrid[i].getTileWorldPos();
            
            Instantiate(tileMarker, worldPos,Quaternion.identity);
        }
        
    }


}
