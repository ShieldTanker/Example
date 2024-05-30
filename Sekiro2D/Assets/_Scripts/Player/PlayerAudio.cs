using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public GameObject[] enemys;
    public AudioSource enemyAudio;

    public AudioSource battleAudioSource;
    public AudioSource moveAudioSource;
    
    public AudioClip footStep;
    public AudioClip jumpSound;
    
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
        battleAudioSource.clip = farrySound[randomIdx];

        battleAudioSource.Play();
    }

    public void FootStepSound()
    {
        moveAudioSource.clip = footStep;
        moveAudioSource.Play();
    }
    public void JumpSound()
    {
        moveAudioSource.clip = jumpSound;
        moveAudioSource.Play();
    }
    public void GuardSound()
    {
        battleAudioSource.clip = guardSound;
        battleAudioSource.Play();
    }
    public void HurtSound()
    {
        battleAudioSource.clip = hurtSound;
        battleAudioSource.Play();
    }

    private void StartSetting()
    {
        VolumeSetting();
    }

    public void VolumeSetting()
    {
        enemys = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (var enemy in enemys)
        {
            enemyAudio = enemy.GetComponent<AudioSource>();
            enemyAudio.volume = PlayerPrefs.GetFloat("Volume");
        }

        battleAudioSource.volume = PlayerPrefs.GetFloat("Volume");
        moveAudioSource.volume = PlayerPrefs.GetFloat("Volume");
    }
}
