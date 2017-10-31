using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Equipment : Item
{
    enum EquipSlot { ARMOR, BOOTS, WEAPON, NUMEQUIPS, };

    EquipSlot equipType;

    // assert( atkMod + defMod + spdMod == totalMod )
    int totalMod, atkMod, defMod, spdMod;

    // equipment status effects?


    // ----------- Debug testing ------------ //
    string element, eqType;
    public int playerLevel;
    public int enemyDifficulty;
    public int difficultyMod;
    int numCommon = 0;
    int numUncommon = 0;
    int numRare = 0;
    int numLegendary = 0;
    string output;
    public class Enemy
    {
        public int gearScore, enemyDifficulty;
        public string armor, weapon, boots;
    }
    public class Room
    {
        public int roomDifficulty;
        public int numEnemies;
        public List<Enemy> enemies;
    }
    List<Room> roomList = new List<Room>();

    // --------------------------------------//

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            numCommon = numUncommon = numRare = numLegendary = 0;
            for (int i = 0; i < 20; i++)
            {
                output = "";
                //playerLevel = Random.Range(1, 11);
                enemyDifficulty = Random.Range(0, 15);
                output += "Player Level: " + playerLevel + ", Enemy Difficulty: " + enemyDifficulty + "\n";
                equipGen();
                statGen(playerLevel, enemyDifficulty);
                if (totalMod < 0)
                    output += totalMod + " " + element + " " + eqType + " -- " + "ATK: " + atkMod + " DEF: " + defMod + " SPD: " + spdMod;
                else
                    output += "+" + totalMod + " " + element + " " + eqType + " -- " + "ATK: " + atkMod + " DEF: " + defMod + " SPD: " + spdMod;
                Debug.Log(output);
            }
            Debug.Log("Spread: Common: " + numCommon + " Uncommon: " + numUncommon + " Rare: " + numRare + " Legendary: " + numLegendary);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            string path = "Assets/Resources/output.txt";
            StreamWriter writer = new StreamWriter(path, true);
            roomGen(playerLevel, difficultyMod);
            writer.WriteLine("Rooms in Floor: " + roomList.Count);
            foreach (Room room in roomList)
            {
                writer.WriteLine("Room Difficulty: " + room.roomDifficulty + ", " + room.numEnemies + " Enemies.");
                foreach (Enemy enemy in room.enemies)
                {
                    writer.WriteLine("\t Enemy Difficulty: " + enemy.enemyDifficulty + ", Gear Score: " + enemy.gearScore + ".");
                    writer.WriteLine("\t\t Gear: \r\n\t\t\t" + enemy.armor + "\r\n\t\t\t " + enemy.boots + "\r\n\t\t\t " + enemy.weapon + ". ");
                }
            }
           // Debug.Log("Dump complete.");
        }
    }

    public int getTotalMod()
    {
        return totalMod;
    }
    public int getAtkMod()
    {
        return atkMod;
    }
    public int getDefMod()
    {
        return defMod;
    }
    public int getSpdMod()
    {
        return spdMod;
    }

    private void rollStats(int _statMin, int _statMax, int _statTotal)
    {
        int atk, def, spd, tot;
        atk = Random.Range(_statMin, _statMax);
        def = Random.Range(_statMin, _statMax);
        spd = Random.Range(_statMin, _statMax);
        tot = atk + spd + def;
        while (tot != _statTotal)
        {
            if (tot > _statTotal)
            {
                int flip = Random.Range(0, 2);
                switch (flip)
                {
                    case 0:
                        atk -= 1;
                        break;
                    case 1:
                        def -= 1;
                        break;
                    case 2:
                        spd -= 1;
                        break;
                    default:
                        Debug.Log("rollStats() tot > _statTotal switch hit default.");
                        break;
                }
            }
            if (tot < _statTotal)
            {
                int flip = Random.Range(0, 2);
                switch (flip)
                {
                    case 0:
                        atk += 1;
                        break;
                    case 1:
                        def += 1;
                        break;
                    case 2:
                        spd += 1;
                        break;
                    default:
                        Debug.Log("rollStats() tot < _statTotal switch hit default.");
                        break;
                }
            }

            if (atk > _statMax)
                atk = _statMax;
            if (def > _statMax)
                def = _statMax;
            if (spd > _statMax)
                spd = _statMax;

            if (atk < _statMin)
                atk = _statMin;
            if (def < _statMin)
                def = _statMin;
            if (spd < _statMin)
                spd = _statMin;

            tot = atk + def + spd;
        }

        atkMod = atk;
        defMod = def;
        spdMod = spd;
        totalMod = _statTotal;
    }

    private void equipGen()
    {
        int ele = Random.Range(0, 4);
        int equip = Random.Range(0, 3);

        switch (ele)
        {
            case 0:
                element = "Heat";
                break;
            case 1:
                element = "Cold";
                break;
            case 2:
                element = "Life";
                break;
            case 3:
                element = "Death";
                break;
            default:
                element = "DUNGOOFED";
                break;
        }
        switch (equip)
        {
            case 0:
                eqType = "Armor";
                break;
            case 1:
                eqType = "Boots";
                break;
            case 2:
                {
                    int wType = Random.Range(0, 9);
                    switch (wType)
                    {
                        case 0:
                            eqType = "Sword";
                            break;
                        case 1:
                            eqType = "Staff";
                            break;
                        case 2:
                            eqType = "Mace";
                            break;
                        case 3:
                            eqType = "Rod";
                            break;
                        case 4:
                            eqType = "Axe";
                            break;
                        case 5:
                            eqType = "Wand";
                            break;
                        case 6:
                            eqType = "Knife";
                            break;
                        case 7:
                            eqType = "Tome";
                            break;
                        case 8:
                            eqType = "Bow";
                            break;
                        case 9:
                            eqType = "Crossbow";
                            break;
                        default:
                            eqType = "weaponDUNGOOFED";
                            break;
                    }
                    break;
                }
            default:
                eqType = "DUNGOOFED";
                break;
        }
    }
    private void statGen(int _playerLevel, int _enemyDifficulty)
    {
        // Generate an item of random rarity, weighted by enemy difficulty. Higher difficulty = higher chance at rares.
        int rarity = Random.Range(0, 85) + _enemyDifficulty;

        // Stats gated by player level (max 10). Common and Rare items use half this number, so it must be even.
        int stat = Random.Range(0, _playerLevel + 2);
        if (stat > 10)
            stat = 10;
        if (stat % 2 != 0)
            stat -= 1;

        if (rarity <= 50)
        {
            output += "Common Item: ";
            numCommon++;
            // max range of stats, final bonus half potential
            rollStats(-stat, stat, stat / 2);
        }
        else if (rarity <= 75)
        {
            output += "Uncommon Item: ";
            numUncommon++;
            // flat ranges
            rollStats(-stat, stat, stat);
        }
        else if (rarity <= 94)
        {
            output += "Rare Item! : ";
            numRare++;
            // less chance of negatives, max final bonus potential
            rollStats(-(stat / 2), stat, stat);
        }
        else
        {
            output += "LEGENDARY! : ";
            numLegendary++;
            // No negatives, max final potential
            rollStats(0, stat, stat);
        }
    }
    private void roomGen(int _playerLevel, int _difficultyMod)
    {
        int floorDifficulty = _playerLevel * _difficultyMod * 4;
        int numRooms = Random.Range(4, _difficultyMod + _playerLevel);
        for (int i = 0; i < numRooms; i++)
        {
            Room room = new Room();
            if (floorDifficulty <= _playerLevel + _difficultyMod)
                room.roomDifficulty = floorDifficulty;
            else
            {
                room.roomDifficulty = Random.Range(_playerLevel + _difficultyMod, floorDifficulty / 2);
                floorDifficulty -= room.roomDifficulty;
            }
            roomList.Add(room);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            roomList[i].numEnemies = Random.Range(0, roomList[i].roomDifficulty / 2);
            if (roomList[i].numEnemies > 8)
                roomList[i].numEnemies = 8;
            roomList[i].enemies = new List<Enemy>();
            for (int j = 0; j < roomList[i].numEnemies; j++)
            {
                Enemy enemy = new Enemy();
                enemy.enemyDifficulty = Random.Range(1, roomList[i].roomDifficulty / 2);
                roomList[i].roomDifficulty -= enemyDifficulty;
                enemy.gearScore = Random.Range(1, enemy.enemyDifficulty);
                equipGen();
                statGen(enemy.gearScore, roomList[i].roomDifficulty);
                enemy.armor = element + " Armor -- ATK: " + atkMod + " DEF: " + defMod + " SPD: " + spdMod;

                enemy.gearScore = Random.Range(1, enemy.enemyDifficulty);
                equipGen();
                statGen(enemy.gearScore, roomList[i].roomDifficulty);
                enemy.boots = element + " Boots -- ATK: " + atkMod + " DEF: " + defMod + " SPD: " + spdMod;

                enemy.gearScore = Random.Range(1, enemy.enemyDifficulty);
                equipGen();
                statGen(enemy.gearScore, roomList[i].roomDifficulty);
                enemy.weapon = element + " Weapon -- ATK: " + atkMod + " DEF: " + defMod + " SPD: " + spdMod;

                roomList[i].enemies.Add(enemy);
            }

        }
    }

}


/* 
 
Difficulty Mods: 4 8 12 16

floorDifficulty = playerLevel * difficultyMod * 10
Ex: 1*4*10 = 40

numRooms = Random.Range(4, difficultyMod + playerLevel)
Ex: Range = 4-5

for each room 
	roomDifficulty = Random.Range(playerLevel + difficultyMod, floorDifficulty / 2)
	floorDifficulty -= roomDifficulty
	if(floorDifficulty <= playerLevel + difficultyMod)
		roomDifficulty = floorDifficulty

for each room
    numEnemies = Random.Range(2, roomDifficulty/2);
    for each enemy
        enemyDifficulty = Random.Range(1, roomDifficulty/2)
        roomDifficulty -= enemyDifficulty;
        for each equip
            gearScore = random.Range(1, enemyDifficulty)
            statGen(gearScore, roomDifficulty)



*/
