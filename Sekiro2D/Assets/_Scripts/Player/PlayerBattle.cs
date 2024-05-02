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

    // 공격
    public GameObject attackPoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private bool isAttack;
    private int attackCombo = 0;

    // 방어
    public GameObject guardPoint;
    public Vector2 boxSize;
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;

    private int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // 체력
    public float playerHp;



    private void Update()
    {
        KeyInput();
        Attack();
        Guard();
    }

    public void KeyInput()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("어택");
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
                attackCombo++;

                if (attackCombo > 2)
                {
                    attackCombo = 1;
                    attackTimeCount = 0;
                }

                playerBattleState = PlayerBattleState.Attack;
                playerAnim.SetTrigger("isAttack" + attackCombo);

                attackTimeCount = 0;
                
                isAttack = false;
            }
        }
        //공격 가능 시간이 리셋 시간 보다 작을떄
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
        
        if (inputGuard)
        {
            if (farryTime > 0)
                farryTime -= Time.deltaTime;

            chkEnemyAttack = Physics2D.OverlapBox(
                guardPoint.transform.position, boxSize, 0, enemyLayer);

            // 패링타이밍에 공격감지
            if (farryTime > 0 && chkEnemyAttack)
            {
                // 패링 모션 순서
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;

                playerBattleState = PlayerBattleState.Farrying;
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
            }

            // 패링 타이밍 이 아니거나 공격감지 아닐때
            else if(farryTime <= 0 && checkGuard)
            {
                checkGuard = false;

                playerBattleState = PlayerBattleState.Guard;
                Debug.Log(playerBattleState);

                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);
            }
        }
    }

    public void SetStateIdle()
    {
        playerBattleState = PlayerBattleState.Idle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(guardPoint.transform.position, boxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.transform.position,new Vector2(2,1));
    }
}
