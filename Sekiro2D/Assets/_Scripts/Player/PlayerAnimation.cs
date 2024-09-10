using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAnimation : MonoBehaviour
{
    public PlayerBattle pB;
    private PlayerMovement pM;

    public Animator playerAnim;
    private Transform cusorPos;

    [Space(10)]
    // 플레이어 상태
    public PlayerState pState;
    public PlayerBattleState pBS;

    private PlayerState lastPlState;
    private PlayerBattleState lastPBS;

    [Space(10)]
    // 움직임
    private float inputX;
    private bool lastGrd;

    [Space(10)]
    // 지형체크
    private bool grdCheck;

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        playerAnim = GetComponent<Animator>();

        pM = GetComponent<PlayerMovement>();
        if (pM == null)  // 자신개체 에 컴포넌트가 없을경우 부모의 컴포넌트를 가져옴
            pM = GetComponentInParent<PlayerMovement>();

        pB = GetComponent<PlayerBattle>();
        if (pB == null)
            pB = GetComponentInParent<PlayerBattle>();

        cusorPos = GameObject.FindWithTag("Cusor").transform;
    }

    private void LateUpdate()
    {
        if (pBS == PlayerBattleState.Die)
            return;

        if (GameManager.GamePause)
            return;

        UpdateParameter();

        GroundAnimCheck(grdCheck);

        PlayerAnimUpdate(pState);
        BattleAnimUpdate(pBS);

        // 공격 및 피격 상태가 아닐때
        if (pBS != PlayerBattleState.Attack && pBS != PlayerBattleState.Hit)
            LookCusorRotation();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    // 마우스 위치 바라보기
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        if (!pM.isWallSlide)
        {
            // 마우스 위치가 오른쪽
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
            // 마우스 위치가 왼쪽
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

    private void GroundAnimCheck(bool grdCheck)
    {
        playerAnim.SetBool("isGround", grdCheck);
        playerAnim.SetBool("isFalling", !grdCheck);
    }

    #region 애니메이션 상태 관련
    /// <summary>
    /// 애니메이션 상태변경
    /// </summary>
    /// <param name="plMove"></param>
    private void PlayerAnimUpdate(PlayerState plMove)
    {
        if (lastPlState == plMove || pBS == PlayerBattleState.Die)
            return;

        if (pM.Ground)
            playerAnim.SetBool("isMove", pM.inputX != 0 ? true : false);
        else
            playerAnim.SetBool("isMove", false);
        
        playerAnim.SetBool("isFalling", !pM.Ground);
        playerAnim.SetBool("isWallSlide", pM.isWallSlide);

        switch (plMove)
        {
            // case PlayerState.Idle:
            // case PlayerState.Move:
            // case PlayerState.Falling:
            // case PlayerState.WallSlideRight:
            // case PlayerState.WallSlideLeft:
            //     break;

            case PlayerState.Jump:
                playerAnim.SetTrigger("isJump");
                break;

            default:
                break;
        }

        lastPlState = plMove;
    }

    /// <summary>
    /// pBState 상태의 따라 애니메이션 상태 변경
    /// </summary>
    /// <param name="pbs"></param>
    void BattleAnimUpdate(PlayerBattleState pbs)
    {
        if (lastPBS == pbs)
            return;

        // 플레이어 애니메이션 재생
        switch (pbs)
        {
            // case PlayerBattleState.Idle:
            // case PlayerBattleState.Guard:
            // case PlayerBattleState.Farrying:
            //     break;

            case PlayerBattleState.Attack:
                playerAnim.SetTrigger("isAttack" + pB.attackCombo);
                break;

            case PlayerBattleState.Hit:
                playerAnim.SetTrigger("isHurt");
                break;

            case PlayerBattleState.Die:
                playerAnim.SetTrigger("isDie");
                break;

            defaault:
                break;
        }

        lastPBS = pbs;
    }
    #endregion

    /// <summary>
    /// pM 과 pB 의 변수, 상태를 불러옴
    /// </summary>
    void UpdateParameter()
    {
        pState = pM.PState;
        pBS = pB.PBState;

        inputX = pM.inputX;
        grdCheck = pM.Ground;
    }

    public void SetMoveStateIdle()
    {
        // playerAnim.SetBool("isMove", false);
        playerAnim.SetBool("isFalling", false);
        playerAnim.SetBool("isWallSlide", false);
    }

    void AirStateIdle()
    {
        playerAnim.SetBool("isMove", false);
    }

    // 애니메이션에서 호출됨 삭제 금지
    void SetStateIdle()
    {
        pB.SetStateIdle();
    }
}