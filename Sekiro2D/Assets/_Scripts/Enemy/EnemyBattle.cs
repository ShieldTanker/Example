using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyBattleState
{
    Idle,
    Attack,
    Guard,
    Farryed,
    Hurt,
    Die
}

public class EnemyBattle : MonoBehaviour
{
    // 에너미
    [Space(10)]
    public Transform enemyRayPos;
    public float moveSpeed;
    [Tooltip("에너미 위치")] private Transform enemyPos;

    // 플레이어
    [Space(10)]
    public PlayerBattle player;
    public Transform playerRayPos;
    private PlayerBattleState pBState;

    // 애니메이션 관련
    [Space(10)]
    private Animator eAnim;

    //오디오 관련
    [Space(10)]
    private AudioSource audioSource;
    private EnemyAudio enemyAudio;

    #region 전투 관련

    // 전투 범위 관련
    [Space(10)]
    [Tooltip("공격시 감지한 콜라이더들을 담을 배열")] private Collider2D[] atkColl;
    [Tooltip("에너미의 공격 위치")] public Transform enemyAtkPoint;
    [Tooltip("감지할 플레이어 레이어")] public LayerMask playerLayer;
    [Tooltip("공격위치에서 얼마만큼 공격할지 범위")] public Vector2 atkBoxSize;

    // 목적지
    [Space(10)]
    [Tooltip("정지 거리")] public float stopPos;
    [Tooltip("최대 감지 거리")] public float maxDistance;
    [Tooltip("현재 거리")] float currentDistance;
    [Tooltip("목표와 의 거리")] float waypointDistance;
    [Tooltip("플레이어 와 에너미의 방향 계산")] Vector2 checkDir;

    [Space(10)]
    [Tooltip("순찰할 위치")]public Transform[] wayPoints;
    private int pointIdx;

    // 공격 관련
    [Space(10)]
    public float atkDamage;

    [Tooltip("공격 딜레이 시간")] private float delayAttack;
    [Tooltip("공격 딜레이 시간을 초기화")] public float initAttackDelay;

    [Tooltip("넉백을 줄 힘의 크기")] public float knockBackForce;
    [Tooltip("설정 시간동안 넉백")] public float knockBackTime;

    [Tooltip("경직무효 시간")] public float noStiffTime;

    //체력 관련
    [Space(10)]
    public float enemyHp;
    public float enemyMaxHp;
    public Slider hpBar;
    public GameObject hpCanvas;

    // 전투 상태 관련
    [Space(10)]
    [Tooltip("적 전투 상태")] public EnemyBattleState enemyBattleState;
    [Tooltip("마지막 적 전투 상태")] private EnemyBattleState lastEBS;
    [Tooltip("패링 당했을시 행동불가 시간")] public float FarryDelay;
    public bool isFarryed;

    [Tooltip("피격시 실행할 코루틴을 담을 변수")] IEnumerator setStateIdle;

    #endregion

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        enemyPos = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        enemyAudio = GetComponent<EnemyAudio>();

        playerRayPos = GameObject.Find("PRayPos").transform;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerBattle>();

        eAnim = GetComponent<Animator>();
        if (eAnim == null)
            eAnim = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (lastEBS == EnemyBattleState.Die)
            return;
        pBState = player.PBState;

        TimeCheck();

        DistanceCheck();

        CheckPlayer();

        AttackDelay();

        EnemyBattleAnimUpdate(enemyBattleState);
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    /// <summary>
    /// 상태에 따른 애니메이션 변경
    /// </summary>
    /// <param name="eBS"></param>
    void EnemyBattleAnimUpdate(EnemyBattleState eBS)
    {
        if (lastEBS == eBS)
            return;

        switch (eBS)
        {
            // case EnemyBattleState.Idle:
            // case EnemyBattleState.Guard:
            //     break;

            case EnemyBattleState.Attack:
                eAnim.SetTrigger("EnemyAttack");
                break;

            case EnemyBattleState.Farryed:
                eAnim.SetTrigger("FarryedAttack");
                break;

            case EnemyBattleState.Hurt:
                eAnim.SetTrigger("isHurt");
                enemyAudio.ChangeSound(audioSource, AudioState.HurtSound);
                break;

            case EnemyBattleState.Die:
                eAnim.SetTrigger("isDie");
                eAnim.SetBool("enemyDied", true);
                enemyAudio.ChangeSound(audioSource, AudioState.DieSound);
                break;

            default:
                break;
        }

        lastEBS = eBS;
    }

