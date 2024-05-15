using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyHit : MonoBehaviour
{
    public PlayerBattle pB;

    public void AttackEnemy()
    {
        pB.AttackEnemy();
    }
}
