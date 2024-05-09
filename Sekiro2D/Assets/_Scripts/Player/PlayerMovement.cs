using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerMoveState
{
    Idle,
    Move,
    Jump,

}

public class PlayerMovement : MonoBehaviour
{
    public Transform cusorPos;

    public Animator playerAnim;

    // 플레이어 상태
    public PlayerBattleState pBState;
    PlayerMoveState plMove;

    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    private float inputX;
    private bool jumpInput;

    // 지형 체크
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    public bool grounded;


    private void Start()
    {
        moveX = transform.position.x;
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        pBState = PlayerBattle.playerBattleState;
        GroundCheck();
        
        KeyInput();
        
        PlayerMove();
        PlayerMoveAnimUpdate(plMove);

        LookCusorRotation();
    }

    // 마우스 위치 바라보기
    void LookCusorRotation()
    {
        float cusorX = cusorPos.position.x - transform.position.x;

        // 마우스 오른쪽
        if (cusorX > 0)
        {
            LookRight();

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
            LookLeft();

            // 오른쪽으로 이동
            if (inputX > 0)
                WalkBackAnim();

            //왼쪽으로 이동
            else if (inputX < 0)
                WalkFowardAnim();
        }
    }
    
    // 바라보는 방향
    void LookRight()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }
    void LookLeft()
    {
        transform.localScale = new Vector3(-1, 1, 1);
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

    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(
            grdCheckPoint.position, 0.1f, grdCheckLayerMask);
        playerAnim.SetBool("isGround", grounded);
        playerAnim.SetBool("isJump", false);
    }

    
    public void KeyInput()
    {
        if (pBState != PlayerBattleState.Attack && pBState != PlayerBattleState.Farrying)
        {
            if (Input.GetButton("Horizontal"))
            {
                plMove = PlayerMoveState.Move;

                inputX = Input.GetAxisRaw("Horizontal");
                moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;

            }
            else
                plMove = PlayerMoveState.Idle;

            if (Input.GetKeyDown(KeyCode.Space) && grounded)
            {
                jumpInput = true;
                plMove = PlayerMoveState.Jump;
            }
            else
                jumpInput = false;
        }
    }

    void PlayerMove()
    {
        transform.position = new Vector2(moveX, transform.position.y);

        if (jumpInput && grounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private void PlayerMoveAnimUpdate(PlayerMoveState plMove)
    {
        switch (plMove)
        {
            case PlayerMoveState.Idle:
                playerAnim.SetBool("isMove", false);
                break;
            case PlayerMoveState.Move:
                playerAnim.SetBool("isMove", true);
                break;
            case PlayerMoveState.Jump:
                playerAnim.SetTrigger("isJump");
                break;
            default:
                break;
        }
    }

}
