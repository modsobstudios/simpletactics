using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour {
    private Transform target;
    private TextMesh textMesh;

    // Use this for initialization
    void Start () {
        target = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
        transform.rotation = target.rotation;
    }
}
