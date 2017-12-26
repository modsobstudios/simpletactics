﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{


    List<Tile> mapGrid;
    List<int> occupiedSpaces;
    // Use this for initialization
    void Start()
    {
        mapGrid = new List<Tile>();
        occupiedSpaces = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // returns random tile's world position
    public Vector3 getRandomTilePos()
    {
        int randMax = mapGrid.Count;
        bool trip = true;
        int index = Random.Range(0, randMax-1);
        while(trip)
        {
            bool trap = false;
            foreach(int x in occupiedSpaces)
            {
                if (x == index)
                {
                    index = Random.Range(0, randMax - 1);
                    trap = true;
                }
            }
            if (!trap)
                trip = false;
        }
        occupiedSpaces.Add(index);
        return mapGrid[index].getTileWorldPos() + new Vector3(0,0.95f,0);
    }

   public void createGrid(int rowNum, int columnNum)
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

    public void instantiateTheGrid(GameObject _tileMarker, Material[] _materials)
    {
        for (int i = 0; i < mapGrid.Count; i++)
        {
            GameObject tmp;
            Vector3 worldPos = mapGrid[i].getTileWorldPos();
            _tileMarker.GetComponent<Tile>().copyTile(mapGrid[i]);
            if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.heat)
            {
                _tileMarker.GetComponent<SpriteRenderer>().material = _materials[0];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.cold)
            {
                _tileMarker.GetComponent<SpriteRenderer>().material = _materials[1];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.death)
            {
                _tileMarker.GetComponent<SpriteRenderer>().material = _materials[2];
            }
            else if (mapGrid[i].getTileEnergy() == Tile.tileEnergy.life)
            {
                _tileMarker.GetComponent<SpriteRenderer>().material = _materials[3];
            }
            tmp = Instantiate(_tileMarker, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
            tmp.GetComponent<Tile>().copyTile(mapGrid[i]);
            tmp.transform.SetParent(this.transform);
            tmp.name = "Tile (" + mapGrid[i].getTileRowAndColumnNum().x + ", " + mapGrid[i].getTileRowAndColumnNum().y + ")";
        }

    }


}
