using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    // Stats
    private int maxHP;
    private int currentHP;

    private int baseAttack;
    private int baseDefense;
    private int baseSpeed;

    private int currentAttack;
    private int currentDefense;
    private int currentSpeed;

    // Flags
    private bool isDead = false;         // If the character is dead
    private bool canMove = false;        // If the character has their Move action left
    private bool canAct = false;         // If the character has their Act action left
    private bool isActive = false;       // If it is the character's turn
    private bool isSelected = false;     // If the character has been clicked on


    // Mechanics Values
    private Tile location;
    private Vector3 position;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }


    #region Getters/Setters


    public int MaxHP
    {
        get
        {
            return maxHP;
        }

        set
        {
            maxHP = value;
        }
    }

    public int CurrentHP
    {
        get
        {
            return currentHP;
        }

        set
        {
            currentHP = value;
        }
    }

    public int BaseAttack
    {
        get
        {
            return baseAttack;
        }

        set
        {
            baseAttack = value;
        }
    }

    public int BaseDefense
    {
        get
        {
            return baseDefense;
        }

        set
        {
            baseDefense = value;
        }
    }

    public int BaseSpeed
    {
        get
        {
            return baseSpeed;
        }

        set
        {
            baseSpeed = value;
        }
    }

    public int CurrentAttack
    {
        get
        {
            return currentAttack;
        }

        set
        {
            currentAttack = value;
        }
    }

    public int CurrentDefense
    {
        get
        {
            return currentDefense;
        }

        set
        {
            currentDefense = value;
        }
    }

    public int CurrentSpeed
    {
        get
        {
            return currentSpeed;
        }

        set
        {
            currentSpeed = value;
        }
    }

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return canMove;
        }

        set
        {
            canMove = value;
        }
    }

    public bool CanAct
    {
        get
        {
            return canAct;
        }

        set
        {
            canAct = value;
        }
    }

    public bool IsActive
    {
        get
        {
            return isActive;
        }

        set
        {
            isActive = value;
        }
    }

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }

        set
        {
            isSelected = value;
        }
    }

    public Tile Location
    {
        get
        {
            return location;
        }

        set
        {
            location = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    #endregion
}
