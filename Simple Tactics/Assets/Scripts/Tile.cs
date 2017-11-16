using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    character thePlayer;
    bool passable;
    public enum tileType
    {
        inert, leyline, changeable
    }
    public enum tileEnergy
    {
         heat, cold, death, life, none
    }

    public tileEnergy thisTilesEnergy;
    
    tileEnergy getTileEnergy()
    {
        return thisTilesEnergy;
    }
    bool getTilePassable()
    {
        return passable;
    }
    void setTilePassable(bool _value)
    {
        passable = _value;
    }
    //0 = heat, 1 = cold, 2 = death, 3 = life, 4 = none
     void setTilesEnergy( int energyValue)
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
	// Use this for initialization
	void Start ()
    {
        setTilesEnergy(4);
        setTilePassable(true);
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

}
