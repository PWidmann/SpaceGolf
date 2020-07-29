using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Range(0, 1)]
    public float volume = 0.6f;
    public AudioClip[] audioClips;
    [SerializeField] AudioSource audioSourceFX;
    private float soundPlayTimer = 0.15f;
    private bool canPlaySound = true;
    
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioSourceFX.GetComponent<AudioSource>();
    }

    private void Update()
    {
        audioSourceFX.volume = 0.8f;

        if (soundPlayTimer >= 0)
            soundPlayTimer -= Time.deltaTime;
        else
        {
            soundPlayTimer = 0.15f;
            canPlaySound = true;
        }
    }


    public void PlaySound(int index)
    {
        if (canPlaySound)
        {
            audioSourceFX.PlayOneShot(audioClips[index]);
            canPlaySound = false;
        }
        
    }

    public float Volume(float value)
    {
        float soundVolume = (value / 100);
        return soundVolume;
    }
}
