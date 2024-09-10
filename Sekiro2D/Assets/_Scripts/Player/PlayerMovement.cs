using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Falling,
    WallSlideRight,
    WallSlideLeft
}

public class PlayerMovement : MonoBehaviour
{
    PlayerBattle pB;
    ObjectAudio pAudio;
    public AudioSource pAudioSource;

    // 플레이어 상태
    [SerializeField] PlayerState pState;
    public PlayerState PState { get { return pState; } set { pState = value; } }
    private PlayerState lastPlState;
    private PlayerBattleState pBState;

    [Space(10)]
    public bool isWallSlide;
    private bool isWallJump;

    // 움직임
    [Space(10)]
    public float jumpForce;
    public float wallJumpForce;
    public float wallJumpTime;
    private Rigidbody2D rb;
    private bool zeroVelocity;

    public float moveX;
    public float moveSpeed;
    public float guardSpeed;

    public float inputX;

    public float slideSpeed;
    IEnumerator falseWallJump;

    #region 지형 체크

    [Space(10)]
    [Tooltip("감지할 Ground 레이어")] public LayerMask grdLayer;
    public Transform grdCheckPoint;
    public float grdCheckSize;

    [Space(10)]
    [Tooltip("감지할 Wall 레이어")] public LayerMask wallLayer;
    public float wallSensorSize;

    // 오른쪽 센서
    [Space(10)]
    public Transform sensorTopRight;
    public Transform sensorLowRight;
    private bool rTopWall;
    private bool rTopGround;
    private bool rLowWall;
    private bool rLowGround;

    // 왼쪽 센서
    [Space(10)]
    public Transform sensorTopLeft;
    public Transform sensorLowLeft;
    private bool lTopWall;
    private bool lTopGround;
    private bool lLowWall;
    private bool lLowGround;

    private bool ground;
    public bool Ground { get { return ground; } }

    #endregion

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    private void Start()
    {
        pAudio = GetComponent<ObjectAudio>();

        pB = GetComponent<PlayerBattle>();
        if (pB == null)
            pB = GetComponentInParent<PlayerBattle>();

        rb = GetComponent<Rigidbody2D>();

        moveX = transform.position.x;
    }

    private void Update()
    {
        if (pBState != PlayerBattleState.Die)
        {
            pBState = pB.PBState;

            StateUpdate();

            HandleInput();

            if (lastPlState != pState)
                lastPlState = pState;
        }
    }

