using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : ObjectAudio
{
    [Tooltip("플레이어의 패링 사운드 배열")]
    public AudioClip[] farrySound;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    public void FarrySound()
    {
        int randomIdx = Random.Range(0, farrySound.Length);
        audioSources[(int)AudioSourceType.Battle].clip = farrySound[randomIdx];
        audioSources[(int)AudioSourceType.Battle].Play();
    }
}
