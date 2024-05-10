using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerBattleState
{
    Idle,
    Attack,
    Guard,
    Farrying,
    Hit,
    Die
}

public class PlayerBattle : MonoBehaviour
{
    // 애니메이션
    public Animator playerAnim;

    //플레이어 상태
    public static PlayerBattleState playerBattleState;
    bool isGround;
    private Rigidbody2D rb;

    // 체력
    public float playerHp;
    public bool playerHit;

    // 공격 관련
    static public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private int attackCombo = 0;

    // 공격 범위 관련
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // 방어,패링
    private bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    private void Update()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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

    public IEnumerator KnockBack(Transform enemy, float knockBackForce, float knockBackTime)
    {
        playerBattleState = PlayerBattleState.Hit;

        float knockBackVecX = gameObject.transform.position.x - enemy.position.x;
        
        if (knockBackVecX > 0)
            knockBackVecX = 1;
        else if (knockBackVecX < 0)
            knockBackVecX = -1;

        Vector2 knockBackVec = new Vector2(knockBackVecX, transform.position.y);

        rb.AddForce(knockBackVec * knockBackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockBackTime);

        rb.velocity = Vector2.zero;

        SetStateIdle();
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

    // 가드 애니메이션 활성화
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
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

    // 애니메이션 Add Event 에 넣어짐
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        inputGuard = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
