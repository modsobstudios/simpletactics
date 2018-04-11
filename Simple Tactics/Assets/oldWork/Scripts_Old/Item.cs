using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    enum ItemType { EQUIPMENT, CONSUMABLE, JUNK, NUMITEMTYPES, };

    int itemID;
    GameObject itemModel, menuCard;
    ItemType itemType;
    string itemName, flavorText;


    // Use this for initialization
    void Start()
    {
        // Instantiate the item with a specific ID
        // Populate item data from a library based on itemID
        // Save space by not having a hundred prefabs
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getItemID()
    {
        return itemID;
    }

    protected void setItemID(int _ID)
    {
        itemID = _ID;
    }
    

    public string getItemName()
    {
        return itemName;
    }

    protected void setItemName(string _name)
    {
        itemName = _name;
    }


    public string getFlavorText()
    {
        return flavorText;
    }

    protected void setFlavorText(string _text)
    {
        flavorText = _text;
    }


    public GameObject getMenuCard()
    {
        return menuCard;
    }

    protected void setMenuCard(GameObject _card)
    {
        menuCard = _card;
    }


}
