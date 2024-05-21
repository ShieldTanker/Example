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

    public Transform enemyRayPos;
    public Transform playerRayPos;

    public Animator animator;

    // 플레이어
    public GameObject player;

    // 본인 오브젝트
    public float moveSpeed;

    Vector2 checkDir;

    // 최대감지 거리
    public float maxDistance;
    // 현재거리
    float currentDistance;
    // 목표와의 거리
    float waypointDistance;

    Ray2D ray;
    RaycastHit2D hit;


    private void Update()
    {
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
        animator.SetFloat("playerChkDis", currentDistance);
        
        waypointDistance = Vector2.Distance(transform.position, wayPoints[pointIdx].position);
        animator.SetFloat("distanceForPoint", waypointDistance);

        checkDir = playerRayPos.position - enemyRayPos.position;
        ray = new Ray2D(enemyRayPos.position, checkDir);
        
        Vector3 checkRay = maxDistance * checkDir.normalized;
        Debug.DrawRay(enemyRayPos.position, checkRay, Color.red);
        
        hit = Physics2D.Raycast(enemyRayPos.position, checkDir, maxDistance , playerLayer);
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

    public void GoToWayPoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[pointIdx].position, moveSpeed * Time.deltaTime);

        CalcVec(wayPoints[pointIdx]);
    }

    public void GoToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

        CalcVec(player.transform);
    }

    void CalcVec(Transform vecWay)
    {
        float vecX = vecWay.position.x - transform.position.x;

        if (vecX > 1)
            transform.localScale = new Vector3(1, 1, 1);
        else if (vecX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
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
}
