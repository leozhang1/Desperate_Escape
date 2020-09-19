using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;

    private void Start()
    {
        musicSlider.value = AudioManager.musicVolume;
        sfxSlider.value = AudioManager.sfxVolume;
    }

    public void SetMusicVolume()
    {
        AudioManager.musicVolume = musicSlider.value;
    }

    public void SetSFXVolume()
    {
        AudioManager.sfxVolume = sfxSlider.value;
    }
}
