using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMoveAnimUpdate : MonoBehaviour
{
    public Transform cusorPos;
    public Animator playerAnim;

    public PlayerBattle pB;
    public PlayerManager pM;

    // �÷��̾� ����
    public PlayerState plState;
    PlayerState lastPlState;
    public PlayerBattleState plBattleState;

    // ������
    private float inputX;
    private bool lastGrd;

    // ����üũ
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


    // ���콺 ��ġ �ٶ󺸱�
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        // ���콺 ������
        if (cusorX > 0)
        {
            LookAHead(new Vector3(1, 1, 1));

            // ���������� �̵�
            if (inputX > 0)
                WalkAnim(1);
            //�������� �̵�
            else if (inputX < 0)
                WalkAnim(-1);
        }
        // ���콺�� ����
        else if (cusorX < 0)
        {
            LookAHead(new Vector3(-1, 1, 1));

            // ���������� �̵�
            if (inputX > 0)
                WalkAnim(-1);

            //�������� �̵�
            else if (inputX < 0)
                WalkAnim(1);
        }
        if (lastPlState == PlayerState.WallSlideLeft)
            LookAHead(new Vector3(-1, 1, 1));
        else if (lastPlState == PlayerState.WallSlideRight)
            LookAHead(Vector3.one);
    }
    
    // �ٶ󺸴� ����
    void LookAHead(Vector3 lookAHead)
    {
        transform.localScale = lookAHead;
    }

    //������ �ȱ� �ڷΰȱ�
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
        // ���� ����
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
    // �ִϸ��̼ǿ��� ȣ��� ���� ����
    void SetStateIdle()
    {
        pB.SetStateIdle();
    }
}
