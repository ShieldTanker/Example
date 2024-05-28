using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    EnemyBattle eB;
    public StageManager sM;

    private void Start()
    {
        eB = GetComponent<EnemyBattle>();
    }

    private void Update()
    {
        if (eB.enemyBattleState == EnemyBattleState.Die)
        {
            sM.SM.BossClear();
        }
    }
}
