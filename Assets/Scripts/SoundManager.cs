using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip ballCollision;
    public AudioClip buttonClick;
    [SerializeField] AudioSource audioSourceFX;
    
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
    }


    public void PlaySound(int index)
    {
        audioSourceFX.PlayOneShot(ballCollision);
    }

    public float Volume(float value)
    {
        float soundVolume = (value / 100);
        return soundVolume;
    }
}
