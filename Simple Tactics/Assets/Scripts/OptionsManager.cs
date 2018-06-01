using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Slider master, bgm, sfx, vox;
    public Toggle fs;
    public AudioManager au;
    bool showing = false;
    bool shown = false;
    bool fullScreen = false;
    float showCt = 0.0f;
    // Use this for initialization
    void Start()
    {
        master.onValueChanged.AddListener(delegate { updateVolumes(); });
        bgm.onValueChanged.AddListener(delegate { updateBGMVolume(); });
        sfx.onValueChanged.AddListener(delegate { updateSFXVolume(); });
        vox.onValueChanged.AddListener(delegate { updateVoxVolume(); });

        fullScreen = Screen.fullScreen;
        fs.isOn = fullScreen;
    }

    // Update is called once per frame
    void Update()
    {
        showHide();
    }
    public void updateSFXVolume()
    {
        au.setSFXVolume(sfx.value);
        if(!au.isSFXPlaying())
            au.PlayAudio("clang", AudioManager.AudioType.SFX);
    }
    public void updateBGMVolume()
    {
        au.setBGMVolume(bgm.value);
        if (!au.isBGMPlaying())
            au.PlayAudio("Nordic Landscape/Nordic Title", AudioManager.AudioType.BGM);
    }
    public void updateVoxVolume()
    {
        au.setVoxVolume(vox.value);
        if (!au.isVoxPlaying())
            au.PlayAudio("awesome", AudioManager.AudioType.VOX);
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

    public void toggleFullscreen()
    {
        fullScreen = !fullScreen;
        Screen.SetResolution(Screen.width, Screen.height, fullScreen);
    }
}
