using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    PlayerBattleState pb;
    public GameObject player;
    private void Update()
    {
        pb = player.GetComponent<PlayerBattleState>();
    }
    public void AttackPlayer()
    {
        if (pb != PlayerBattleState.Guard && pb != PlayerBattleState.Farrying)
        {
            Debug.Log("EnemyAttack");
        }
    }
}
