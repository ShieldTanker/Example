using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    // 목적지
    public Transform[] wayPoints;
    int pointIdx;

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

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
    }

    private void Update()
    {
        currentDistance = Vector2.Distance(player.transform.position, transform.position);
        checkDir = playerRayPos.position- enemyRayPos.position;

        waypointDistance = Vector2.Distance(transform.position, wayPoints[pointIdx].position);
        animator.SetFloat("distanceForPoint", waypointDistance);

        animator.SetFloat("playerChkDis", currentDistance);

        ray = new Ray(enemyRayPos.position, checkDir);
        
        Vector3 checkRay = maxDistance * checkDir.normalized;
        Debug.DrawRay(enemyRayPos.position,checkRay, Color.red);

        if (Physics.Raycast(ray, out hit , maxDistance))
        {
            if (hit.collider.gameObject == player)
                animator.SetBool("PlayerCheck", true);
            else
                animator.SetBool("PlayerCheck", false);
        }
        else
            animator.SetBool("PlayerCheck", false);
        GoToWayPoint();
    }

    public void GoToWayPoint()
    {
        transform.position = Vector3.Lerp(transform.position, wayPoints[pointIdx].position, moveSpeed * Time.deltaTime);
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
