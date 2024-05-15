using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerLookUpdate : MonoBehaviour
{
    public Transform cusorPos;
    public Animator playerAnim;

    public PlayerBattle pB;

    // 플레이어 상태
    public PlayerState plState;

    // 움직임
    private float inputX;
    private bool lastGrd;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (PlayerManager.PManager.PlState != PlayerState.Die)
        {
            plState = PlayerManager.PManager.PlState;
            inputX = PlayerMovement1.inputX;

            GroundAnimCheck(PlayerMovement1.Ground);


            PlayerMoveAnimUpdate(plState);

            if (plState != PlayerState.Attack && plState != PlayerState.Hit)
                LookCusorRotation();
        }
    }

    // 마우스 위치 바라보기
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        // 마우스 오른쪽
        if (cusorX > 0)
        {
            LookAHead(new Vector3(1, 1, 1));

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkFowardAnim();
            //왼쪽으로 이동
            else if (inputX < 0)
                WalkBackAnim();
        }
        // 마우스가 왼쪽
        else if (cusorX < 0)
        {
            LookAHead(new Vector3(-1, 1, 1));

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkBackAnim();

            //왼쪽으로 이동
            else if (inputX < 0)
                WalkFowardAnim();
        }
    }
    
    // 바라보는 방향
    void LookAHead(Vector3 lookAHead)
    {
        transform.localScale = lookAHead;
    }

    //앞으로 걷기 뒤로걷기
    void WalkFowardAnim()
    {
        playerAnim.SetFloat("runSpeed", 1);
    }
    void WalkBackAnim()
    {
        playerAnim.SetFloat("runSpeed", -1);
    }

    void StartSetting()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void GroundAnimCheck(bool grdCheck)
    {
        if (lastGrd == grdCheck)
            return;

        playerAnim.SetBool("isGround", grdCheck);

        lastGrd = grdCheck;
    }

    private void PlayerMoveAnimUpdate(PlayerState plMove)
    {

        switch (plMove)
        {
            case PlayerState.Idle:
                playerAnim.SetBool("isMove", false);
                break;
            case PlayerState.Move:
                playerAnim.SetBool("isMove", true);
                break;
            case PlayerState.Jump:
                playerAnim.SetTrigger("isJump");
                break;
            default:
                break;
        }
    }
    
    public void SetStateIdle()
    {
        pB.SetStateIdle();
    }
}
