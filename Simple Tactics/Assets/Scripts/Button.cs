using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

    public bool isHighlighted = false;
    public bool isClicked = false;
    public bool isSelected = false;
    public Sprite Text;
    public Sprite HiText;

    public delegate void ClickAction(GameObject _button);
    public static event ClickAction OnClicked;

    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    void Update() {
        if (isHighlighted || isSelected)
            gameObject.GetComponent<SpriteRenderer>().sprite = HiText;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = Text;

        if(isClicked)
        {
            isClicked = false;
            if(OnClicked != null)
            {
                OnClicked(gameObject);
            }
            Debug.Log("Click Event Triggered");
        }
    }
}
