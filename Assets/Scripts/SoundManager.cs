using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    // Start is called before the first frame update
    public enum Sound
    {
        Click,
        Save,
        Menu,
        Exploration,
    }

    private static GameObject soundManagerGameObject;
    private static AudioSource audioSource;
    public static float backgroundVol = 1f;
    public static float effectsVol = 1f;

    public static void CreateSoundManagerGameobject()
    {
        //Debug.Log
        if (soundManagerGameObject == null)
        {
            soundManagerGameObject = new GameObject("Sound Manager");
            audioSource = soundManagerGameObject.AddComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("Sound Manager already exist");
        }
    }
    public static void changeBackgroundVol(float value)
    {
        backgroundVol = value;
    }

    public static void changeEffectVolumen(float value)
    {
        effectsVol = value;
    }

    public static void PlaySound(Sound sound)
    {
        audioSource.PlayOneShot(GetAudioClipFromSound(sound), effectsVol);
    }

    public static void PlaySong(Sound sound)
    {
        audioSource.clip = GetAudioClipFromSound(sound);
        audioSource.Play();
        audioSource.loop = true;
    }

    private static AudioClip GetAudioClipFromSound(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipsArray)
        {

            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError($"Sound {sound} NOT Found");
        return null;
    }
}