    private void FixedUpdate()
    {
        CheckUpdate();
        CharacterVelocity();
    }

/*-----------------------------------------------------------------------------------------------------------------------------------*/

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWallSlide)
                WallJump(); // 벽 점프 처리
            else if (ground)
                Jump(); // 일반 점프 처리
            pB.SetStateIdle();
        }
        // 방향키 입력 처리
        inputX = Input.GetAxisRaw("Horizontal");
    }

    public void CharacterVelocity()
    {
        if (zeroVelocity)
            rb.velocity = Vector3.zero;

        MovePosX();
    }

    void StateUpdate()
    {
        if (pBState == PlayerBattleState.Attack)
            zeroVelocity = true;
        else
            zeroVelocity = false;

        if (pBState != PlayerBattleState.Hit)
        {
            if (ground)
            {
                // 입력이 있고 벽점프, 피격상태 가 아닐시
                if (inputX != 0 && !isWallJump)
                    pState = PlayerState.Move;

                // 땅에있고 가드, 피격 상태가 아닐시
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    pState = PlayerState.Idle;
                }
            }
        }
    }

    // 움직임
    void MovePosX()
    {
        moveX = inputX * moveSpeed;

        BattleMoveSpeed();
        WallSlideSpeed();
        LimitCheck();

        // 벽점프 중 이 아니고 입력이 있으며 넉백 상태가 아닐때
        if (!isWallJump && inputX != 0 && !pB.isKnockBack)
            rb.velocity = new Vector2(moveX, rb.velocity.y);
    }

    void BattleMoveSpeed()
    {
        if (pBState == PlayerBattleState.Attack)
        {
            moveX = 0;
        }
        else if (pBState == PlayerBattleState.Guard &&
            !pB.isKnockBack && ground)
        {
            moveX = inputX * guardSpeed;
        }
    }

    /// <summary>
    /// 물리 관련 업데이트
    /// </summary>
    void CheckUpdate()
    {
        UpdateSensors();

        WallSlideCheck();

        CheckFallingState();
        CheckWallSlide();
    }

    #region 상태 감지 관련

    void CheckFallingState()
    {
        // 벽슬라이드나 점프 상태가 아닌데도 공중에 있을때
        if (!ground && pState != PlayerState.Jump &&
            !(pState == PlayerState.WallSlideLeft || pState == PlayerState.WallSlideRight))
        {
            isWallSlide = false;
            pState = PlayerState.Falling;
        }
    }

    void CheckWallSlide()
    {
        if (pState == PlayerState.WallSlideRight || pState == PlayerState.WallSlideLeft)
            isWallSlide = true;
        else
            isWallSlide = false;
    }
    
    #endregion

    #region 점프 관련
    
    void Jump()
    {
        pAudio.ChangeSound(pAudioSource, AudioState.JumpSound);

        pState = PlayerState.Jump;
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    void WallJump()
    {
        pAudio.ChangeSound(pAudioSource, AudioState.JumpSound);

        isWallJump = true;

        // 설정 시간뒤 병렬로 함수 실행
        if (falseWallJump != null)
            StopCoroutine(falseWallJump);

        falseWallJump = FalseWallJump(wallJumpTime);

        StartCoroutine(falseWallJump);

        Vector2 dir = pState == PlayerState.WallSlideRight ? Vector2.left : Vector2.right;

        Vector2 wallJump = new Vector2(dir.x * wallJumpForce, 0.9f * jumpForce);
        rb.velocity = wallJump;

        isWallSlide = false;
        pState = PlayerState.Jump;
    }

    // 벽점프 거짓으로 만드는 코루틴
    IEnumerator FalseWallJump(float wallJumpTime)
    {
        yield return new WaitForSeconds(wallJumpTime);
        isWallJump = false;
    }

    #endregion

    #region 벽 감지 관련 메소드

    /// <summary>
    /// 땅에 닿지 않고 벽에 붙을때 상태 변경
    /// </summary>
    private void WallSlideCheck()
    {
        if (!ground)
        {
            if (rTopWall)
            { 
                pState = PlayerState.WallSlideRight;
                pB.SetStateIdle();
            }
            else if (lTopWall)
            { 
                pState = PlayerState.WallSlideLeft;
                pB.SetStateIdle();
            }
            else
                pState = PlayerState.Falling;
        }
    }

    /// <summary>
    /// Wall or Ground 레이어인 벽 닿을시 이동제한
    /// </summary>
    void LimitCheck()
    {
        // 일반 벽 센서
        if ((inputX > 0 && (rTopWall || rLowWall)) || (inputX < 0 && (lTopWall || lLowWall)))
            moveX = 0f;

        // 바닥벽 센서
        else if ((inputX > 0 && (rTopGround || rLowGround)) || (inputX < 0 && (lTopGround || lLowGround)))
            moveX = 0f;
    }

    // 벽에 닿았을시 속도 감소
    private void WallSlideSpeed()
    {
        if (isWallSlide && !isWallJump)
        {
            float slowY = rb.velocity.y * slideSpeed;
            rb.velocity = new Vector2(rb.velocity.x, slowY);
        }
    }

    /// <summary>
    /// 각종 센서들 확인
    /// </summary>
    public void UpdateSensors()
    {
        // 바닥 감지
        ground = CheckSensor(ground, grdCheckPoint, grdCheckSize, grdLayer);

        #region 벽감지
        rTopGround = CheckSensor(rTopGround, sensorTopRight, wallSensorSize, grdLayer);
        rLowGround = CheckSensor(rLowGround, sensorLowRight, wallSensorSize, grdLayer);

        lTopGround = CheckSensor(lTopGround, sensorTopLeft, wallSensorSize, grdLayer);
        lLowGround = CheckSensor(lLowGround, sensorLowLeft, wallSensorSize,grdLayer);

        rTopWall = CheckSensor(rTopWall, sensorTopRight, wallSensorSize, wallLayer);
        rLowWall = CheckSensor(rLowWall, sensorLowRight, wallSensorSize, wallLayer);

        lTopWall = CheckSensor(lTopWall, sensorTopLeft, wallSensorSize, wallLayer);
        lLowWall = CheckSensor(lLowWall, sensorLowLeft, wallSensorSize, wallLayer);
        #endregion
    }

    /// <summary>
    /// 센서에 감지할 레이어가 닿았는지 확인후 결과값 리턴
    /// </summary>
    /// <param name="sensor">센서 닿았는지 확인할 변수</param>
    /// <param name="sensePos">센서의 위치</param>
    /// <param name="layer">감지할 레이어</param>
    /// <returns></returns>
    bool CheckSensor(bool sensor, Transform sensePos, float wallSensorSize, LayerMask layer)
    {
        sensor = Physics2D.OverlapCircle(sensePos.position, wallSensorSize, layer);
        return sensor;
    }

    #endregion

    #region 센서 범위 확인용

    private void OnDrawGizmos()
    {
        DrawGizmo(sensorTopRight, wallSensorSize);
        DrawGizmo(sensorLowRight, wallSensorSize);
        DrawGizmo(sensorTopLeft, wallSensorSize);
        DrawGizmo(sensorLowLeft, wallSensorSize);

        DrawGizmo(grdCheckPoint, grdCheckSize);
    }
    void DrawGizmo(Transform point, float wallSensorSize)
    {
        Gizmos.DrawWireSphere(point.position, wallSensorSize);
    }

    #endregion
}