    #region 전투 관련 함수들
    public void AttackPlayer()
    {
        Collider2D playerColl = AtkCollider(enemyAtkPoint, atkBoxSize);

        // 감지된 콜라이더가 플레이어 일때
        if (playerColl != null)
        {
            PlayerBattle pB = playerColl.GetComponent<PlayerBattle>();
            PlayerBattleState pS = pB.PBState;

            if (pS == PlayerBattleState.Farrying)
            {   // 플레이어가 패링상태일때
                pB.Farryed();
                enemyBattleState = EnemyBattleState.Farryed;

                isFarryed = true;
                delayAttack = FarryDelay;
                Invoke("FalseFarryed", FarryDelay);
            }
            else if (pS == PlayerBattleState.Guard)
            {   // 플레이어가 가드상태일때
                pB.KnockBack(enemyPos, knockBackForce / 2, knockBackTime);
                pB.Guarded();
            }
            else
            {   // 가드,패링 상태가 아닐때
                pB.KnockBack(enemyPos, knockBackForce, knockBackTime);
                pB.TakeDamage(atkDamage);
            }
        }
    }
    void FalseFarryed()
    {
        isFarryed = false;
        enemyBattleState = EnemyBattleState.Idle;
    }

    // 공격시 닿은 물체 확인
    private Collider2D AtkCollider(Transform atkPoint, Vector2 boxSize)
    {
        Collider2D playerColl = new Collider2D();
        atkColl = Physics2D.OverlapBoxAll(atkPoint.position, boxSize, 0f, playerLayer);

        foreach (Collider2D col in atkColl)
        {
            playerColl = col;
        }

        return playerColl;
    }

    // 적 피격
    public void enemyHurt(float damage)
    {
        enemyHp -= damage;
        hpBar.value = enemyHp / enemyMaxHp;
        
        enemyAudio.ChangeSound(audioSource, AudioState.HurtSound);

        if (enemyHp > 0)
        {   // 피격
            enemyBattleState = EnemyBattleState.Hurt;

            if (setStateIdle != null)
                StopCoroutine(setStateIdle);

            setStateIdle = SetStateIdle(noStiffTime);
            StartCoroutine(setStateIdle);
        }
        else
        {   // 사망
            enemyBattleState = EnemyBattleState.Die;
            hpBar.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 피격후 설정 시간이후 정상상태로 변경
    /// </summary>
    /// <param name="noStiffTime"></param>
    /// <returns></returns>
    IEnumerator SetStateIdle(float noStiffTime)
    {
        yield return new WaitForSeconds(noStiffTime);
        enemyBattleState = EnemyBattleState.Idle;
    }


    // 타이머
    void TimeCheck()
    {
        if (delayAttack > 0)
            delayAttack -= Time.deltaTime;
    }
    void AttackDelay()
    {
        if (pBState != PlayerBattleState.Die)
        {
            if (currentDistance <= 1.8 && eAnim.GetBool("PlayerCheck") &&
                delayAttack <= 0 && !isFarryed && enemyBattleState != EnemyBattleState.Die)
            {
                eAnim.SetTrigger("EnemyAttack");
                delayAttack = initAttackDelay;
            }
        }
    }

    #endregion

    #region 순찰 및 추격 관련

    // 체크
    void CheckPlayer()
    {
        if (pBState != PlayerBattleState.Die)
        {
            checkDir = playerRayPos.position - enemyRayPos.position;

            Vector3 checkRay = maxDistance * checkDir.normalized;
            Debug.DrawRay(enemyRayPos.position, checkRay, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(enemyRayPos.position, checkDir, maxDistance, playerLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                    eAnim.SetBool("PlayerCheck", true);
                else
                    eAnim.SetBool("PlayerCheck", false);
            }
            else
                eAnim.SetBool("PlayerCheck", false);
        }
        else
        {
            eAnim.SetBool("playerDie", true);
        }
    }

    // 거리 계산
    void DistanceCheck()
    {
        // 플레이어 와 자신의 거리를 계산하여 FSM 거리계산에 사용
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
        eAnim.SetFloat("playerChkDis", currentDistance);

        // 목적지와 자신의 거리를 계산하여 FSM 거리계산에 사용
        waypointDistance = Vector2.Distance(transform.position, wayPoints[pointIdx].position);
        eAnim.SetFloat("distanceForPoint", waypointDistance);
    }

    // 목표로 이동
    public void GoToTarget()
    {
        if (pBState != PlayerBattleState.Die)
        {
            LookAHead(player.transform);

            Vector3 dir = player.transform.position;

            if (currentDistance >= stopPos)
                transform.position =
                    Vector3.MoveTowards(transform.position,
                                        dir,
                                        moveSpeed * Time.deltaTime);
        }
    }

    // 목적지로 이동
    public void GoToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[pointIdx].position, moveSpeed * Time.deltaTime);
        LookAHead(wayPoints[pointIdx]);
    }

    // 목적지 재설정
    public void WayPointSet()
    {
        switch (pointIdx)
        {
            case 0:
                pointIdx = 1;
                break;
            case 1:
                pointIdx = 0;
                break;
            default:
                break;
        }
    }

    #endregion

    /// <summary>
    /// 매개변수의 위치 방향으로 회전
    /// </summary>
    /// <param name="pos">목표의 위치</param>

    public void LookAHead(Transform pos)
    {
        float dir = pos.position.x - transform.position.x;

        if (dir > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPoint.position, atkBoxSize);
    }
}
