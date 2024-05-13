using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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

    //ü�� ����
    public float enemyHp;

    private void Start()
    {
        StartCoroutine(AtkTest());
    }

    private void Update()
    {
    }

    public void AttackPlayer()
    {
        Collider2D playerColl = AtkCollider(enemyAtkPoint, atkBoxSize);

        // �ݶ��̴��� �÷��̾� �±׸� ������ ������
        if (playerColl != null)
        {
            PlayerBattle pb = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pbs = PlayerBattle.playerBattleState;

            if (pbs == PlayerBattleState.Farrying)
            {   // �÷��̾ �и������϶�
                pb.Farryed();
                // �и� ���� �ִϸ��̼� ����
                enemyAnim.SetTrigger("FarryedAttack");
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

    // �� �ǰ�
    public void enemyHurt(float damage)
    {
        enemyHp -= damage;
    }

    IEnumerator AtkTest()
    {
        while (true)
        {
            enemyAnim.SetTrigger("EnemyAttack");
            yield return new WaitForSeconds(enemyAtkTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
