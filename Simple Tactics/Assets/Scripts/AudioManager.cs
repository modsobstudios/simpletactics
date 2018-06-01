using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Key terms:
 *      SFX - Sound Effects: Weapon clangs, grunts, spell sounds, ambient noises
 *      Vox - Vocal Tracks: Spoken dialogue
 *      BGM - Background Music: Any music that is not meant to be audible to the game world.
 *      
 * Intended use:
 *      Place the AudioManager prefab as a child of the Main Camera
 *      If a function would cause audio to play, have the script save off GameObject.Find("AudioManager").GetComponent<AudioManager>()
 *      in its Start() function.
 */
public class AudioManager : MonoBehaviour
{
    public enum AudioType { BGM = 1, SFX, VOX }
    [SerializeField]
    AudioSource sfxSource;
    [SerializeField]
    AudioSource voxSource;
    [SerializeField]
    AudioSource bgmSource;

    AudioClip currentSFX;
    AudioClip currentVox;
    AudioClip currentBGM;

    float sfxVolume = 1.0f;
    float voxVolume = 1.0f;
    float bgmVolume = 1.0f;
    float masterVolume = 1.0f;


    // Use this for initialization
    void Start()
    {
        bgmSource.loop = true;
    }

    public bool isSFXPlaying()
    {
        return sfxSource.isPlaying;
    }

    public bool isBGMPlaying()
    {
        return bgmSource.isPlaying;
    }

    public bool isVoxPlaying()
    {
        return voxSource.isPlaying;
    }

    public void PlayAudio(string audioName, AudioType audioType)
    {
        switch (audioType)
        {
            case AudioType.BGM:
                {
                    currentBGM = loadBGM(audioName);
                    playBGM();
                    break;
                }
            case AudioType.SFX:
                {
                    currentSFX = loadSFX(audioName);
                    playSFX();
                    break;
                }
            case AudioType.VOX:
                {
                    currentVox = loadVox(audioName);
                    playVox();
                    break;
                }
            default:
                {
                    Debug.Log("HOW???");
                    break;
                }
        }
    }

    // Loads a sound effect into the sfxSource to be played
    public AudioClip loadSFX(string sfxName)
    {
        return Resources.Load<AudioClip>("Audio/SFX/" + sfxName);
    }

    // Loads a vocal track into the voxSource to be played
    public AudioClip loadVox(string voxName)
    {
        return Resources.Load<AudioClip>("Audio/VOX/" + voxName);
    }

    // Loads a song into the bgmSource to be played.
    public AudioClip loadBGM(string bgmName)
    {
        return Resources.Load<AudioClip>("Audio/BGM/" + bgmName);
    }

    // Plays the currently loaded sound effect
    public void playSFX()
    {
        sfxSource.PlayOneShot(currentSFX);
    }

    // Plays the currently loaded vocal track
    public void playVox()
    {
        voxSource.PlayOneShot(currentVox);
    }

    // Plays the currently loaded song
    public void playBGM()
    {
        bgmSource.PlayOneShot(currentBGM);
    }

    // Sets the SFX volume ratio and recalculates all volume settings
    public float setSFXVolume(float volume)
    {
        sfxVolume = volume;
        calculateVolumes();
        return sfxSource.volume;
    }

    // Sets the vox volume ratio and recalculates all volume settings
    public float setVoxVolume(float volume)
    {
        voxVolume = volume;
        calculateVolumes();
        return voxSource.volume;
    }

    // Sets the music volume and recalculates all volume settings
    public float setBGMVolume(float volume)
    {
        bgmVolume = volume;
        calculateVolumes();
        return bgmSource.volume;
    }

    // Sets the master volume and recalculates all volume settings
    public float setMasterVolume(float volume)
    {
        masterVolume = volume;
        calculateVolumes();
        return masterVolume;
    }

    // 'Precise' Linear Interpolation 
    public float volumeLerp(float start, float end, float ratio)
    {
        return (1 - ratio) * start + ratio * end;
    }

    // 'Full Sail' Linear Interpolation
    public float volumeLerpFS(float start, float end, float ratio)
    {
        return (end - start) * ratio + start;
    }

    // Calculates all individual volumes based on the master volume setting.
    public void calculateVolumes()
    {
        sfxSource.volume = volumeLerpFS(0.0f, masterVolume, sfxVolume);
        voxSource.volume = volumeLerpFS(0.0f, masterVolume, voxVolume);
        bgmSource.volume = volumeLerpFS(0.0f, masterVolume, bgmVolume);
    }

    // Test functions for functionality:


    public void playExampleBGM()
    {
        Debug.Log("Playing music!");
        currentBGM = loadBGM("Nordic Landscape/Nordic Title");
        if (currentBGM == null) Debug.Log("BGM is null!");
        playBGM();
    }

    public void playExampleSFX()
    {
        Debug.Log("Playing sound!");
        currentSFX = loadSFX("clang");
        if (currentSFX == null) Debug.Log("SFX is null!");
        playSFX();
    }

    public void playExampleVox()
    {
        Debug.Log("Playing voice!");
        currentVox = loadVox("demo");
        if (currentVox == null) Debug.Log("Vox is null!");
        playVox();
    }

    public float getSFXVolume()
    {
        return sfxVolume;
    }
    public float getVoxVolume()
    {
        return voxVolume;
    }
    public float getBGMVolume()
    {
        return bgmVolume;
    }
}
