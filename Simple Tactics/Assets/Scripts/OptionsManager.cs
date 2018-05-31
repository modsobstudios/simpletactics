using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider master, bgm, sfx, vox;
    public AudioManager au;
    bool showing = false;
    bool shown = false;
    float showCt = 0.0f;
    // Use this for initialization
    void Start()
    {
        master.onValueChanged.AddListener(delegate { updateVolumes(); });
        bgm.onValueChanged.AddListener(delegate { updateVolumes(); });
        sfx.onValueChanged.AddListener(delegate { updateVolumes(); });
        vox.onValueChanged.AddListener(delegate { updateVolumes(); });
    }

    // Update is called once per frame
    void Update()
    {
        showHide();
    }

    public void updateVolumes()
    {
        au.setMasterVolume(master.value);
        au.setSFXVolume(sfx.value);
        au.setVoxVolume(vox.value);
        au.setBGMVolume(bgm.value);
        au.calculateVolumes();
    }

    public void showHide()
    {
        if (showing)
        {

            if (shown)
            {
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

    public void toggle()
    {
        showing = !showing;
    }
}
