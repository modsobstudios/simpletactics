using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    List<Tile> mapGrid;
    List<int> occupiedSpaces;
    int activeTile = -1;
    int width, height;
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

    public List<Tile> getGrid()
    {
        return mapGrid;
    }

    // returns random tile's world position
    public Vector3 getRandomTilePos()
    {
        int randMax = mapGrid.Count;
        bool trip = true;
        int index = Random.Range(0, randMax - 1);
        while (trip)
        {
            bool trap = false;
            foreach (int x in occupiedSpaces)
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
        return mapGrid[index].getTileWorldPos() + new Vector3(0, 0.95f, 0);
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
                Vector3 worldPos = new Vector3(0, 0, 0);
                worldPos.x += i;
                worldPos.z += j;
                newTile.setTileWorldPos(worldPos);
                int tileEnergy = Random.Range(0, 4);
                int tileType = Random.Range(0, 2);
                newTile.setTilesEnergy(tileEnergy);
                newTile.setTileType(tileType);


                switch ((Tile.tileEnergy)tileEnergy)
                {
                    case Tile.tileEnergy.heat:
                        {
                            _obj.GetComponent<SpriteRenderer>().material = _mats[0];

                            break;
                        }
                    case Tile.tileEnergy.cold:
                        {
                            _obj.GetComponent<SpriteRenderer>().material = _mats[1];

                            break;
                        }
                    case Tile.tileEnergy.death:
                        {
                            _obj.GetComponent<SpriteRenderer>().material = _mats[2];

                            break;
                        }
                    case Tile.tileEnergy.life:
                        {
                            _obj.GetComponent<SpriteRenderer>().material = _mats[3];

                            break;
                        }
                    default:
                        break;
                }
                GameObject tmp;
                tmp = Instantiate(_obj, worldPos, Quaternion.Euler(90.0f, 0.0f, 0.0f));
                tmp.transform.SetParent(this.transform);
                tmp.name = "Tile (" + i + ", " + j + ")";
                newTile.setMesh(tmp);
                tmp.GetComponent<Tile>().copyTile(newTile);
                tmp.GetComponent<Tile>().setMesh(tmp);
                mapGrid.Add(tmp.GetComponent<Tile>());
            }
        }
    }



    // functionality added to createGrid() to assist Tile/GameObject association
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
