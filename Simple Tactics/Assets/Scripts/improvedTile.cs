using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class improvedTile : MonoBehaviour {

    Vector3 worldPos;
    public int columnNum ,rowNum, gridIndex, gridW, gridH;
    public tileElement thisTileElement;
    public terrainType thisTileTerrType;
    Vector3 tileOffset = new Vector3(37.5f, 7.5f, 0);
    MeshRenderer meshRend;
    Color color, inColor;

    //environment is non-passable terrain and playfield is all terrain in which the player can move to or move over
    public enum terrainType
    {
        environment, playfield
    }
    public enum tileElement
    {
        heat, cold, death, life, none
    }
    public tileElement getTileElement()
    {
        return thisTileElement;
    }
    public terrainType getTileTerrType()
    {
        return thisTileTerrType;
    }

    public void setTileElement(int newVal)
    {
        if (newVal >= 0 && newVal <= 4)
        {
            thisTileElement = (tileElement)newVal;
        }
        else
        {
            thisTileElement = (tileElement)4;
        }
    }
    public void setTileTerrType(int newVal)
    {
        if (newVal >= 0 & newVal <= 2)
        {
            thisTileTerrType = (terrainType)newVal;
        }
        else
        {
            thisTileTerrType = (terrainType)0;
        }
    }
    public int getTileRow()
    {
        return rowNum;
    }
    public int getTileColumn()
    {
        return columnNum;
    }
   
    public Vector3 getTileWorldPos()
    {
        return worldPos;
    }
    public void setTileRowAndColumnNum(int row, int column)
    {
        rowNum = row;
        columnNum = column;
    }
    public Vector2 getTileRowAndColumnNum()
    {
        return new Vector2(rowNum, columnNum);
    }
    public void setTileWorldPos(Vector3 newWorldPos)
    {
        worldPos = newWorldPos;
    }

    public void copyTile(improvedTile tileToCopy)
    {
        this.thisTileElement = tileToCopy.thisTileElement;
        this.thisTileTerrType = tileToCopy.thisTileTerrType;
        this.rowNum = tileToCopy.rowNum;
        this.columnNum = tileToCopy.columnNum;
        this.worldPos = tileToCopy.worldPos;
        this.gridH = tileToCopy.gridH;
        this.gridW = tileToCopy.gridW;
        this.gridIndex = tileToCopy.gridIndex;
    }
    // Use this for initialization
    void Start ()
    {
        meshRend = GetComponent<MeshRenderer>();
        color = meshRend.material.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        
    }
    private void OnMouseEnter()
    {
        // raise slightly to indicate selection
        this.transform.position += new Vector3(0, 0.15f, 0);

        // capture spot for tooltip
        Vector3 spot = Input.mousePosition + tileOffset;
    }

    // while mouse is hovering
    private void OnMouseOver()
    {

    }

    private void OnMouseExit()
    {
        // reset position
        this.transform.position -= new Vector3(0, 0.15f, 0);
    }

    private void OnMouseDown()
    {
       
    }

    
}
