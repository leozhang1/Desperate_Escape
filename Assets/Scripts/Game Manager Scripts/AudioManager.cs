using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static float musicVolume = 1f, sfxVolume = 1f;

    [SerializeField] private AudioSource backgroundMusic, reloadWeapon, emptyMagazine, gunfire, gunfireSilenced;
    [SerializeField] private Slider musicSlider, sfxSlider;

    private void Start()
    {
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        UpdateMusicVolume(musicSlider.value);
        UpdateSFXVolume(sfxSlider.value);
    }

    public void PlaySound(SoundEffect effect)
    {
        switch (effect)
        {
            case SoundEffect.FireEmpty:
                emptyMagazine.Play();
                break;
            case SoundEffect.Reload:
                reloadWeapon.Play();
                break;
            case SoundEffect.Gunfire:
                gunfire.Play();
                break;
            case SoundEffect.GunfireSilenced:
                gunfireSilenced.Play();
                break;
        }
    }

    public void UpdateMusicVolume()
    {
        musicVolume = musicSlider.value;
        UpdateMusicVolume(musicVolume);
    }

    private void UpdateMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }

    public void UpdateSFXVolume()
    {
        sfxVolume = sfxSlider.value;
        UpdateSFXVolume(sfxVolume);
    }

    private void UpdateSFXVolume(float volume)
    {
        reloadWeapon.volume = volume;
        emptyMagazine.volume = volume;
        gunfire.volume = volume;
        gunfireSilenced.volume = volume;
    }

    public enum SoundEffect
    {
        FireEmpty,
        Reload,
        Gunfire,
        GunfireSilenced
    }
}
