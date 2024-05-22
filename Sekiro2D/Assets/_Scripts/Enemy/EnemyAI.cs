using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // 목적지
    public Transform[] wayPoints;
    int pointIdx;
    public LayerMask playerLayer;
    public float disForPlayer;

    public Transform enemyRayPos;
    public Transform playerRayPos;

    public Animator animator;

    // 플레이어
    public GameObject player;
    PlayerBattleState pBState;

    // 본인 오브젝트
    public float moveSpeed;
    public EnemyBattleState enemyBState;

    Vector2 checkDir;

    // 최대감지 거리
    public float maxDistance;
    // 현재거리
    float currentDistance;
    // 목표와의 거리
    float waypointDistance;
    // 정지 거리
    public float stopPos;

    float delayAttack;
    public float initAttackDelay;


    RaycastHit2D hit;

    private void Start()
    {
        enemyBState = gameObject.GetComponent<EnemyBattleState>();
    }

    private void Update()
    {
        pBState = PlayerManager.PManager.PlBattleState;
        TimeCheck();

        DistanceCheck();

        CheckPlayer();

        AttackDelay();
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
            if (currentDistance <= 1.8 && animator.GetBool("PlayerCheck") &&
                delayAttack <= 0 && !EnemyBattle.isFarryed && enemyBState != EnemyBattleState.Die)
            {
                animator.SetTrigger("EnemyAttack");
                delayAttack = initAttackDelay;
            }
        }
    }
    
    // 체크
    void CheckPlayer()
    {
        if (pBState != PlayerBattleState.Die)
        {
            checkDir = playerRayPos.position - enemyRayPos.position;

            Vector3 checkRay = maxDistance * checkDir.normalized;
            Debug.DrawRay(enemyRayPos.position, checkRay, Color.red);

            hit = Physics2D.Raycast(enemyRayPos.position, checkDir, maxDistance, playerLayer);
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Player")
                    animator.SetBool("PlayerCheck", true);
                else
                    animator.SetBool("PlayerCheck", false);
            }
            else
                animator.SetBool("PlayerCheck", false);
        }
        else
        {
            animator.SetBool("playerDie", true);
        }
    }
    void DistanceCheck()
    {
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
        animator.SetFloat("playerChkDis", currentDistance);

        waypointDistance = Vector2.Distance(transform.position, wayPoints[pointIdx].position);
        animator.SetFloat("distanceForPoint", waypointDistance);
    }

    // 이동
    public void GoToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[pointIdx].position, moveSpeed * Time.deltaTime);
        CalcVec(wayPoints[pointIdx]);
    }
    public void GoToTarget()
    {
        if (pBState != PlayerBattleState.Die)
        {
            CalcVec(player.transform);

            Vector3 dir = player.transform.position;

            if (currentDistance >= stopPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, dir, moveSpeed * Time.deltaTime);
            }
        }
    }
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

    // 방향 계산
    void CalcVec(Transform vecWay)
    {
        float vecX = vecWay.position.x - transform.position.x;

        if (vecX > 1)
            transform.localScale = new Vector3(1, 1, 1);
        else if (vecX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }
}
