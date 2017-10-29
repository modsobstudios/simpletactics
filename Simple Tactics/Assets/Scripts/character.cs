﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    #region StatusVeriables
    public enum STATUS_EFFECT
    {
        SPEED, ARMOR, ATTACK, NONE
    }
    private bool hasTakenStatus = false;
    private ArrayList status;
    #endregion

    //TODO: Add Equipment stuff
    #region Equipment

    #endregion


    #region HealthVariables
    //Must set the MAX_HEALTH value in the inspector for this class to function properly
    [SerializeField]
    private int MAX_HEALTH;
    private int health;
    private bool isDead = false;

    private ArrayList Dots;
    private bool hasTakenDOT = false;
    #endregion

    #region TurnVariables
    public bool isPlayerControlled = false;

    private bool hasMoved = false;
    private bool hasTakenAction = false;
    #endregion

    #region StatsVariables
    //TODO: Initialize Variables to proper values based on equipment calculations
    //Base variables are intended to act as the value that should exist counting equipment, but not counting any buff/debuff
    private float baseArmor;
    private float baseSpeed;
    private float baseAttack;
    //Non-base varaibles are used for calculations and modifed by buff/debuff
    private float armor;
    private float speed;
    private float attack;
    #endregion


    // Use this for initialization
    void Start()
    {
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update()
    {
        //Start of new turn

        //reset stats to base value before calculating new buff/debuff values
        resetStats();

        #region ApplyStatus
        //If the Character has Status/s, apply them
        if (!hasTakenStatus && status.Count > 0)
        {
            foreach (StatusEffect Effect in status)
            {
                //Applies the turn's DOT damage and reduces dot durations
                if (Effect.getRemaining() > 0)
                {
                    Effect.applyBuffDebuff(ref speed, ref armor, ref attack);

                    Effect.setRemaining(Effect.getRemaining() - 1);
                    //If the DOT has a compounding status, it applies here automatically. If not, this does nothing.
                    Effect.applyCompounding();
                }
                else
                {
                    Effect.clearStatus();
                    status.Remove(Effect);
                }
            }
            hasTakenStatus = true;
        }
        #endregion

        #region ApplyDoTs
        //If the Character has DOTS, apply them
        if (!hasTakenDOT && Dots.Count > 0 && hasTakenStatus)
        {
            foreach (DOT dot in Dots)
            {
                if (dot.getRemaining() > 0)
                {
                    dot.setRemaining(dot.getRemaining() - 1);
                    takeDamage(dot.getDamage());
                }
                else
                {
                    //Remove the DOT from the list, and remove any status effects it causes.
                    Dots.Remove(dot);
                    status.Remove(dot.getStatus());
                }
            }
            //Character has taken it's DOT for the turn
            hasTakenDOT = true;
        }
        #endregion

        #region ApplyMove_Actions
        //Choose character movement
        if (!hasMoved && hasTakenDOT)
        {
            //TODO: Make a move (add character input options etc)

            hasMoved = true;
        }

        if (hasMoved && !hasTakenAction)
        {
            //TODO: Add attack / consumable things
        }
        #endregion
    }

    #region HealthFunctions
    //------------------------------------------getHealth()
    public int getHealth()
    {
        return health;
    }
    //------------------------------------------setHealth(int _newHealth)
    public void setHealth(int _newHealth)
    {
        health = _newHealth;
    }
    //------------------------------------------getIsDead()
    public bool getIsDead()
    {
        return isDead;
    }
    //------------------------------------------takeDamage(int _damage)
    public void takeDamage(int _damage)
    {
        health -= _damage;

        //TODO: Apply compounding status effect

        if (health <= 0)
        {
            isDead = true;
        }
    }
    //------------------------------------------gainLife(int _lifeGain)
    public void gainLife(int _lifeGain)
    {
        health += _lifeGain;
    }
    //------------------------------------------applyDOT()
    public void applyDOT(int _damage, int _duration, float _statMod, STATUS_EFFECT _status, bool _isCompounding, float _multiplier)
    {
        DOT d = new DOT(_damage, _duration, _duration, _statMod, _status, _isCompounding, _multiplier);
        Dots.Add(d);
    }
    //------------------------------------------
    #endregion

    #region StatFunctions
    //------------------------------------------getArmor()
    public float getArmor()
    {
        return armor;
    }
    //------------------------------------------resetArmor()
    public void resetArmor()
    {
        armor = baseArmor;
    }
    //------------------------------------------getSpeed()
    public float getSpeed()
    {
        return speed;
    }
    //------------------------------------------resetSpeed()
    public void resetSpeed()
    {
        speed = baseSpeed;
    }
    //------------------------------------------getAttack()
    public float getAttack()
    {
        return attack;
    }
    //------------------------------------------resetAttacK()
    public void resetAttack()
    {
        attack = baseAttack;
    }
    //------------------------------------------resetStats()
    public void resetStats()
    {
        resetArmor();
        resetAttack();
        resetSpeed();
    }
    //------------------------------------------applyStatus
    public void applyStatus(int _duration, float _statMod, STATUS_EFFECT _type, bool _isCompounding, float _multiplier)
    {
        StatusEffect se = new StatusEffect(_duration, _statMod, _type, _isCompounding, _multiplier);
        status.Add(se);
    }
    //------------------------------------------
    #endregion

    //Call this passing the turn to this Character
    public void ResetTurn()
    {
        hasTakenStatus = false;
        hasTakenDOT = false;
        hasMoved = false;
        hasTakenAction = false;
    }
}

