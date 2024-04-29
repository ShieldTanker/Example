using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BattleState
{
    Idle,
    Attacking,
    Guard,
}
public class PlayerBattle : MonoBehaviour
{
    public Animator playerAnim;

    public GameObject attackPoint;
    public GameObject guardPoint;

    public float attackTime;
    public float farryTime;
    public float waitActiveTime;

    private BattleState battleState;
    private int attackCombo = 0;
    private float attackTimeCount = 0f;

    public float resetComboTime;
    public float delayAttackTime;

    

    private void Update()
    {
        KeyInput();
        Attack();
        //Guard();
    }

    public void KeyInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            battleState = BattleState.Attacking;
        }

        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            battleState = BattleState.Guard;
            guardPoint.SetActive(true);

            playerAnim.SetTrigger("isGuard");
            playerAnim.SetBool("idleGuard", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            battleState = BattleState.Idle;
            guardPoint.SetActive(false);
            playerAnim.SetBool("idleGuard", false);
        }
    }

    public void Attack()
    {
        if (battleState == BattleState.Attacking && attackTimeCount > delayAttackTime)
        {
            attackCombo++;

            if (attackCombo > 2)
            {
                attackCombo = 1;
                battleState = BattleState.Idle;
                attackTimeCount = 0;
            }

            StartCoroutine(AttackCoroutine("isAttack" + attackCombo));
            attackTimeCount = 0;
            battleState = BattleState.Idle;
        }

        if (attackTimeCount < resetComboTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;
            attackTimeCount = 0;

            battleState = BattleState.Idle;
        }
    }
    /*public void Guard()
    {
        if (battleState == BattleState.Guard)
        {
            
        }
        else
        {
            
        }
    }*/

    IEnumerator AttackCoroutine(string animation)
    {
        battleState = BattleState.Attacking;

        playerAnim.SetTrigger(animation);

        yield return new WaitForSeconds(waitActiveTime);

        attackPoint.SetActive(true);
        
        yield return new WaitForSeconds(attackTime);

        attackPoint.SetActive(false);
    }
    
}
