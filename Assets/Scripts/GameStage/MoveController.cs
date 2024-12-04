using UnityEngine;

public class MoveController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float xInput;

    [Header("Collison check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool facingRight = true;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        AnimationControllers();
        FlipControllers();
        CollisionChecks();
        xInput = Input.GetAxisRaw("Horizontal");
        Movement();
        if (Input.GetKeyDown(KeyCode.Space)) Jump();
    }

    private void FlipControllers()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x < transform.position.x && facingRight) Filp();
        else if (mousePos.x > transform.position.x && !facingRight) Filp();
    }

    private void Filp()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AnimationControllers()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            GameBgmManager.Instance.PlayJumpSound();
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    /*    private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
        }*/

}
