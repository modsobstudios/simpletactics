using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscHandler : MonoBehaviour
{
    bool shown = false;
    bool showing = false;
    float showCt = 0.0f;
    Director d;
    // Use this for initialization
    void Start()
    {
        this.transform.localScale = new Vector3(0, 0, 0);
        d = GameObject.Find("ScriptTester").GetComponent<Director>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            d.paused = !d.paused;
            showing = !showing;
        }
        pauseUnpause();
    }

    public void restartGame()
    {
        GameObject.Find("ScriptTester").GetComponent<Director>().restartGame();
    }

    public void optionsMenu()
    {
        SceneManager.LoadScene("optionsMenu_04");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("mainMenu_01");
    }

    public void pauseUnpause()
    {
        if (showing)
        {
            if (shown)
            {
                Time.timeScale = 1;
                showCt -= 0.1f;
                this.transform.localScale = new Vector3(showCt, showCt, showCt);
                if (showCt <= 0.0f)
                {
                    this.transform.localScale = new Vector3(0, 0, 0);

                    shown = !shown;
                    showCt = 0.0f;
                    showing = false;
                }
            }
            else
            {
                showCt += 0.1f;
                Time.timeScale = 0;
                this.transform.localScale = new Vector3(showCt, showCt, showCt);
                if (showCt >= 1.0f)
                {
                    shown = !shown;
                    showCt = 1.0f;
                    showing = false;
                }
            }
        }
    }
}
