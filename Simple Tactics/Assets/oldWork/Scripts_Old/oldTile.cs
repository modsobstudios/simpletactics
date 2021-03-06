﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oldTile : MonoBehaviour
{

    oldCharacter thePlayer;
    bool passable;
    public bool selected = false;
    public bool isNeighbor = false;
    Vector3 worldPos;
    int tileRowNum;
    int tileColumnNum;
    public int gridIndex, gridW, gridH;
    public GameObject tiletip, tileMesh;
    Vector3 tileOffset = new Vector3(37.5f, 7.5f, 0);
    SpriteRenderer spriteRend;
    Color color, inColor;

    public enum tileType
    {
        inert, leyline, changeable
    }
    public enum tileEnergy
    {
        heat, cold, death, life, none
    }

    public tileEnergy thisTilesEnergy;
    tileEnergy tempEnergy;
    public tileType thisTilesType;

    public tileEnergy getTileEnergy()
    {
        return thisTilesEnergy;
    }
    public tileType getTileType()
    {
        return thisTilesType;
    }
    bool getTilePassable()
    {
        return passable;
    }
    public int getTileRow()
    {
        return tileRowNum;
    }
    public int getTileColumn()
    {
        return tileColumnNum;
    }
    public GameObject getMesh()
    {
        return tileMesh;
    }
    public void setMesh(GameObject _o)
    {
        tileMesh = _o;
    }
    public Vector3 getTileWorldPos()
    {
        return worldPos;
    }
    public void setTileRowAndColumnNum(int row, int column)
    {
        tileRowNum = row;
        tileColumnNum = column;
    }
    public Vector2 getTileRowAndColumnNum()
    {
        return new Vector2(tileRowNum, tileColumnNum);
    }
    public void setTileWorldPos(Vector3 newWorldPos)
    {
        worldPos = newWorldPos;
    }

    public void setTilePassable(bool _value)
    {
        passable = _value;
    }
    //0 = heat, 1 = cold, 2 = death, 3 = life, 4 = none
    public void setTilesEnergy(int energyValue)
    {
        if (energyValue >= 0 && energyValue <= 4)
        {
            thisTilesEnergy = (tileEnergy)energyValue;
        }
        else
        {
            thisTilesEnergy = (tileEnergy)4;
        }

    }
    public void setTileType(int typeValue)
    {
        if (typeValue >= 0 & typeValue <= 2)
        {
            thisTilesType = (tileType)typeValue;
        }
        else
        {
            thisTilesType = (tileType)0;
        }
    }
    public void copyTile(oldTile tileToCopy)
    {
        this.thisTilesEnergy = tileToCopy.thisTilesEnergy;
        this.thisTilesType = tileToCopy.thisTilesType;
        this.passable = tileToCopy.passable;
        this.tileRowNum = tileToCopy.tileRowNum;
        this.tileColumnNum = tileToCopy.tileColumnNum;
        this.worldPos = tileToCopy.worldPos;
        this.gridH = tileToCopy.gridH;
        this.gridW = tileToCopy.gridW;
        this.gridIndex = tileToCopy.gridIndex;
    }


    // Use this for initialization
    void Start()
    {
        // setTilesEnergy(4);
        //setTilePassable(true);
        spriteRend = GetComponent<SpriteRenderer>();
        color = spriteRend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            spriteRend.material.color = Color.blue;
        }
        else if (isNeighbor)
            spriteRend.material.color = inColor;
        else
            spriteRend.material.color = color;
    }

    private void OnMouseEnter()
    {
        // raise slightly to indicate selection
        this.transform.position += new Vector3(0, 0.15f, 0);

        // capture spot for tooltip
        Vector3 spot = Input.mousePosition + tileOffset;
        // create the thing
        tiletip = Instantiate(Resources.Load("tiletip", typeof(GameObject)) as GameObject, spot, Quaternion.identity);
        // assign to canvas
        tiletip.transform.SetParent(FindObjectOfType<Canvas>().transform);
        // set text
        tiletip.GetComponentInChildren<Text>().text = getTypeString() + " (" + tileRowNum + ", " + tileColumnNum + ")";

    }

    // while mouse is hovering
    private void OnMouseOver()
    {
        // maintain position
        tiletip.transform.position = Input.mousePosition + tileOffset;
    }

    private void OnMouseExit()
    {
        // reset position
        this.transform.position -= new Vector3(0, 0.15f, 0);
        // remove tooltip
        Destroy(tiletip);
    }

    private void OnMouseDown()
    {
        selected = true;
    }

    private string getTypeString()
    {
        switch (thisTilesEnergy)
        {
            case tileEnergy.cold:
                {
                    return "Cold";
                }
            case tileEnergy.death:
                {
                    return "Death";
                }
            case tileEnergy.heat:
                {
                    return "Heat";
                }
            case tileEnergy.life:
                {
                    return "Life";
                }
            default:
                return "ERR";
        }
    }

    public void changeColor(Color _c)
    {
        inColor = _c;
        isNeighbor = true;
    }
    public void resetColor()
    {
        isNeighbor = false;
    }

    public int getNorthIndex()
    {
        if (tileRowNum == 0)
            return -1;
        else
            return (tileRowNum - 1) * gridW + tileColumnNum;
    }

    public int getEastIndex()
    {
        if (tileColumnNum == gridW - 1)
            return -1;
        else
            return tileRowNum * gridW + (tileColumnNum + 1);
    }

    public int getSouthIndex()
    {
        if (tileRowNum == gridH - 1)
            return -1;
        else
            return (tileRowNum + 1) * gridW + tileColumnNum;
    }

    public int getWestIndex()
    {
        if (tileColumnNum == 0)
            return -1;
        else
            return tileRowNum * gridW + (tileColumnNum - 1);
    }
}
