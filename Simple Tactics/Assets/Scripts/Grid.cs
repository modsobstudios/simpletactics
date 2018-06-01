using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    List<Tile> mapGrid;
    int width, height;
    public Material[] mats;
    public GameObject tileObj;
    public Material plainMat;
    public List<GameObject> envObjs;

    // Use this for initialization
    void Start()
    {
        mapGrid = new List<Tile>();
        //createGrid(10, 10, tileObj, mats);
        //instantiateTheGrid(tileObj, mats);
        envObjs = new List<GameObject>();
        buildGrid(20, 20);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(envObjs.Count);
    }

    public void destroyGrid()
    {
        foreach (Tile t in mapGrid)
            Destroy(t.gameObject);
        envObjs.Clear();
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
                if (tileType == (int)Tile.terrainType.playfield)
                {

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
                }
                else
                {
                    _obj.GetComponent<MeshRenderer>().material = plainMat;

                }
                GameObject tmp;
                tmp = Instantiate(_obj, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
                tmp.transform.SetParent(this.transform);
                tmp.name = "Tile (" + i + ", " + j + ")";
                tmp.GetComponent<Tile>().copyTile(newTile);
                mapGrid.Add(tmp.GetComponent<Tile>());
                //Destroy(tmp);
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

    public void buildGrid(int numRows, int numCols)
    {
        width = numRows;
        height = numCols;
        mapGrid = new List<Tile>();
        for (int r = 0; r < width; r++)
        {
            for (int c = 0; c < height; c++)
            {
                Vector3 worldPos = new Vector3(r, 0, c);
                GameObject tmp = Instantiate(tileObj, worldPos, Quaternion.Euler(90.0f, 0, 0));
                tmp.transform.SetParent(transform);
                tmp.name = "Tile (" + r + ", " + c + ")";
                Tile t = tmp.GetComponent<Tile>();
                t.setTileRowAndColumnNum(r, c);
                t.gridH = height;
                t.gridW = width;
                t.setTileWorldPos(worldPos);
                int tileElement = Random.Range(-5, 4);
                if (tileElement < 0)
                    t.setTileElement(5);
                else
                    t.setTileElement(tileElement);

                int tileType = Random.Range(-4, 2);
                if (tileType <= 0)
                    t.setTileTerrType(1);
                else
                    t.setTileTerrType(0);
                if (t.getTileTerrType() == Tile.terrainType.playfield)
                {
                    t.cost = 0;
                    switch (t.getTileElement())
                    {
                        case Tile.tileElement.heat:
                        {
                            tmp.GetComponent<MeshRenderer>().material = mats[0];
                            break;
                        }
                        case Tile.tileElement.cold:
                        {
                            tmp.GetComponent<MeshRenderer>().material = mats[1];
                            break;
                        }
                        case Tile.tileElement.death:
                        {
                            tmp.GetComponent<MeshRenderer>().material = mats[2];
                            break;
                        }
                        case Tile.tileElement.life:
                        {
                            tmp.GetComponent<MeshRenderer>().material = mats[3];
                            break;
                        }
                        case Tile.tileElement.none:
                        {
                            tmp.GetComponent<MeshRenderer>().material = mats[4];
                            break;
                        }
                        default:
                        {
                            Debug.Log("Invalid Element.");
                            break;
                        }
                    }
                }
                else
                {
                    tmp.GetComponent<MeshRenderer>().material = plainMat;
                    t.cost = int.MaxValue;
                    GameObject x = Instantiate(Resources.Load<GameObject>("Cube"), tmp.transform.position + new Vector3(0,0.5f,0), Quaternion.identity);
                    x.transform.parent = tmp.transform;
                    x.tag = "Environment Object";
                    x.name = "Wall (" + t.rowNum + ", " + t.columnNum + ")";
                    envObjs.Add(x);
                    //Debug.Log(envObjs.Count);
                }
                mapGrid.Add(t);
            }
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
