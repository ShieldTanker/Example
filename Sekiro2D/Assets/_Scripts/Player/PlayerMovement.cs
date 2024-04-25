using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Animator playerAnim;

    // 움직임
    public float jumpForce;
    public float moveSpeed;
    Rigidbody2D rb;
    
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
        PlayerMove();
        PlayerJump(grounded);
        PlayerActionSignal();
    }

    void PlayerActionSignal()
    {
        if (Input.GetButton("Horizontal"))
        {
            playerAnim.SetBool("isMove", true);
        }
        else
            playerAnim.SetBool("isMove", false);
    }

    void PlayerMove()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float moveX = transform.position.x + inputX * moveSpeed * Time.deltaTime;
        transform.position = new Vector2(moveX, transform.position.y);
    }

    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(grdCheckPoint.position, 0.1f, grdCheckLayerMask);
        playerAnim.SetBool("isGround", grounded);
    }

    void PlayerJump(bool grounded)
    {
        if (Input.GetKeyDown(KeyCode.Space) &&  grounded)
        {
            rb.AddForce(Vector3.up * jumpForce , ForceMode2D.Impulse);
        }
    }
}
