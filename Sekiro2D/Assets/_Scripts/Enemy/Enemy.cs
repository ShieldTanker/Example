using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyBattleState
{
    Idle,
    Attack,
    Guard,
    Farryed,
    Hurt,
    Die
}

public class Enemy : MonoBehaviour
{
    EnemyBattleState enemyBattleState;
    EnemyBattleState lastEBS;

    // �ִϸ��̼� ����
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
    public float enemyMaxHp;
    public Slider hpBar;
    public GameObject hpCanvas;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        EnemyBattleAnimUpdate(enemyBattleState);
    }

    public void AttackPlayer()
    {
        Collider2D playerColl = AtkCollider(enemyAtkPoint, atkBoxSize);

        // �ݶ��̴��� �÷��̾� �±׸� ������ ������
        if (playerColl != null)
        {
            PlayerBattle pb = playerColl.GetComponent<PlayerBattle>();
            PlayerState pbs = GameManager.GManager.PlState;

            if (pbs == PlayerState.Farrying)
            {   // �÷��̾ �и������϶�
                pb.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;
            }
            else if (pbs == PlayerState.Guard)
            {   // �÷��̾ ��������϶�
                StartCoroutine(pb.KnockBack(enemy.transform, knockBackForce / 2, playerKnocBackTime));
                pb.Guarded();
            }
            else
            {   // ����,�и� ���°� �ƴҶ�
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
        hpBar.value = enemyHp / enemyMaxHp;

        if (enemyHp > 0)
        {   // �ǰ� �Ҹ� �ֱ�
            enemyBattleState = EnemyBattleState.Hurt;
        }
        else
        {   // �״� �Ҹ� �ֱ�
            enemyBattleState = EnemyBattleState.Die;
        }
    }

    IEnumerator AtkTest()
    {
        while (enemyBattleState != EnemyBattleState.Die)
        {
            enemyBattleState = EnemyBattleState.Attack;

            yield return new WaitForSeconds(enemyAtkTime);
            
            if (enemyBattleState != EnemyBattleState.Die)
            {
                enemyBattleState = EnemyBattleState.Idle;
            }
        }
    }

    void EnemyBattleAnimUpdate(EnemyBattleState eBS)
    {
        if (lastEBS == eBS)
            return;

        switch (eBS)
        {
            case EnemyBattleState.Idle:
                break;

            case EnemyBattleState.Attack:
                enemyAnim.SetTrigger("EnemyAttack");
                break;

            case EnemyBattleState.Guard:
                break;

            case EnemyBattleState.Farryed:
                enemyAnim.SetTrigger("FarryedAttack");
                break;

            case EnemyBattleState.Hurt:
                enemyAnim.SetTrigger("isHurt");
                break;

            case EnemyBattleState.Die:
                enemyAnim.SetTrigger("isDie");
                break;

            default:
                break;
        }

        lastEBS = eBS;
    }

    void StartSetting()
    {
        // �׽�Ʈ�� ���� ���
        StartCoroutine(AtkTest());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
