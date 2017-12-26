using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{

    character thePlayer;
    bool passable;
    Vector3 worldPos;
    int tileRowNum;
    int tileColumnNum;
    GameObject tiletip;
    Vector3 tileOffset = new Vector3(25, 7.5f, 0);

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
    int getTileRow()
    {
        return tileRowNum;
    }
    int getTileColumn()
    {
        return tileColumnNum;
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
    public void setTilesEnergy( int energyValue)
    {
        if(energyValue >= 0 && energyValue <= 4)
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
        if(typeValue >=0 & typeValue <=2)
        {
            thisTilesType = (tileType)typeValue;
        }
        else
        {
            thisTilesType = (tileType)0;
        }
    }
    public void copyTile(Tile tileToCopy)
    {
        this.thisTilesEnergy = tileToCopy.thisTilesEnergy;
        this.thisTilesType = tileToCopy.thisTilesType;
        this.passable = tileToCopy.passable;
        this.tileRowNum = tileToCopy.tileRowNum;
        this.tileColumnNum = tileToCopy.tileColumnNum;
        this.worldPos = tileToCopy.worldPos;
        
    }

    // Use this for initialization
    void Start ()
    {
       // setTilesEnergy(4);
        //setTilePassable(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    private void OnMouseEnter()
    {
        Debug.Log(this.name + "was moused.");
        // raise slightly to indicate selection
        this.transform.position += new Vector3(0, 0.15f, 0);

        // capture spot for tooltip
        Vector3 spot = Input.mousePosition + tileOffset;
        // create the thing
        tiletip = Instantiate(Resources.Load("tiletip", typeof(GameObject)) as GameObject, spot, Quaternion.identity);
        // assign to canvas
        tiletip.transform.SetParent(FindObjectOfType<Canvas>().transform);
        // set text
        tiletip.GetComponentInChildren<Text>().text = "(" + tileRowNum + ", " + tileColumnNum + ")";

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
}
