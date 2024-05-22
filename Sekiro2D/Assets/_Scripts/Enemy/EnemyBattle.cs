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

public class EnemyBattle : MonoBehaviour
{
    public EnemyBattleState enemyBattleState;
    EnemyBattleState lastEBS;

    // �ִϸ��̼� ����
    public Animator enemyAnim;

    //����� ����
    private AudioSource audioSource;
    public AudioClip hurtSound;

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

    // ���� ���� ����
    public static bool isFarryed;
    public float FarryDelay;

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
            PlayerBattle pB = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pS = PlayerManager.PManager.PlBattleState;

            if (pS == PlayerBattleState.Farrying)
            {   // �÷��̾ �и������϶�
                pB.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;

                isFarryed = true;
                Invoke("FalseFarryed", FarryDelay);

            }
            else if (pS == PlayerBattleState.Guard)
            {   // �÷��̾ ��������϶�
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce / 2, playerKnocBackTime));
                pB.Guarded();
            }
            else
            {   // ����,�и� ���°� �ƴҶ�
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pB.TakeDamage(atkDamage);
            }
        }
    }
    void FalseFarryed()
    {
        isFarryed = false;
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

        audioSource.clip = hurtSound;
        audioSource.Play();

        if (enemyHp > 0)
        {   // �ǰ�
            enemyBattleState = EnemyBattleState.Hurt;
        }
        else
        {   // ���
            enemyBattleState = EnemyBattleState.Die;
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
                enemyAnim.SetBool("enemyDied",true);
                break;

            default:
                break;
        }

        lastEBS = eBS;
    }

    void StartSetting()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
