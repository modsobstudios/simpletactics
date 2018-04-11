using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    [SerializeField]
    private bool hasTooltip = false;
    private bool is2D = false;
    public bool Selectable = true;

    private bool isHighlighted = false;
    [System.NonSerialized]
    private bool isClicked = false;
    private bool isSelected = false;
    private bool isHeld = false;

    public Sprite NormalSprite;
    public Sprite HilightedSprite;
    public Sprite HeldSprite;

    public Material NormalMaterial;
    public Material HilightedMaterial;
    public Material HeldMaterial;

    public GameObject tooltip;

    public delegate void ClickAction(GameObject _button);
    public static event ClickAction OnClicked;

    // Use this for initialization
    void Start()
    {
        tooltip.SetActive(false);

        if (gameObject.GetComponent<MeshRenderer>() != null)
        {
            is2D = false;
        }
        else if(gameObject.GetComponent<SpriteRenderer>() != null)
        {
            is2D = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((isHighlighted || isSelected) && !isHeld)
        {
            if (is2D)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = HilightedSprite;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = HilightedMaterial;
            }
            if (hasTooltip)
            {
                tooltip.SetActive(true);
            }

        }
        else if(isHeld)
        {
            if (is2D)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = HeldSprite;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = HeldMaterial;
            }
            if (hasTooltip)
            {
                tooltip.SetActive(true);
            }
        }
        else
        {
            if (is2D)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = NormalSprite;
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material = NormalMaterial;
            }
            if (hasTooltip)
                tooltip.SetActive(false);
        }

        if (isClicked)
        {
            isClicked = false;
            if (OnClicked != null)
            {
                OnClicked(gameObject);
            }
            Debug.Log("Click Event Triggered");
        }
    }

    public bool getIsHighlighted()
    {
        return isHighlighted;
    }
    public void setIsHighlighted(bool _isHighlighted)
    {
        isHighlighted = _isHighlighted;
    }

    public bool getIsSelected()
    {
        return isSelected;
    }
    public void setIsSelected(bool _isSelected)
    {
        isSelected = _isSelected;
    }

    public bool getIsHeld()
    {
        return isHeld;
    }
    public void setIsHeld(bool _isHeld)
    {
        isHeld = _isHeld;
    }

    public bool getIsClicked()
    {
        return isClicked;
    }
    public void setIsClicked(bool _isClicked)
    {
        isClicked = _isClicked;
    }
}
