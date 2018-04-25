using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Key terms:
 *      SFX - Sound Effects: Weapon clangs, grunts, spell sounds, ambient noises
 *      Vox - Vocal Tracks: Spoken dialogue
 *      BGM - Background Music: Any music that is not meant to be audible to the game world.
 */

public class AudioManager : MonoBehaviour
{
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

    // Update is called once per frame
    void Update()
    {

    }

    // Loads a sound effect into the sfxSource to be played
    public AudioClip loadSFX(string sfxName)
    {
        return Resources.Load<AudioClip>("sfxFiles/" + sfxName);
    }

    // Loads a vocal track into the voxSource to be played
    public AudioClip loadVox(string voxName)
    {
        return Resources.Load<AudioClip>("voxFiles/" + voxName);
    }

    // Loads a song into the bgmSource to be played.
    public AudioClip loadBGM(string bgmName)
    {
        return Resources.Load<AudioClip>("bgmFiles/" + bgmName);
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
}
