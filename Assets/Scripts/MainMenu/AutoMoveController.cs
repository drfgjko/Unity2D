using UnityEngine;

public class AutoMoveController : MonoBehaviour
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponent<Animator>();

    [Header("Collison check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded = false;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float edgeCheckDistance = 1f;
    private bool facingRight = true;

    void Update()
    {
        AnimationControllers();
        CollisionChecks();
        AutoMove();
        FlipControllers();
    }


    private void AnimationControllers()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
    private void FlipControllers()
    {
        if (isGrounded)
        {
            Vector2 direction = facingRight ? Vector2.right : Vector2.left;
            Vector2 rayOrigin = (Vector2)transform.position + (facingRight ? Vector2.right : Vector2.left) * 0.5f;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, edgeCheckDistance, whatIsGround);
            if (hit.collider != null) Filp();
/*            Debug.DrawRay(rayOrigin, direction * edgeCheckDistance, Color.red);
*/
        }
    }
    private void Filp()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void AutoMove()
    {
        if (isGrounded)
        {
            float xInput = facingRight ? 1 : -1;
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
        }
    }
    private void CollisionChecks()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Emoji")
        {
            Destroy(collision.gameObject);
            MainMenuSoundManager.Instance.PlayExitClipSound();
        }
    }
/*    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }*/
}
