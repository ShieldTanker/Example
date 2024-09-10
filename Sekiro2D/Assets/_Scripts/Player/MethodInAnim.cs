using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodInAnim : MonoBehaviour
{
    public PlayerBattle pB;
    public AudioSource audioSource;
    public PlayerAudio pA;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pA = GetComponentInParent<PlayerAudio>();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    public void AttackEnemy()
    {
        pB.AttackEnemy();
    }

    void FootStep()
    {
        pA.ChangeSound(audioSource, AudioState.FootStepSound);
    }

    void AttackSound()
    {
        pA.ChangeSound(audioSource, AudioState.AttackSound);
    }
}
