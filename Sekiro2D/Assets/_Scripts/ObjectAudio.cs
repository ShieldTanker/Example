using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AudioState
{
    FootStepSound   = 0,
    JumpSound       = 1,
    AttackSound     = 2,
    GuardSound      = 3,
    HurtSound       = 4,
    DieSound        = 5,
}

public enum AudioSourceType
{
    Movement,
    Battle,
}

public class ObjectAudio : MonoBehaviour
{
    [Tooltip("오디오 소스가 여러개 일 수 있음 \n 움직임 관련 = 0번 \n 전투 관련 = 1번")]
    public AudioSource[] audioSources;
    
    [Tooltip("FootStepSound = 0 \n JumpSound = 1 \n AttackSound = 2 \n GuardSound = 3, \n HurtSound = 4, \n DieSound = 5")]
    public AudioClip[] sounds;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        StartSetting();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    public void ChangeSound(AudioSource audioSource, AudioState type)
    {
        audioSource.clip = sounds[(int)type];
        audioSource.Play();
    }

    private void StartSetting()
    {
        VolumeSetting();
    }

    public void VolumeSetting()
    {
        if (audioSources.Length > 0)
        {
            foreach (AudioSource audioSource in audioSources)
                audioSource.volume = PlayerPrefs.GetFloat("Volume");
        }
    }
}