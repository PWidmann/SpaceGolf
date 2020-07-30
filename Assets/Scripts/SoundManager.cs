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
        // Make sure the collision system can't spam the collision sound
        if (soundPlayTimer >= 0)
        {
            soundPlayTimer -= Time.deltaTime;
        }
        else
        {
            soundPlayTimer = 0.15f;
            canPlaySound = true;
        }

        audioSourceFX.volume = GameManager.SoundVolume / 100;
    }


    public void PlaySound(int index)
    {
        if (canPlaySound)
        {
            audioSourceFX.PlayOneShot(audioClips[index]);
            canPlaySound = false;
        }
    }
}
