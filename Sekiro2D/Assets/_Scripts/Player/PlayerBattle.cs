using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBattle : MonoBehaviour
{
    // �ִϸ��̼�
    public Animator playerAnim;

    public PlayerManager pM;

    // ����
    private PlayerAudio plAudio;

    //�÷��̾� ����
    public PlayerBattleState pState;
    PlayerBattleState lastPBS;
    private Rigidbody2D rb;
    private bool isGround;

    // ü��
    public float playerHp;
    public float playerMaxHP;

    // ���� ����
    public float atkDamage;
    public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public int attackCombo = 0;

    // ���� ���� ����
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // ���,�и�
    public bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;


    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (pState != PlayerBattleState.Die)
        {
            isGround = PlayerMovement.Ground;

            pState = pM.PManager.PlBattleState;

            KeyInput();

            BattleAnimUpdate(pState);

            //���� ���� �ð��� ���� �ð� ���� ������
            ResetAttackComboTimeCount(resetComboTime);

        }
    }

    public void KeyInput()
    {
        if (pState != PlayerBattleState.Hit &&
            pState != PlayerBattleState.Die)
        {
            // ����
            if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
            {
                Attack();
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
                    playerAnim.SetBool("idleGuard", false);
                    SetStateIdle();
                }
            }
        }
    }

    public void Attack()
    {
        isAttack = true;

        // ���ݰ��� �ð��� ���� ������ �ð����� ������
        if (attackTimeCount > delayAttackTime)
        {
            attackTimeCount = 0;

            //���ݸ��
            attackCombo = ActionCombo(attackCombo,2);

            // �÷��̾� ����
            pM.PManager.PlBattleState = PlayerBattleState.Attack;
        }
    }
    private void ResetAttackComboTimeCount(float resetTime)
    {
        if (attackTimeCount < resetTime)
            attackTimeCount += Time.deltaTime;
        else
        {
            attackCombo = 0;
        }
    }

    // Attack1, Attack2 �ִϸ��̼� ���� Add Event �� ȣ��
    private int ActionCombo(int currentCombo, int maxCombo)
    {
        //���ݸ��
        currentCombo++;

        if (currentCombo > maxCombo)
        {
            currentCombo = 1;
        }
        return currentCombo;
    }

    public void AttackEnemy()
    {
        enemyObj = Physics2D.OverlapBoxAll(battlePoint.position, battleBoxSize, 0f, enemyLayer);

        foreach (Collider2D col in enemyObj)
        {
            EnemyBattle enemy = col.gameObject.GetComponent<EnemyBattle>();
            enemy.enemyHurt(atkDamage);
        }
    }

    IEnumerator GuardOrFarry()
    {
        SetAnimationGuard();
        pM.PManager.PlBattleState = PlayerBattleState.Farrying;

        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            pM.PManager.PlBattleState = PlayerBattleState.Guard;
        }
    }
    public void Farryed()
    {
        // �÷��̾� �и� �ִϸ��̼� ���
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        plAudio.FarrySound();
    }
    public void Guarded()
    {
        plAudio.GuardSound();
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
        if (pState == PlayerBattleState.Die)
            return;

        SetStateIdle();

        playerHp -= damage;
        
        ChangeHpBarValue();

        if (playerHp > 0)
        {
            pM.PManager.PlBattleState = PlayerBattleState.Hit;

            plAudio.HurtSound();
        }
        else
        {
            pM.PManager.PlBattleState = PlayerBattleState.Die;
        }
    }
    public IEnumerator KnockBack(Transform enemy, float knockBackForce, float knockBackTime)
    {
        float knockBackVecX = gameObject.transform.position.x - enemy.position.x;

        if (knockBackVecX > 0)
            knockBackVecX = 1;
        else if (knockBackVecX < 0)
            knockBackVecX = -1;

        Vector2 knockBackVec = new Vector2(knockBackVecX, transform.position.y);

        //rb.AddForce(knockBackVec * knockBackForce, ForceMode2D.Impulse);
        rb.velocity = knockBackVec * knockBackForce;

        yield return new WaitForSeconds(knockBackTime);

        rb.velocity = Vector2.zero;

        if (pState != PlayerBattleState.Guard && pState != PlayerBattleState.Die)
        {
            SetStateIdle();
        }
    }


    void BattleAnimUpdate(PlayerBattleState pbs)
    {
        if (lastPBS == pbs)
            return;

        // �÷��̾� �ִϸ��̼� ���
        switch (pbs)
        {
            case PlayerBattleState.Idle:
                break;
            case PlayerBattleState.Attack:
                playerAnim.SetTrigger("isAttack" + attackCombo);
                break;
            case PlayerBattleState.Guard:
                break;
            case PlayerBattleState.Farrying:
                break;
            case PlayerBattleState.Hit:
                playerAnim.SetTrigger("isHurt");
                break;
            case PlayerBattleState.Die:
                playerAnim.SetTrigger("isDie");
                break;
            default:
                break;
        }

        lastPBS = pbs;
    }

    // �ִϸ��̼� Add Event �� �־���
    public void SetStateIdle()
    {
        pM.PManager.PlBattleState = PlayerBattleState.Idle;

        isAttack = false;
        inputGuard = false;
    }
    void ChangeHpBarValue()
    {
        pM.PManager.PlayerHpBarChange(playerHp, playerMaxHP);
    }

    void StartSetting()
    {
        ChangeHpBarValue();
        pM.PManager.PlState = PlayerState.Idle;
        
        plAudio = GetComponent<PlayerAudio>();
        
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
