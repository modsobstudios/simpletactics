using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : MonoBehaviour
{

    Grid g;
    Character selectedCharacter;
    Tile selectedTile;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            selectObject();
        }
    }

    private void selectObject()
    {
        Debug.Log("Casting Ray...");
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        // hit.transform.position += new Vector3(0, 1, 0);
        if(hit.collider == null)
        {
            if (selectedCharacter != null)
                selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.white;
            if (selectedTile != null)
                selectedTile.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();
            selectedTile = null;
            selectedCharacter = null;
        }
        else if (hit.collider.tag == "Character")
        {
            Debug.DrawLine(ray.origin, hit.point, Color.magenta, 5, true);
            if(selectedCharacter != null)
                selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.white;
            selectedCharacter = hit.transform.gameObject.GetComponent<Character>();
            selectedCharacter.GetComponentInChildren<SkinnedMeshRenderer>().materials[0].color = Color.magenta;
        }
        else if (hit.collider.tag == "Tile")
        {
            Debug.DrawLine(ray.origin, hit.point, Color.magenta, 5, true);
            if (selectedTile != null)
                selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = selectedTile.getColor();
            selectedTile = hit.transform.gameObject.GetComponent<Tile>();
            selectedTile.transform.GetComponent<MeshRenderer>().materials[0].color = Color.blue;
        }

    }
}
