using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMoveAnimUpdate : MonoBehaviour
{
    public Transform cusorPos;
    public Animator playerAnim;

    public PlayerBattle pB;
    public PlayerManager pM;

    // 플레이어 상태
    public PlayerState plState;
    PlayerState lastPlState;
    public PlayerBattleState plBattleState;

    // 움직임
    private float inputX;
    private bool lastGrd;

    // 지형체크
    private bool grdCheck;

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (plBattleState == PlayerBattleState.Die)
            return;

        if (GameManager.GamePause)
            return;

        GroundAnimCheck(grdCheck);

        WallAnimCheck(PlayerMovement.RightTopSensor, PlayerMovement.LeftTopSensor, grdCheck);

        UpdateParameter();

        PlayerAnimUpdate(plState);

        if (plBattleState != PlayerBattleState.Attack &&
            plBattleState != PlayerBattleState.Hit)
            LookCusorRotation();

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
                WalkAnim(1);
            //왼쪽으로 이동
            else if (inputX < 0)
                WalkAnim(-1);
        }
        // 마우스가 왼쪽
        else if (cusorX < 0)
        {
            LookAHead(new Vector3(-1, 1, 1));

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkAnim(-1);

            //왼쪽으로 이동
            else if (inputX < 0)
                WalkAnim(1);
        }
        if (lastPlState == PlayerState.WallSlideLeft)
            LookAHead(new Vector3(-1, 1, 1));
        else if (lastPlState == PlayerState.WallSlideRight)
            LookAHead(Vector3.one);
    }
    
    // 바라보는 방향
    void LookAHead(Vector3 lookAHead)
    {
        transform.localScale = lookAHead;
    }

    //앞으로 걷기 뒤로걷기
    void WalkAnim(int value)
    {
        playerAnim.SetFloat("runSpeed", value);
    }

    void StartSetting()
    {
        playerAnim = GetComponent<Animator>();
        cusorPos = GameObject.Find("Cusor").transform;
    }

    private void GroundAnimCheck(bool grdCheck)
    {
        if (lastGrd == grdCheck)
            return;

        lastGrd = grdCheck;

        playerAnim.SetBool("isGround", lastGrd);
        playerAnim.SetBool("isFalling", !lastGrd);
    }

    private void WallAnimCheck(bool rightTopSen, bool leftTopSen, bool grdCheck)
    {
        if (rightTopSen && !grdCheck)
        {
            pM.PManager.PlState = PlayerState.WallSlideRight;
        }
        else if (leftTopSen && !grdCheck)
        {
            pM.PManager.PlState = PlayerState.WallSlideLeft;
        }
    }

    private void PlayerAnimUpdate(PlayerState plMove)
    {
        if (lastPlState == plMove)
            return;

        switch (plMove)
        {
            case PlayerState.Idle:
                SetMoveStateIdle();
                break;

            case PlayerState.Move:
                playerAnim.SetBool("isMove", true);
                break;

            case PlayerState.Jump:
                playerAnim.SetTrigger("isJump");
                break;

            case PlayerState.Falling:
                AirStateIdle();
                playerAnim.SetBool("isFalling",true);
                break;

            case PlayerState.WallSlideRight:
                playerAnim.SetBool("isWallRight", true);
                break;

            case PlayerState.WallSlideLeft:
                playerAnim.SetBool("isWallLeft", true);
                break;
            default:
                break;
        }

        lastPlState = plMove;
    }

    void UpdateParameter()
    {
        // 상태 관련
        plState = pM.PManager.PlState;
        plBattleState = pM.PManager.PlBattleState;

        inputX = PlayerMovement.inputX;
        grdCheck = PlayerMovement.Ground;
    }

    public void SetMoveStateIdle()
    {
        playerAnim.SetBool("isMove", false);
        playerAnim.SetBool("isFalling", false);
    }

    void AirStateIdle()
    {
        playerAnim.SetBool("isMove", false);
        playerAnim.SetBool("isWallRight", false);
        playerAnim.SetBool("isWallLeft", false);
    }
    // 애니메이션에서 호출됨 삭제 금지
    void SetStateIdle()
    {
        pB.SetStateIdle();
    }
}
