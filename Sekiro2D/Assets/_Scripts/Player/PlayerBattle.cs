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
    public Animator playerAnim;
    public LayerMask enemyLayer;

    //플레이어 상태
    public static PlayerBattleState playerBattleState;
    bool isGround;

    public float testForce;

    public Collider2D[] enemyCollider;
    public Collider2D[] enemyObj;


    // 공격
    public Transform battlePoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public static int attackCombo = 0;
    private bool isAttack;
    public Vector2 battleBoxSize;

    // 방어
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;
    public bool isGuard;

    public bool isFarrying;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // 체력
    public float playerHp;


    private void Update()
    {
        isGround = gameObject.GetComponent<PlayerMovement>().grounded;
        KeyInput();
        Attack();
        Guard();
    }

    public void KeyInput()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            isAttack = true;
        }
        
        //방어
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            inputGuard = true;
            checkGuard = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            inputGuard = false;

            playerAnim.SetBool("idleGuard", false);

            // Debug.Log("ResetFarryTime");
            farryTime = resetFarryTime;
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
                attackCombo++;
                if (attackCombo > 2)
                {
                    attackCombo = 1;
                    attackTimeCount = 0;
                }

                // 플레이어 상태
                playerBattleState = PlayerBattleState.Attack;

                // 플레이어 애니메이션 재생
                playerAnim.SetTrigger("isAttack" + attackCombo);

                attackTimeCount = 0;
            }
        }

        //공격 가능 시간이 리셋 시간 보다 작을때
        if (attackTimeCount < resetComboTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;

            isAttack = false;
        }
    }

    public void Guard()
    {
        if (farryTime > 0 && inputGuard)
            farryTime -= Time.deltaTime;

        if (inputGuard)
        {
            // 처음 가드 올리기
            if (checkGuard)
            {
                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);

                // 패링 타이밍 확인 부분
                if (farryTime > 0)
                {
                    // 적의 공격 매소드 호출
                }
            }

            // 패링타이밍에 공격감지
            if (chkEnemyAttack)
            {
                isFarrying = true;

                // 패링 모션
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;
                // 플레이어 상태
                playerBattleState = PlayerBattleState.Farrying;
                // 플레이어 애니메이션
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
                checkGuard = false;
            }

            // 패링 타이밍 이 아니거나 공격감지 아닐때
            else if(farryTime <= 0 && checkGuard)
            {
                checkGuard = false;

                isGuard = true;

                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);
            }
        }
    }

    // Attack1, Attack2 애니메이션 에서 Add Event 로 호출
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyObj)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // 여기안에 적 피격 넣을것
                Debug.Log("EnemyHit");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce,ForceMode2D.Impulse);
            }
        }
    }

    // Farry1, Farry2 애니메이션 에서 Add Event 로 호출
    public void FarryAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // 여기안에 패리 기능 넣을것
                Debug.Log("FarryAttack");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * testForce, ForceMode2D.Impulse);
            }
        }
    } 

    // 방어 판정
    public void GuardAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // 여기안에 가드 기능 넣을것
                Debug.Log("GuardAttack");

                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce, ForceMode2D.Impulse);
            }
        }
    }

    // 애니메이션 Add Event 에 넣어짐
    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
        isAttack = false;
        isFarrying = false;
        isGuard = false;
    }


    IEnumerator GuardOrFarry()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Debug.Log("Farry");
                yield break;
            }
        }

        yield return new WaitForSeconds(resetFarryTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
