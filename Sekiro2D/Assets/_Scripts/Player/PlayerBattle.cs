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

    //�÷��̾� ����
    public static PlayerBattleState playerBattleState;

    // ����
    public GameObject attackPoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private bool isAttack;
    private int attackCombo = 0;

    // ���
    public GameObject guardPoint;
    public Vector2 boxSize;
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;

    private int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // ü��
    public float playerHp;



    private void Update()
    {
        KeyInput();
        Attack();
        Guard();
    }

    public void KeyInput()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("����");
            isAttack = true;
        }
        
        //���
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
            // ���ݰ��� �ð��� ���� ������ �ð����� ������
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
        //���� ���� �ð��� ���� �ð� ���� ������
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

            // �и�Ÿ�ֿ̹� ���ݰ���
            if (farryTime > 0 && chkEnemyAttack)
            {
                // �и� ��� ����
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;

                playerBattleState = PlayerBattleState.Farrying;
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
            }

            // �и� Ÿ�̹� �� �ƴϰų� ���ݰ��� �ƴҶ�
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
