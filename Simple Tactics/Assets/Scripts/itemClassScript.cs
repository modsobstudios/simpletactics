using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemClassScript : MonoBehaviour
{
    enum ItemType { EQUIPMENT, CONSUMABLE, JUNK, NUMITEMTYPES, };

    int itemID;
    GameObject itemModel, menuCard;
    ItemType itemType;
    string flavorText;

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
}
