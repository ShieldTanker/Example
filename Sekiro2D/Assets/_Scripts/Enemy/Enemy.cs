using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
<<<<<<< HEAD:Sekiro2D/Assets/_Scripts/Enemy/Enemy.cs
    // 애니메이션 관련
    public Animator pAnim;
    public Animator enemyAnim;

    // 전투 범위 관련
    public Collider2D[] atkColl;
    public Transform enemyAtkPoint;
    public GameObject enemy;
    public LayerMask playerLayer;
    public Vector2 atkBoxSize;
    
    // 공격 관련
    public float atkDamage;
    public float enemyAtkTime;
    public float knockBackForce;
    public float playerKnocBackTime;

    private void Start()
=======
    PlayerBattleState pb;
    public GameObject player;
    private void Update()
>>>>>>> parent of 81b66e7 (�뙣留� �뙋�젙 蹂�寃� 諛� �쟻 �븷�땲硫붿씠�뀡 �닔�젙):Sekiro2D/Assets/_Scripts/Enemy.cs
    {
        pb = player.GetComponent<PlayerBattleState>();
    }

    private void Update()
    {
    }

    public void AttackPlayer()
    {
        if (pb != PlayerBattleState.Guard && pb != PlayerBattleState.Farrying)
        {
<<<<<<< HEAD:Sekiro2D/Assets/_Scripts/Enemy/Enemy.cs
            PlayerBattle pb = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pbs = PlayerBattle.playerBattleState;

            if (pbs == PlayerBattleState.Farrying)
            {   // 플레이어가 패링상태일때
                Farryed();
            }
            else if (pbs == PlayerBattleState.Guard)
            {   // 플레이어가 가드상태일때
            }
            else
            {
                // 가드,패링 상태가 아닐때
               StartCoroutine(pb.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pb.TakeDamage(atkDamage);
            }
        }
    }

    private void Farryed()
    {
        // 플레이어 패링 애니메이션 재생
        pAnim.SetTrigger("isFarry" + Random.Range(1, 3));
        pAnim.SetBool("idleGuard", false);

        // 패링 당한 애니메이션 실행
        enemyAnim.SetTrigger("FarryedAttack");
    }

    // 공격시 닿은 물체 확인
    private Collider2D AtkCollider(Transform atkPoint, Vector2 boxSize)
    {
        Collider2D playerColl = new Collider2D();
        atkColl = Physics2D.OverlapBoxAll(atkPoint.position, boxSize, 0f, playerLayer);

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
=======
            Debug.Log("EnemyAttack");
>>>>>>> parent of 81b66e7 (�뙣留� �뙋�젙 蹂�寃� 諛� �쟻 �븷�땲硫붿씠�뀡 �닔�젙):Sekiro2D/Assets/_Scripts/Enemy.cs
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
