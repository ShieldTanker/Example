using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Collider2D[] atkColl;
    public Transform enemyAtkPoint;
    public Vector2 atkBoxSize;
    public float atkDamage;
    public Animator pAnim;
    public Animator enemyAnim;
    public float enemyAtkTime;

    private void Start()
    {
        StartCoroutine(AtkTest());
    }
    public void AttackPlayer()
    {
        Collider2D playerColl = AtkCollider(enemyAtkPoint, atkBoxSize);

        // 콜라이더가 플레이어 태그를 가지고 있을때
        if (playerColl != null)
        {
            PlayerBattleState pbs = PlayerBattle.playerBattleState;

            if (pbs == PlayerBattleState.Farrying)
            {
                // 플레이어가 패링상태일때
                Farryed();
            }
            else if (pbs == PlayerBattleState.Guard)
            {
                // 플레이어가 가드상태일떄
            }
            else
            {
                // 가드,패링 상태가 아닐때
                PlayerBattle pb = playerColl.GetComponent<PlayerBattle>();
                pb.TakeDamage(atkDamage);
            }
        }
    }

    private void Farryed()
    {
        pAnim.SetTrigger("isFarry" + Random.Range(1, 3));
        pAnim.SetBool("idleGuard", false);
        enemyAnim.SetTrigger("FarryedAttack");
    }

    // 공격시 닿은 물체 확인
    private Collider2D AtkCollider(Transform atkPoint, Vector2 boxSize)
    {
        Collider2D playerColl = new Collider2D();
        atkColl = Physics2D.OverlapBoxAll(atkPoint.position, boxSize, 0f);

        foreach (Collider2D col in atkColl)
        {
            if (col.gameObject.tag == "Player")
            {
                playerColl = col;
            }
        }

        return playerColl;
    }


    IEnumerator AtkTest()
    {
        while (true)
        {
            enemyAnim.SetTrigger("EnemyAttack");
            yield return new WaitForSeconds(enemyAtkTime);
        }
    }
}
