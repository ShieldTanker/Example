using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    // 플레이어 상태
    PlayerState plState;
    PlayerState lastPlState;
    PlayerBattleState plBattleState;
    bool isWallJump;
    public bool isWallSlide;

    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpTime;

    public float moveSpeed;
    public float moveX;
    public static float inputX;

    public float fallingSpeed;

    // 지형 체크
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    public float grdCheckSize;
    private static bool ground;

    // 오른쪽 센서
    public Transform senseTopRight;
    public static bool rTopWall;
    public static bool rTopGround;
    public Transform senseLowRight;
    public static bool rLowWall;
    public static bool rLowGround;

    // 왼쪽 센서
    public Transform senseTopLeft;
    public static bool lTopWall;
    public static bool lTopGround;
    public Transform senseLowLeft;
    public static bool lLowWall;
    public static bool lLowGround;

    public LayerMask wallLayer;
    public float wallSensorSize;


    // 센서들 속성
    public static bool RightTopSensor { get { return rTopWall; } }
    public static bool RightLowSensor { get { return rLowWall; } }
    public static bool LeftTopSensor { get { return lTopWall; } }
    public static bool LeftLowSensor { get { return lLowWall; } }


    public static bool Ground { get { return ground;} }

    private void Start()
    {
        StartSetting();
    }

    private void Update()
    {
        if (plBattleState != PlayerBattleState.Die)
        {
            UpdateParameter();

            CheckUpdate();

            KeyInput();

            if (lastPlState != plState)
                lastPlState = plState;
        }
    }

    public void KeyInput()
    {
        if (plBattleState == PlayerBattleState.Attack ||
            plBattleState == PlayerBattleState.Farrying)
            rb.velocity = Vector2.zero;
        else
        {
            MovePosX();
            LimitCheck();

            // 입력이 있을시
            if (Input.GetButton("Horizontal") && plBattleState != PlayerBattleState.Hit)
            {
                if (!isWallJump)
                {
                    rb.velocity = new Vector2(moveX, rb.velocity.y);
                    PlayerManager.PManager.PlState = PlayerState.Move;
                }
            }
            else if(ground &&
                    (plBattleState != PlayerBattleState.Guard &&
                    plBattleState != PlayerBattleState.Hit))
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                PlayerManager.PManager.PlState = PlayerState.Idle;
            }
            // 벽점프
            InputWallJump();
            WallSlideSpeed();

            InputJump();
        }
    }

    // 점프관련
    void InputJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ground)
        {
            PlayerManager.PManager.PlState = PlayerState.Jump;
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (!ground)
        {
            PlayerManager.PManager.PlState = PlayerState.Falling;
        }
    }
    void InputWallJump()
    {
        if (!ground)
        {
            // 오른쪽 벽일때
            if (plState == PlayerState.WallSlideRight)
                WallJump(Vector2.left);

            // 왼쪽 벽일때
            else if (plState == PlayerState.WallSlideLeft)
                WallJump(Vector2.right);
        }
    }
    void WallJump(Vector2 wayVec)
    {
        if (Input.GetKeyDown(KeyCode.Space) && wallLayer != grdCheckLayerMask)
        {
            isWallJump = true;
            Invoke("FalseWallJump", wallJumpTime);

            isWallSlide = false;
            PlayerManager.PManager.PlState = PlayerState.Jump;

            Vector2 wallJump = (Vector2.up + wayVec) * jumpForce;
            wallJump.Normalize();

            rb.velocity = wallJump * wallJumpForce;
        }
    }
    
    // 벽점프 거짓으로 만드는 메소드
    void FalseWallJump()
    {
        isWallJump = false;
    }
    
    // 벽에 닿았을시 속도 감소
    private void WallSlideSpeed()
    {
        if (isWallSlide)
        {
            float slowY = rb.velocity.y * fallingSpeed;
            rb.velocity = new Vector2(rb.velocity.x, slowY);
        }
    }

    // 움직임
    void MovePosX()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        moveX = inputX * moveSpeed;
    }

    void CheckUpdate()
    {
        GroundCheck();
        GroundWallSense();
        WallSenseCheck();

        CheckWallSlide();
        CheckFallingState();

    }

    // 지형 체크 메소드들
    void LimitCheck()
    {
        if ((inputX > 0 && (rTopWall || rLowWall)) ||
            (inputX < 0 && (lTopWall || lLowWall)))
        {
            moveX = 0f;
        }
        else if ((inputX > 0 && (rTopGround || rLowGround)) ||
            (inputX < 0 && (lTopGround || lLowGround)))
        {
            moveX = 0f;
        }
    }
    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, grdCheckSize, grdCheckLayerMask);
    }
    bool CheckWall(bool wallCheck, Transform sensePos, LayerMask layer)
    {
        wallCheck = Physics2D.OverlapCircle(sensePos.position, wallSensorSize, layer);
        return wallCheck;
    }
    public void WallSenseCheck()
    {
        rTopWall= CheckWall(rTopWall,senseTopRight, wallLayer);
        rLowWall = CheckWall(rLowWall, senseLowRight, wallLayer);
        lTopWall = CheckWall(lTopWall, senseTopLeft, wallLayer);
        lLowWall = CheckWall(lLowWall, senseLowLeft, wallLayer);
    }
    public void GroundWallSense()
    {
        rTopGround= CheckWall(rTopGround, senseTopRight, grdCheckLayerMask);
        rLowGround = CheckWall(rLowGround, senseLowRight, grdCheckLayerMask);
        lTopGround = CheckWall(lTopGround, senseTopLeft, grdCheckLayerMask);
        lLowGround = CheckWall(lLowGround, senseLowLeft, grdCheckLayerMask);
    }
    void CheckFallingState()
    {
        if (!ground && plState != PlayerState.Jump &&
            (plState != PlayerState.WallSlideLeft || plState != PlayerState.WallSlideRight))
        {
            isWallSlide = false;
            PlayerManager.PManager.PlState = PlayerState.Falling;
        }
    }
    void CheckWallSlide()
    {
        if (plState == PlayerState.WallSlideRight || plState == PlayerState.WallSlideLeft)
            isWallSlide = true;
        else
            isWallSlide = false;
    }
    
    // 세팅 메소드
    void StartSetting()
    {
        rb = GetComponent<Rigidbody2D>();
        moveX = transform.position.x;
    }
    void UpdateParameter()
    {
        plState = PlayerManager.PManager.PlState;
        plBattleState = PlayerManager.PManager.PlBattleState;
    }

    // 범위 확인용
    private void OnDrawGizmos()
    {
        DrawGizmo(senseTopRight, wallSensorSize);
        DrawGizmo(senseLowRight, wallSensorSize);
        DrawGizmo(senseTopLeft, wallSensorSize);
        DrawGizmo(senseLowLeft, wallSensorSize);

        DrawGizmo(grdCheckPoint, grdCheckSize);
    }
    void DrawGizmo(Transform point, float wallSensorSize)
    {
        Gizmos.DrawWireSphere(point.position, wallSensorSize);
    }
}
