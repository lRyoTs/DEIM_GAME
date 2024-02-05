using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Sound
    {
        Click,
        ShootingSound,
        ImpactSound,
    }

    private static GameObject soundManagerGameObject;
    private static AudioSource audioSource;

    public static void CreateSoundManagerGameobject() {
        if (soundManagerGameObject == null)
        {
            soundManagerGameObject = new GameObject("Sound Manager");
            audioSource = soundManagerGameObject.AddComponent<AudioSource>();
        }
        else {
            Debug.LogError("Sound Manager already exist");
        }
    }

    public static void PlaySound(Sound sound) {
        audioSource.PlayOneShot(GetAudioClipFromSound(sound));
    }

    private static AudioClip GetAudioClipFromSound(Sound sound) {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.Instance.soundAudioClipsArray) {

            if (soundAudioClip.sound == sound) {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError($"Sound {sound} NOT Found");
        return null;
    }
}
