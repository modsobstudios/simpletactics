using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Load_Level : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Button.OnClicked += LoadLevel;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadLevel(GameObject _button)
    {
        if (_button.tag == "Scene")
        {
            SceneManager.LoadScene(_button.name);
        }
    }
}
