﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    List<Tile> mapGrid;
    int width, height;
    public Material[] mats;
    public GameObject tileObj;


    // Use this for initialization
    void Start()
    {
        mapGrid = new List<Tile>();
        createGrid(10, 10,tileObj, mats);
        instantiateTheGrid(tileObj,mats);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public List<Tile> getGrid()
    {
        return mapGrid;
    }

    public Tile getTileByRowCol(int _row, int _col)
    {
        //Debug.Log("Given Row: " + _row + ", Given Col:" + _col);
        //Debug.Log("Actual Row: " + mapGrid[_row * width + _col].rowNum + ", Actual Col:" + mapGrid[_row * width + _col].columnNum);
        return mapGrid[_row * width + _col];
    }

    // Neighbor Getters
    // r * w + c = i
    public int verifyTileIndex(int _index)
    {
        return mapGrid[_index].getTileRow() * width + mapGrid[_index].getTileColumn();
    }

    // returns the north neighbor's index, takes the index of the tile in question
    // (r - 1) * w + c = i
    public int getNorthIndex(int _index)
    {
        if (mapGrid[_index].getTileRow() == 0)
            return -1;
        else
            return (mapGrid[_index].getTileRow() - 1) * width + mapGrid[_index].getTileColumn();
    }
    // returns the east neighbor's index, takes the index of the tile in question
    // r * w + (c - 1) = i
    public int getEastIndex(int _index)
    {
        if (mapGrid[_index].getTileColumn() == width - 1)
            return -1;
        else
            return mapGrid[_index].getTileRow() * width + (mapGrid[_index].getTileColumn() + 1);
    }
    // returns the south neighbor's index, takes the index of the tile in question
    // (r + 1) * w + c = i
    public int getSouthIndex(int _index)
    {
        if (mapGrid[_index].getTileRow() == height - 1)
            return -1;
        else
            return (mapGrid[_index].getTileRow() + 1) * width + mapGrid[_index].getTileColumn();
    }
    // returns the west neighbor's index, takes the index of the tile in question
    // r * w + (c + 1) = i
    public int getWestIndex(int _index)
    {
        if (mapGrid[_index].getTileColumn() == 0)
            return -1;
        else
            return mapGrid[_index].getTileRow() * width + (mapGrid[_index].getTileColumn() - 1);
    }

    public void createGrid(int rowNum, int columnNum, GameObject _obj, Material[] _mats)
    {
        width = columnNum;
        height = rowNum;
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < columnNum; j++)
            {
                Tile newTile = new Tile();
                newTile.setTileRowAndColumnNum(i, j);
                newTile.gridH = rowNum;
                newTile.gridW = columnNum;
                Vector3 worldPos = new Vector3(0, 0, 0);
                worldPos.x += i;
                worldPos.z += j;
                newTile.setTileWorldPos(worldPos);
                int tileEnergy = Random.Range(0, 4);
                int tileType = Random.Range(0, 2);
                newTile.setTileElement(tileEnergy);
                newTile.setTileTerrType(tileType);

                switch ((Tile.tileElement)tileEnergy)
                {
                    case Tile.tileElement.heat:
                        {
                            _obj.GetComponent<MeshRenderer>().material = _mats[0];

                            break;
                        }
                    case Tile.tileElement.cold:
                        {
                            _obj.GetComponent<MeshRenderer>().material = _mats[1];

                            break;
                        }
                    case Tile.tileElement.death:
                        {
                            _obj.GetComponent<MeshRenderer>().material = _mats[2];

                            break;
                        }
                    case Tile.tileElement.life:
                        {
                            _obj.GetComponent<MeshRenderer>().material = _mats[3];

                            break;
                        }
                    default:
                        break;
                }
                GameObject tmp;
                tmp = Instantiate(_obj, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
                tmp.transform.SetParent(this.transform);
                tmp.name = "Tile (" + i + ", " + j + ")";
                tmp.GetComponent<Tile>().copyTile(newTile);
                mapGrid.Add(tmp.GetComponent<Tile>());
                Destroy(tmp);
            }
        }
        for (int i = 0; i < mapGrid.Count; i++)
            mapGrid[i].gridIndex = i;
    }



    // functionality added to createGrid() to assist Tile/GameObject association
    public void instantiateTheGrid(GameObject _tileMarker, Material[] _materials)
    {
        for (int i = 0; i < mapGrid.Count; i++)
        {
            GameObject tmp;
            Vector3 worldPos = mapGrid[i].getTileWorldPos();
            _tileMarker.GetComponent<Tile>().copyTile(mapGrid[i]);
            if (mapGrid[i].getTileElement() == Tile.tileElement.heat)
            {
                _tileMarker.GetComponent<MeshRenderer>().material = _materials[0];
            }
            else if (mapGrid[i].getTileElement() == Tile.tileElement.cold)
            {
                _tileMarker.GetComponent<MeshRenderer>().material = _materials[1];
            }
            else if (mapGrid[i].getTileElement() == Tile.tileElement.death)
            {
                _tileMarker.GetComponent<MeshRenderer>().material = _materials[2];
            }
            else if (mapGrid[i].getTileElement() == Tile.tileElement.life)
            {
                _tileMarker.GetComponent<MeshRenderer>().material = _materials[3];
            }
            tmp = Instantiate(_tileMarker, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
            tmp.GetComponent<Tile>().copyTile(mapGrid[i]);
            tmp.transform.SetParent(this.transform);
            tmp.name = "Tile (" + mapGrid[i].getTileRowAndColumnNum().x + ", " + mapGrid[i].getTileRowAndColumnNum().y + ")";
        }

    }

    public int Width
    {
        get
        {
            return width;
        }

        set
        {
            width = value;
        }
    }

    public int Height
    {
        get
        {
            return height;
        }

        set
        {
            height = value;
        }
    }

}