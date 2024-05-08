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

        // �ݶ��̴��� �÷��̾� �±׸� ������ ������
        if (playerColl != null)
        {
            PlayerBattleState pbs = PlayerBattle.playerBattleState;

            if (pbs == PlayerBattleState.Farrying)
            {
                // �÷��̾ �и������϶�
                Farryed();
            }
            else if (pbs == PlayerBattleState.Guard)
            {
                // �÷��̾ ��������ϋ�
            }
            else
            {
                // ����,�и� ���°� �ƴҶ�
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

    // ���ݽ� ���� ��ü Ȯ��
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
