using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerBattle : MonoBehaviour
{
    public Animator playerAnim;
    public LayerMask enemyLayer;

    // 공격
    public GameObject attackPoint;
    public float farryTime;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private bool isAttack;

    // 방어
    public GameObject guardPoint;
    public Vector2 boxSize;
    public bool chkEnemyAttack;
    public bool isFarrying;
    private int farryCount;
    public float resetFarryTime;
    public float farryInTime;

    // 체력
    public float playerHp;

    private int attackCombo = 0;


    private void Update()
    {
        KeyInput();
        Attack();
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
            chkEnemyAttack = Physics2D.OverlapBox(guardPoint.transform.position, boxSize, 0, enemyLayer);
            
            Guard();
        }
        else if(Input.GetKeyUp(KeyCode.Mouse1))
        {
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

                playerAnim.SetTrigger("isAttack" + attackCombo);
                attackTimeCount = 0;
            }

            //공격 가능 시간이 리셋 시간 보다 작을떄
            if (attackTimeCount < resetComboTime)
                attackTimeCount += Time.deltaTime;
            else
            {
                attackCombo = 0;
                attackTimeCount = 0;
            }
        }
    }

    public void Guard()
    {
        farryTime -= Time.deltaTime;

        // 패링타이밍에 공격감지
        if (farryTime > 0 && chkEnemyAttack)
        {
            farryCount++;

            if (farryCount > 2)
                farryCount = 1;

            playerAnim.SetTrigger("isFarry" + farryCount);

            chkEnemyAttack = false;
            farryTime = resetFarryTime;

            playerAnim.SetBool("idleGuard", false);
        }
        
        // 패링 타이밍 이 아니거나 공격감지 아닐때
        else
        {
            playerAnim.SetTrigger("isGuard");
            playerAnim.SetBool("idleGuard", true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(guardPoint.transform.position, boxSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPoint.transform.position,new Vector2(2,1));
    }
}
