using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMethodInAnim : MonoBehaviour
{
    EnemyBattle eB;
    EnemyAudio enemyAudio;
    AudioSource audioSource;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null )
            audioSource = GetComponentInParent<AudioSource>();

        eB = GetComponent<EnemyBattle>();
        if (eB == null)
            eB = GetComponentInParent<EnemyBattle>();

        enemyAudio = GetComponent<EnemyAudio>();
        if (eB == null)
            enemyAudio = GetComponentInParent<EnemyAudio>();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    void AnimLookAHead()
    {
        eB.LookAHead(eB.player.transform);
    }

    void AnimFootStep()
    {
        enemyAudio.ChangeSound(audioSource, AudioState.FootStepSound);
    }
    void Attacksound()
    {
        enemyAudio.ChangeSound(audioSource, AudioState.AttackSound);
    }
}
