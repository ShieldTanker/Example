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
    bool isGround;

    public float testForce;

    public Collider2D[] enemyCollider;
    public Collider2D[] enemyObj;


    // ����
    public Transform battlePoint;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public static int attackCombo = 0;
    private bool isAttack;
    public Vector2 battleBoxSize;

    // ���
    public bool chkEnemyAttack;
    public bool inputGuard;
    private bool checkGuard = false;
    public bool isGuard;

    public bool isFarrying;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;

    // ü��
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
        // ����
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
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

            // Debug.Log("ResetFarryTime");
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
                //���ݸ��
                attackCombo++;
                if (attackCombo > 2)
                {
                    attackCombo = 1;
                    attackTimeCount = 0;
                }

                // �÷��̾� ����
                playerBattleState = PlayerBattleState.Attack;

                // �÷��̾� �ִϸ��̼� ���
                playerAnim.SetTrigger("isAttack" + attackCombo);

                attackTimeCount = 0;
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
        if (farryTime > 0 && inputGuard)
            farryTime -= Time.deltaTime;

        if (inputGuard)
        {
            // ó�� ���� �ø���
            if (checkGuard)
            {
                playerBattleState = PlayerBattleState.Guard;
                playerAnim.SetTrigger("isGuard");
                playerAnim.SetBool("idleGuard", true);

                // �и� Ÿ�̹� Ȯ�� �κ�
                if (farryTime > 0)
                {
                    // ���� ���� �żҵ� ȣ��
                }
            }

            // �и�Ÿ�ֿ̹� ���ݰ���
            if (chkEnemyAttack)
            {
                isFarrying = true;

                // �и� ���
                farryCount++;
                if (farryCount > 2)
                    farryCount = 1;
                // �÷��̾� ����
                playerBattleState = PlayerBattleState.Farrying;
                // �÷��̾� �ִϸ��̼�
                playerAnim.SetTrigger("isFarry" + farryCount);

                farryTime = resetFarryTime;

                chkEnemyAttack = false;

                inputGuard = false;
                checkGuard = false;
            }

            // �и� Ÿ�̹� �� �ƴϰų� ���ݰ��� �ƴҶ�
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

    // Attack1, Attack2 �ִϸ��̼� ���� Add Event �� ȣ��
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyObj)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� �� �ǰ� ������
                Debug.Log("EnemyHit");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce,ForceMode2D.Impulse);
            }
        }
    }

    // Farry1, Farry2 �ִϸ��̼� ���� Add Event �� ȣ��
    public void FarryAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� �и� ��� ������
                Debug.Log("FarryAttack");
                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.right * testForce, ForceMode2D.Impulse);
            }
        }
    } 

    // ��� ����
    public void GuardAttack()
    {
        enemyCollider = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f);

        foreach (Collider2D col in enemyCollider)
        {
            if (col.gameObject.tag == "Enemy")
            {
                // ����ȿ� ���� ��� ������
                Debug.Log("GuardAttack");

                Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * testForce, ForceMode2D.Impulse);
            }
        }
    }

    // �ִϸ��̼� Add Event �� �־���
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