public struct DOT
{
    private StatusEffect status;

    private int duration;
    private int remaining;
    private int damage;

    public DOT(int _damage, int _duration, int _remaining, float _statMod, character.STATUS_EFFECT _status, bool _isCompounding, float _multiplier)
    {
        duration = _duration;
        remaining = _remaining;
        damage = _damage;
        status = new StatusEffect(_duration, _statMod, _status, _isCompounding, _multiplier);
    }

    public int getRemaining()
    {
        return remaining;
    }

    public void setRemaining(int _duration)
    {
        remaining = _duration;
    }

    public int getDamage()
    {
        return damage;
    }

    public StatusEffect getStatus()
    {
        return status;
    }
}

//Be careful not to accidentally set _multiplier to 0. When passing in 'no multiplier' please don't forget to pass 1.
public struct StatusEffect
{
    private int duration;
    private int remaining;
    private float armorMod;
    private float speedMod;
    private float attackMod;
    private bool isCompounding;
    private float multiplier;

    character.STATUS_EFFECT status;

    public StatusEffect(int _duration, float _statMod, character.STATUS_EFFECT _type, bool _isCompounding, float _multiplier)
    {
        duration = _duration;
        switch (_type)
        {
            case character.STATUS_EFFECT.ARMOR:
                armorMod = _statMod;
                speedMod = 0;
                attackMod = 0;
                break;
            case character.STATUS_EFFECT.SPEED:
                armorMod = 0;
                speedMod = _statMod;
                attackMod = 0;
                break;
            case character.STATUS_EFFECT.ATTACK:
                armorMod = 0;
                speedMod = 0;
                attackMod = _statMod;
                break;
            default:
                armorMod = 0;
                speedMod = 0;
                attackMod = 0;
                break;
        }
        multiplier = _multiplier;
        status = _type;
        isCompounding = _isCompounding;
        remaining = _duration;
    }

    public int getRemaining()
    {
        return remaining;
    }

    public void applyCompounding()
    {
        switch (status)
        {
            case character.STATUS_EFFECT.SPEED:
                speedMod *= multiplier;
                break;
            case character.STATUS_EFFECT.ARMOR:
                armorMod *= multiplier;
                break;
            case character.STATUS_EFFECT.ATTACK:
                attackMod *= multiplier;
                break;
            default:
                break;
        }
    }

    public bool getIsCompounding()
    {
        return isCompounding;
    }

    public void applyBuffDebuff(ref float _speed, ref float _armor, ref float _attack)
    {
        _speed += speedMod;
        _armor += armorMod;
        _attack += attackMod;
    }

    public void setRemaining(int _duration)
    {
        remaining = _duration;
    }

    public void clearStatus()
    {
        armorMod = 0;
        attackMod = 0;
        speedMod = 0;
    }
}