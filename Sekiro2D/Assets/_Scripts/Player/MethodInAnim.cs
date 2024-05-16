using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodInAnim : MonoBehaviour
{
    public PlayerBattle pB;
    public AudioSource aS;
    public PlayerAudio pA;

    public void AttackEnemy()
    {
        pB.AttackEnemy();
    }
    void FootStep()
    {
        aS.clip = pA.footStep;
        aS.Play();
    }
}
