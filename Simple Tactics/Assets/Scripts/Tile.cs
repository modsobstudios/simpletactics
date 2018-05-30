using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    Vector3 worldPos;
    public int columnNum ,rowNum, gridIndex, gridW, gridH;
    public tileElement thisTileElement;
    public terrainType thisTileTerrType;
    Vector3 tileOffset = new Vector3(37.5f, 7.5f, 0);
    MeshRenderer meshRend;
    Color color, baseColor, tempColor;
    float ratio = 0.0f;
    bool lerpBool = false;
    public bool selected = false;
    public int cost;
    public bool outOfRange = false;

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
    public Color getColor()
    {
        return color;
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

    public void copyTile(Tile tileToCopy)
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
        color = baseColor = tempColor = meshRend.material.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            meshRend = GetComponent<MeshRenderer>();
            color = meshRend.material.color;
            Debug.Log(color);
        }
    }
    private void OnMouseEnter()
    {
        // raise slightly to indicate selection
        //this.transform.position += new Vector3(0, 0.15f, 0);

        // capture spot for tooltip
        // Vector3 spot = Input.mousePosition + tileOffset;
    }

    // while mouse is hovering
    private void OnMouseOver()
    {

        meshRend.material.color = Color.Lerp(tempColor, Color.white, ratio);
        if (ratio <= 0.01f && lerpBool)
            lerpBool = false;
        if (ratio >= 0.99f && !lerpBool)
            lerpBool = true;
        if (!lerpBool) ratio += 0.05f;
        else ratio -= 0.05f;
        
    }

    private void OnMouseExit()
    {
        // reset position
        //this.transform.position -= new Vector3(0, 0.15f, 0);
        meshRend.material.color = tempColor;

    }

    private void OnMouseDown()
    {
       
    }


    public int getNorthIndex()
    {
        if (rowNum == 0)
            return -1;
        else
            return (rowNum - 1) * gridW + columnNum;
    }

    public int getEastIndex()
    {
        if (columnNum == gridW - 1)
            return -1;
        else
            return rowNum * gridW + (columnNum + 1);
    }

    public int getSouthIndex()
    {
        if (rowNum == gridH - 1)
            return -1;
        else
            return (rowNum + 1) * gridW + columnNum;
    }

    public int getWestIndex()
    {
        if (columnNum == 0)
            return -1;
        else
            return rowNum * gridW + (columnNum - 1);
    }

    public void enforceColoring()
    {
        meshRend = GetComponent<MeshRenderer>();
        color = meshRend.material.color;
    }

    public void setSelectedColor()
    {
        meshRend.materials[0].color = Color.blue;
        tempColor = Color.blue;
    }

    public void setDefaultColor()
    {
        meshRend.materials[0].color = color;
        tempColor = color;
    }

    public void setTemporaryColor(Color _c)
    {
        tempColor = _c; //Color.Lerp(color, _c, 0.7f);
        meshRend.materials[0].color = tempColor;
    }

    public void setBaseColor(Color _c)
    {
        color = _c;
        meshRend.materials[0].color = color;
    }
    
    public void resetBaseColor()
    {
        color = baseColor;
        meshRend.materials[0].color = color;

    }
}
