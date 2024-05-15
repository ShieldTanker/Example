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

    // 애니메이션 관련
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

    //체력 관련
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

        // 콜라이더가 플레이어 태그를 가지고 있을때
        if (playerColl != null)
        {
            PlayerBattle pB = playerColl.GetComponent<PlayerBattle>();
            PlayerState pS = PlayerManager.PManager.PlState;

            if (pS == PlayerState.Farrying)
            {   // 플레이어가 패링상태일때
                pB.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;
            }
            else if (pS == PlayerState.Guard)
            {   // 플레이어가 가드상태일때
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce / 2, playerKnocBackTime));
                pB.Guarded();
            }
            else
            {   // 가드,패링 상태가 아닐때
                StartCoroutine(pB.KnockBack(enemy.transform, knockBackForce, playerKnocBackTime));
                pB.TakeDamage(atkDamage);
            }
        }
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

    // 적 피격
    public void enemyHurt(float damage)
    {
        enemyHp -= damage;
        hpBar.value = enemyHp / enemyMaxHp;

        if (enemyHp > 0)
        {   // 피격 소리 넣기
            enemyBattleState = EnemyBattleState.Hurt;
        }
        else
        {   // 죽는 소리 넣기
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
        // 테스트용 공격 명령
        StartCoroutine(AtkTest());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
