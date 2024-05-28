using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBattle : MonoBehaviour
{
    // 애니메이션
    public Animator playerAnim;

    public PlayerManager pM;

    // 사운드
    private PlayerAudio plAudio;

    //플레이어 상태
    public PlayerBattleState pState;
    PlayerBattleState lastPBS;
    private Rigidbody2D rb;
    private bool isGround;

    // 체력
    public float playerHp;
    public float playerMaxHP;

    // 공격 관련
    public float atkDamage;
    public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public int attackCombo = 0;

    // 공격 범위 관련
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    // 방어,패링
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

            //공격 가능 시간이 리셋 시간 보다 작을때
            ResetAttackComboTimeCount(resetComboTime);

        }
    }

    public void KeyInput()
    {
        if (pState != PlayerBattleState.Hit &&
            pState != PlayerBattleState.Die)
        {
            // 공격
            if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
            {
                Attack();
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
                    playerAnim.SetBool("idleGuard", false);
                    SetStateIdle();
                }
            }
        }
    }

    public void Attack()
    {
        isAttack = true;

        // 공격가능 시간이 공격 딜레이 시간보다 많을때
        if (attackTimeCount > delayAttackTime)
        {
            attackTimeCount = 0;

            //공격모션
            attackCombo = ActionCombo(attackCombo,2);

            // 플레이어 상태
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

    // Attack1, Attack2 애니메이션 에서 Add Event 로 호출
    private int ActionCombo(int currentCombo, int maxCombo)
    {
        //공격모션
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
        // 플레이어 패링 애니메이션 재생
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        plAudio.FarrySound();
    }
    public void Guarded()
    {
        plAudio.GuardSound();
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

        // 플레이어 애니메이션 재생
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

    // 애니메이션 Add Event 에 넣어짐
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
