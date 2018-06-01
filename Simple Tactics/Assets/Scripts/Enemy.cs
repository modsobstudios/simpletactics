using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int maxHP = 100;
    protected int currentHP = 100;

    protected int baseAttack = 10;
    protected int baseDefense = 10;
    protected int baseSpeed = 4;

    protected int currentAttack = 10;
    protected int currentDefense = 10;
    protected int currentSpeed = 4;

    protected int moveRange;
    protected int atkRange;
    protected int minAtkRange;

    // Flags
    protected bool isDead = false;         // If the character is dead
    protected bool canMove = false;        // If the character has their Move action left
    protected bool canAct = false;         // If the character has their Act action left
    protected bool isActive = false;       // If it is the character's turn
    protected bool isSelected = false;     // If the character has been clicked on


    // Mechanics Values
    protected Tile location;
    protected Vector3 position;
    protected Vector3 positionOffset = new Vector3(0, 0, 0);      // To match the character's feet to the level of the ground.
    protected List<Tile> moveRangeTiles;
    protected List<Tile> atkRangeTiles;
    protected SkinnedMeshRenderer meshRend;
    protected Color defaultColor, currentColor;
    protected float ratio = 0.0f;
    protected bool lerpBool = false;
    protected SpriteRenderer hpBar;
    public Character target;
    // Use this for initialization
    void Start()
    {
        moveRange = 5;
        minAtkRange = 3;
        atkRange = 6;
        meshRend = GetComponentInChildren<SkinnedMeshRenderer>();
        defaultColor = currentColor = meshRend.materials[0].color;
        hpBar = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentHP <= 0)
            Destroy(this.gameObject);

        hpBar.transform.LookAt(Camera.main.transform.position, -Vector3.up);
        //lookAtMe();
        hpBar.transform.localScale = new Vector3(currentHP * 0.01f, 0.1f, 1);
        hpBar.color = Color.Lerp(Color.red, Color.yellow, ((float)currentHP / (float)maxHP));

    }

    void lookAtMe()
    {
        float x = Camera.main.transform.position.x - transform.position.x;
        float y = Camera.main.transform.position.y - transform.position.y;
        float th = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        hpBar.transform.rotation = Quaternion.Euler(new Vector3(th, 0, 0));
    }

    // Indicate interaction
    private void OnMouseOver()
    {
        meshRend.materials[0].color = Color.Lerp(currentColor, Color.black, ratio);
        if (ratio <= 0.01f && lerpBool)
            lerpBool = false;
        if (ratio >= 0.99f && !lerpBool)
            lerpBool = true;
        if (!lerpBool) ratio += 0.05f;
        else ratio -= 0.05f;
    }

    private void OnMouseExit()
    {
        meshRend.materials[0].color = currentColor;
    }

    // Manually change character position, independent of tiles
    public void setEnemyPosition(Vector3 _position)
    {
        position = _position;
        this.transform.position = _position + positionOffset;
    }

    // Set character position to the given tile center
    public void setEnemyTile(Tile _tile)
    {
        if (location != null)
            location.occupied = false;
        location = _tile;
        setEnemyPosition(_tile.getTileWorldPos());
        _tile.occupied = true;
    }

    public void setCurrentColor(Color _c)
    {
        currentColor = _c;
        meshRend.materials[0].color = currentColor;
    }

    public void setDefaultColor()
    {
        currentColor = defaultColor;
        meshRend.materials[0].color = defaultColor;
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

    public int MoveRange
    {
        get
        {
            return moveRange;
        }

        set
        {
            moveRange = value;
        }
    }

    public int AtkRange
    {
        get
        {
            return atkRange;
        }

        set
        {
            atkRange = value;
        }
    }

    public int MinAtkRange
    {
        get
        {
            return minAtkRange;
        }

        set
        {
            minAtkRange = value;
        }
    }

    public List<Tile> MoveRangeTiles
    {
        get
        {
            return moveRangeTiles;
        }

        set
        {
            moveRangeTiles = value;
        }
    }

    public List<Tile> AtkRangeTiles
    {
        get
        {
            return atkRangeTiles;
        }

        set
        {
            atkRangeTiles = value;
        }
    }

    public Character Target
    {
        get
        {
            return target;
        }

        set
        {
            target = value;
        }
    }

    #endregion
}
