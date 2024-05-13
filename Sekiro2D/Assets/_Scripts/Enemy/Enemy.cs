using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
<<<<<<< HEAD:Sekiro2D/Assets/_Scripts/Enemy/Enemy.cs
    // �ִϸ��̼� ����
    public Animator pAnim;
    public Animator enemyAnim;

    // ���� ���� ����
    public Collider2D[] atkColl;
    public Transform enemyAtkPoint;
    public GameObject enemy;
    public LayerMask playerLayer;
    public Vector2 atkBoxSize;
    
    // ���� ����
    public float atkDamage;
    public float enemyAtkTime;
    public float knockBackForce;
    public float playerKnocBackTime;

    private void Start()
=======
    PlayerBattleState pb;
    public GameObject player;
    private void Update()
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정):Sekiro2D/Assets/_Scripts/Enemy.cs
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
            {   // �÷��̾ �и������϶�
                Farryed();
            }
            else if (pbs == PlayerBattleState.Guard)
            {   // �÷��̾ ��������϶�
            }
            else
            {
                // ����,�и� ���°� �ƴҶ�
               StartCoroutine(pb.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pb.TakeDamage(atkDamage);
            }
        }
    }

    private void Farryed()
    {
        // �÷��̾� �и� �ִϸ��̼� ���
        pAnim.SetTrigger("isFarry" + Random.Range(1, 3));
        pAnim.SetBool("idleGuard", false);

        // �и� ���� �ִϸ��̼� ����
        enemyAnim.SetTrigger("FarryedAttack");
    }

    // ���ݽ� ���� ��ü Ȯ��
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
>>>>>>> parent of 81b66e7 (패링 판정 변경 및 적 애니메이션 수정):Sekiro2D/Assets/_Scripts/Enemy.cs
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
