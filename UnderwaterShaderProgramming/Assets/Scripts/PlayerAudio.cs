using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip swimSound;
    [SerializeField] float swimSoundLength;

    float swimSoundTimer = 0;
    
    public void PlaySwimAudio()
    {
        if (swimSoundTimer <= 0)
        {
            source.PlayOneShot(swimSound);
            swimSoundTimer = swimSoundLength;
        }
    }

    private void Update()
    {
        if(swimSoundTimer > 0)
            swimSoundTimer -= Time.deltaTime;
    }
}
