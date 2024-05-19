using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerMovement : MonoBehaviour
{

    // �÷��̾� ����
    public PlayerState plState;
    PlayerState lastPlState;
    public PlayerBattleState plBattleState;
    public bool isWall;

    // ������
    Rigidbody2D rb;
    public float jumpForce;
    public float wallJumpForce;

    public float moveSpeed;
    private float moveX;
    public static float inputX;

    public float fallingSpeed;

    // ���� üũ
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    public float grdCheckSize;
    private static bool ground;

    // ������ ����
    public Transform senseTopRight;
    public static bool rTopWall;
    public Transform senseLowRight;
    public static bool rLowWall;

    // ���� ����
    public Transform senseTopLeft;
    public static bool lTopWall;
    public Transform senseLowLeft;
    public static bool lLowWall;

    public LayerMask wallLayer;
    public float wallSensorSize;


    // ������ �Ӽ�
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

            GroundCheck();
            WallCheck();

            KeyInput();

            if (lastPlState != plState)
                lastPlState = plState;
        }
    }

    public void KeyInput()
    {
        if (plBattleState != PlayerBattleState.Attack &&
            plBattleState != PlayerBattleState.Farrying)
        {
            MovePosX();
            // ���� �������
            LimitCheck();

            if (Input.GetButton("Horizontal"))
            {
                // transform.position = new Vector2(transform.position.x + moveX, transform.position.y);

                rb.velocity = new Vector2(moveX, rb.velocity.y);

                if (ground)
                {
                    PlayerManager.PManager.PlState = PlayerState.Move;
                }
            }
            else
            {
                rb.velocity = new Vector2(0,rb.velocity.y);
                PlayerManager.PManager.PlState = PlayerState.Idle;
            }

            // ���� ����
            if (Input.GetKeyDown(KeyCode.Space) && ground)
            {
                PlayerManager.PManager.PlState = PlayerState.Jump;

                rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            }

            // ������ �� �����̵�
            else if (plState == PlayerState.WallSlideRight)
            {
                isWall = true;
                WallSlideSpeed();
                WallJump(Vector2.left);
            }
            else if (plState == PlayerState.WallSlideLeft)
            {
                isWall = true;
                WallSlideSpeed();
                WallJump(Vector2.right);
            }
            else if (!ground)
            {
                isWall = false;
                PlayerManager.PManager.PlState = PlayerState.Falling;
            }
        }
    }


    void WallJump(Vector2 wayVec)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerManager.PManager.PlState = PlayerState.Jump;

            Vector2 wallJump = (Vector2.up + wayVec) * jumpForce;
            wallJump.Normalize();

            rb.AddForce(wallJump * wallJumpForce, ForceMode2D.Impulse);
        }
    }

    // ���� ������� �ӵ� ����
    private void WallSlideSpeed()
    {
        if (isWall)
        {
            float slowY = rb.velocity.y * fallingSpeed;
            rb.velocity = new Vector2(rb.velocity.x, slowY);
        }
    }

    // ������
    void MovePosX()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        moveX = inputX * moveSpeed;
    }

    // ���� üũ �޼ҵ��
    void LimitCheck()
    {
        if ((inputX > 0 && (rTopWall || rLowWall)) ||
            (inputX < 0 && (lTopWall || lLowWall)))
            moveX = 0f;
    }
    public void GroundCheck()
    {
        ground = Physics2D.OverlapCircle(
            grdCheckPoint.position, grdCheckSize, grdCheckLayerMask);
    }
    bool CheckPosition(bool wallCheck, Transform senssPos)
    {
        wallCheck = Physics2D.OverlapCircle(senssPos.position, wallSensorSize, wallLayer);
        return wallCheck;
    }
    public void WallCheck()
    {
        rTopWall= CheckPosition(rTopWall,senseTopRight);
        rLowWall = CheckPosition(rLowWall, senseLowRight);
        lTopWall = CheckPosition(lTopWall, senseTopLeft);
        lLowWall = CheckPosition(lLowWall, senseLowLeft);
    }
    
    // ���� ���� �޼ҵ�
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
    // ���� Ȯ�ο�
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
