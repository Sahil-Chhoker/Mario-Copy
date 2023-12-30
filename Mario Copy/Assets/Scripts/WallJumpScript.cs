using UnityEngine;

public class WallJumpScript : MonoBehaviour
{
    private PlayerManager _playerManagerScript;
    private Rigidbody2D playerRb;

    [Header("Wall Jump")]
    [SerializeField] private float wallJumpForce = 10f;
    [SerializeField] private float wallJumpTime = 0.5f;
    [SerializeField] private float wallJumpControlDelay = 0.2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    [HideInInspector] public bool isWallJumping;
    private bool isWallSliding;
    private float wallJumpDirection;
    private float wallJumpTimeCounter;

    void Start()
    {
        GameObject player = this.gameObject;
        _playerManagerScript = player.GetComponent<PlayerManager>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        WallJump();
        WallSlide();
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !_playerManagerScript.isGrounded)
        {
            isWallSliding = true;
            playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Clamp(playerRb.velocity.y, -wallJumpForce, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimeCounter = wallJumpTime;

            // Reset control delay when touching the wall
            if (!isWallJumping)
            {
                CancelInvoke(nameof(StopWallJumping));
                Invoke(nameof(StopWallJumping), wallJumpControlDelay);
            }
        }
        else
        {
            wallJumpTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpTimeCounter > 0f)
        {
            isWallJumping = true;
            playerRb.velocity = new Vector2(wallJumpDirection * wallJumpForce, wallJumpForce);
            
            Invoke(nameof(StopWallJumping), wallJumpControlDelay);       
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
