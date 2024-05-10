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
    // �ִϸ��̼�
    public Animator playerAnim;

    //�÷��̾� ����
    public static PlayerBattleState playerBattleState;
    bool isGround;
    private Rigidbody2D rb;

    // ü��
    public float playerHp;
    public bool playerHit;

    // ���� ����
    static public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    private int attackCombo = 0;

    // ���� ���� ����
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // ���,�и�
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
        // ����
        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            isAttack = true;
        }
        else if (!isAttack)
        {
            //���
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
            // ���ݰ��� �ð��� ���� ������ �ð����� ������
            if (attackTimeCount > delayAttackTime)
            {
                //���ݸ��
                AttackCombo(2);
                // �÷��̾� ����
                playerBattleState = PlayerBattleState.Attack;
                // �÷��̾� �ִϸ��̼� ���
                playerAnim.SetTrigger("isAttack" + attackCombo);
                
                attackTimeCount = 0;
            }
        }
        //���� ���� �ð��� ���� �ð� ���� ������
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


    // Attack1, Attack2 �ִϸ��̼� ���� Add Event �� ȣ��
    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
            // ����ȿ� �� �ǰ� ������
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
        //���ݸ��
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

    // ���� �ִϸ��̼� Ȱ��ȭ
    private void SetAnimationGuard()
    {
        playerAnim.SetTrigger("isGuard");
        playerAnim.SetBool("idleGuard", true);
    }

    // �÷��̾� ������ ����
    public void TakeDamage(float damage)
    {
        if (playerBattleState == PlayerBattleState.Die)
            return;

        if (playerHp > 0)
        {
            playerHp -= damage;
            // ���߿� �ǰ� �ִϸ��̼� �ֱ�
        }
        else
        {
            playerBattleState = PlayerBattleState.Die;
            // ���߿� ��� �ִϸ��̼� �ֱ�
        }
    }

    // �ִϸ��̼� Add Event �� �־���
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
