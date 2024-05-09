using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBattleState
{
    Idle,
    Attack,
    Guard,
    Farrying,
    Die
}

public class PlayerBattle : MonoBehaviour
{
    // 애니메이션
    public Animator playerAnim;

    //플레이어 상태
    public static PlayerBattleState playerBattleState;
    bool isGround;

    public Collider2D[] enemyCollider;
    public Collider2D[] enemyObj;

    // 체력
    public float playerHp;

    // 공격 관련
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private int attackCombo = 0;
    private bool isAttack;

    // 공격 범위 관련
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;

    // 방어,패링
    private bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    private void Update()
    {
        isGround = gameObject.GetComponent<PlayerMovement>().grounded;
        KeyInput();
        Attack();
    }

    public void KeyInput()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            isAttack = true;
        }
        else if (!isAttack)
        {
            //방어
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                inputGuard = true;
                StartCoroutine(GuardOrFarry());
            }
            else if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                SetStateIdle();
                playerAnim.SetBool("idleGuard", false);
            }
        }
    }

    public void Attack()
    {
        if (isAttack)
        {
            // 공격가능 시간이 공격 딜레이 시간보다 많을때
            if (attackTimeCount > delayAttackTime)
            {
                //공격모션
                AttackCombo(2);

                // 플레이어 상태
                playerBattleState = PlayerBattleState.Attack;

                // 플레이어 애니메이션 재생
                playerAnim.SetTrigger("isAttack" + attackCombo);

                attackTimeCount = 0;
            }
        }
        //공격 가능 시간이 리셋 시간 보다 작을때
        ResetAttackComboTimeCount(resetComboTime);
    }


    // Attack1, Attack2 애니메이션 에서 Add Event 로 호출
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
            // 여기안에 적 피격 넣을것
            Debug.Log("EnemyHit");
            Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }

    // 애니메이션 Add Event 에 넣어짐
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        inputGuard = false;
    }

    private void AttackCombo(int maxCombo)
    {
        //공격모션
        attackCombo++;
        if (attackCombo > maxCombo)
        {
            attackCombo = 1;
            attackTimeCount = 0;
        }
    }

    private void ResetAttackComboTimeCount(float resetTime)
    {
        if (attackTimeCount < resetComboTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;
            isAttack = false;
        }
    }
    IEnumerator GuardOrFarry()
    {
        SetAnimationGuard();
        playerBattleState = PlayerBattleState.Farrying;
        
        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            playerBattleState = PlayerBattleState.Guard;
        }
    }

    // 플레이어 데미지 입음
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        if (playerHp > 0)
        {
            playerHp -= damage;
            // 나중에 피격 애니메이션 넣기
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // 나중에 사망 애니메이션 넣기
        }
    }

    // 가드 애니메이션 활성화
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
