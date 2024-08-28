using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    public void setMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", level);
    }

    public void setSoundFXVolume(float level)
    {
        audioMixer.SetFloat("SFX", level);
    }

    public void setMusicVolume(float level)
    {
        audioMixer.SetFloat("Music", level);
    }
}
