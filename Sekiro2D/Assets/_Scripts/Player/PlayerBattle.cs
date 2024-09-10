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
    // 사운드
    public AudioSource battleAudioSource;
    private PlayerAudio pAudio;

    // 애니메이션
    [Space(10)]
    public Animator playerAnim;

    public PlayerMovement pMove;

    //플레이어 상태
    [Space(10)]
    [SerializeField] private PlayerBattleState pBState;
    public PlayerBattleState PBState { get {return pBState; } set{ pBState = value; } }

    private Rigidbody2D rb;
    private bool isGround;

    // 체력
    [Space(10)]
    private float playerHp;
    public float Hp
    {
        get
        { return playerHp; }
        set
        {
            if (value <= 0)
            {
                playerHp = 0f;
            }
            else
            {
                playerHp = value;
            }
        }
    }
    public float maxHp;

    #region 공격 관련

    [Space(10)]
    // 공격 관련
    public float atkDamage;
    public bool isAttack;
    public float resetComboTime;
    public float delayAttackTime;
    public float attackTimeCount;
    public int attackCombo = 0;

    [Space(10)]
    // 공격 범위 관련
    public Transform battlePoint;
    public Vector2 battleBoxSize;
    public LayerMask enemyLayer;
    public Collider2D[] enemyObj;

    #endregion

    #region 방어, 체력 관련

    // 방어,패링
    [Space(10)]
    public bool inputGuard;
    public static int farryCount;
    public float resetFarryTime;
    public float farryTime;
    public bool isKnockBack;

    #endregion

    [Tooltip("패링상태 에서 가드상태 로 넘어가는 코루틴 을 담는 변수")] IEnumerator guardOrFarry;
    [Tooltip("넉백 코루틴 담는 함수")] IEnumerator knockBack;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        playerHp = maxHp;
        ChangeHpBarValue();

        pMove = GetComponent<PlayerMovement>();
        if (pMove == null)
            pMove = GetComponentInParent<PlayerMovement>();
        
        battleAudioSource = GetComponent<AudioSource>();
        pAudio = GetComponent<PlayerAudio>();

        guardOrFarry = GuardOrFarry();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (pBState != PlayerBattleState.Die)
        {
            isGround = pMove.Ground;

            KeyInput();

            //공격 가능 시간이 리셋 시간 보다 작을때
            ResetAttackComboTimeCount(resetComboTime);
        }
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    public void KeyInput()
    {
        if (GameManager.GamePause)
            return;

        if (pBState != PlayerBattleState.Hit &&
            pBState != PlayerBattleState.Die)
        {
            // 공격
            if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
            {
                Attack();
            }
            // 공격중이 아닐때
            else if (!isAttack &&
                !(pMove.PState == PlayerState.WallSlideLeft || pMove.PState == PlayerState.WallSlideRight))
            {
                //방어
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    if (guardOrFarry != null)
                        StopCoroutine(guardOrFarry);

                    guardOrFarry = GuardOrFarry();

                    inputGuard = true;
                    StartCoroutine(guardOrFarry);
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
            pBState = PlayerBattleState.Attack;
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

    /// <summary>
    /// 패링 가능 시간이 지나면 가드 상태로 변환
    /// </summary>
    /// <returns></returns>
    IEnumerator GuardOrFarry()
    {
        SetAnimationGuard();
        pBState = PlayerBattleState.Farrying;

        yield return new WaitForSeconds(resetFarryTime);

        if (inputGuard)
        {
            pBState = PlayerBattleState.Guard;
        }
    }

    public void Farryed()
    {
        // 플레이어 패링 애니메이션 재생
        farryCount = ActionCombo(farryCount,2);
        playerAnim.SetTrigger("isFarry" + farryCount);

        pAudio.FarrySound();
    }

    public void Guarded()
    {
        pAudio.ChangeSound(battleAudioSource, AudioState.GuardSound);
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
        if (pBState == PlayerBattleState.Die)
            return;

        SetStateIdle();

        playerHp -= damage;
        
        ChangeHpBarValue();

        if (playerHp > 0)
        {
            pBState = PlayerBattleState.Hit;
            pAudio.ChangeSound(battleAudioSource, AudioState.HurtSound);
        }
        else
        {
            pBState = PlayerBattleState.Die;
            pAudio.ChangeSound(battleAudioSource, AudioState.DieSound);
            UiManager.UIManager.ShowGameOver();
        }
    }

    #region 넉백

    public void KnockBack(Transform enemy, float knockBackForce, float knockBackTime)
    {
        if (knockBack != null)
            StopCoroutine(knockBack);

        knockBack = KnockBackCoroutine(enemy, knockBackForce, knockBackTime);
        StartCoroutine(knockBack);
    }

    IEnumerator KnockBackCoroutine(Transform enemy, float knockBackForce, float knockBackTime)
    {
        #region 방향 계산

        float dir = gameObject.transform.position.x - enemy.position.x;

        Vector2 knockBackVec;

        if (dir > 0)
            knockBackVec = Vector2.right;
        else if (dir < 0)
            knockBackVec = Vector2.left;
        else
            knockBackVec = Vector2.zero;

        #endregion

        rb.velocity = knockBackVec * knockBackForce;

        isKnockBack = true;

        yield return new WaitForSeconds(knockBackTime);

        rb.velocity = Vector2.zero;

        isKnockBack = false;

        if (pBState != PlayerBattleState.Guard && pBState != PlayerBattleState.Die)
        {
            SetStateIdle();
        }
    }

    #endregion

    public void SetStateIdle()
    {
        pBState = PlayerBattleState.Idle;

        isAttack = false;
        inputGuard = false;
        playerAnim.SetBool("idleGuard", false);
    }

    /// <summary>
    /// 플레이어의 체력이 변동될시 호출하여 HP 바 값 변경
    /// </summary>
    public void ChangeHpBarValue()
    {
        UiManager.UIManager.PlayerHpBarChange(playerHp, maxHp);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(battlePoint.position, battleBoxSize);
    }
}
