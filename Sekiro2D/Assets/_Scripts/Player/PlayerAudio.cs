using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    
    public AudioClip footStep;
    
    public AudioClip[] farrySound;
    public AudioClip guardSound;
    
    public AudioClip hurtSound;


    private void Start()
    {
        StartSetting();
    }


    public void FarrySound()
    {
        // 오디오 재생
        int randomIdx = Random.Range(0, 2);
        audioSource.clip = farrySound[randomIdx];

        audioSource.Play();
    }

    public void FootStepSound()
    {
        audioSource.clip = footStep;
        audioSource.Play();
    }
    public void GuardSound()
    {
        audioSource.clip = guardSound;
        audioSource.Play();
    }
    public void HurtSound()
    {
        audioSource.clip = hurtSound;
        audioSource.Play();
    }

    private void StartSetting()
    {
    }
}
