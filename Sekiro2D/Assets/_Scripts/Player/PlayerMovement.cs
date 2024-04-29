using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator playerAnim;

    // 움직임
    Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    private float moveX;
    private bool jumpInput;
    
    // 지형 체크
    public LayerMask grdCheckLayerMask;
    public Transform grdCheckPoint;
    private bool grounded;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        GroundCheck();
        KeyInput();
        PlayerMove();
    }

    void PlayerMove()
    {
        transform.position = new Vector2(moveX, transform.position.y);

        if (jumpInput && grounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(grdCheckPoint.position, 0.1f, grdCheckLayerMask);
        playerAnim.SetBool("isGround", grounded);
        playerAnim.SetBool("isJump", false);
    }

    
    public void KeyInput()
    {
        if (Input.GetButton("Horizontal"))
        {
            playerAnim.SetBool("isMove", true);

            float inputX = Input.GetAxisRaw("Horizontal");
            moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;
        }
        else
            playerAnim.SetBool("isMove", false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpInput = true;
            playerAnim.SetBool("isJump", jumpInput);
        }
        else
            jumpInput = false;
    }
}
