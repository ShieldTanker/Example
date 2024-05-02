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

    public Animator playerAnim;

    // 플레이어 상태
    public PlayerBattleState pBState;
    PlayerMoveState plMove;

    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    private bool jumpInput;
    
    // 지형 체크
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    public static bool grounded;


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
        PlayerMoveUpdate(plMove);
    }

    void PlayerMove()
    {
        transform.position = new Vector2(moveX, transform.position.y);

        if (jumpInput && grounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
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

                float inputX = Input.GetAxisRaw("Horizontal");
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

    private void PlayerMoveUpdate(PlayerMoveState plMove)
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
