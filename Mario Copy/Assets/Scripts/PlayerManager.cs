using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private WallJumpScript _wallJumpScript;

    //Movement
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool IsFacingRight = true;
    private Rigidbody2D rb;

    //Attack
    [Header("Attack")]
    [SerializeField] private float attackDamage;
    [SerializeField] private LayerMask enemyLayer;

    //Health
    [Header("Health")]
    [SerializeField] private float playerHealth;
    [HideInInspector] public float currentHealth;

    //KnockBackByEnemy
    [Header("Knockback By Enemy")]
    [SerializeField] private float knockbackForce;

    //Buff
    [Header("Buff Stats")]
    [SerializeField] private float buffTimer = 10f;
    [HideInInspector] public bool isBuff;

    [Header("Menus")]
    [SerializeField] private GameObject deathMenu;
    [SerializeField] private GameObject winMenu;
    bool canWin = true;

    void Awake()
    {
        currentHealth = playerHealth;
        deathMenu.SetActive(false);
        winMenu.SetActive(false);
    }

    void Start()
    {
        _wallJumpScript = GetComponent<WallJumpScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        PlayerMovement();
        Jump();
        

        if(currentHealth <= 0f)
            Die();

        // Check if the player is grounded
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);        

        playerHealth = currentHealth;

        if(rb.velocity.x > 0.35f && !IsFacingRight || rb.velocity.x < -0.35f && IsFacingRight)
        {
            Flip();
        }

        currentHealth = Mathf.Clamp(currentHealth, 0, 100);
    }

    void Jump()
    {
        if(isGrounded)
            rb.gravityScale = 1f;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            FindObjectOfType<AudioManager>().Play("Jump");
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        
        if(Input.GetButtonUp("Jump"))
        {
            rb.gravityScale = 3f;
        }
        
        if(!isGrounded && !Input.GetButtonDown("Jump"))
        {
            rb.gravityScale += Time.deltaTime;
            rb.gravityScale = Mathf.Clamp(rb.gravityScale, 1, 3.5f);
        }
    }

    void PlayerMovement()
    {
        if(!_wallJumpScript.isWallJumping)
        {
            // Player movement
            float horizontalInput = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

            rb.velocity = movement;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && IsEnemyBeneath())
        {
            AttackEnemy(collision);
            KnockBackByEnemy();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(Grow(collision));
        if(collision.CompareTag("DeathCollider"))
        {
            Die();
        }

        if(collision.CompareTag("Pole") && canWin)
            StartCoroutine(Win());
    }

    public bool IsEnemyBeneath()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, enemyLayer);
    }

    void AttackEnemy(Collision2D collision)
    {
        GameObject currEnemy = collision.gameObject;
        currEnemy.GetComponent<EnemyManager>().currentHealth -= attackDamage;
    }

    void KnockBackByEnemy()
    {
        FindObjectOfType<AudioManager>().Play("Bump Enemy");

        Vector2 dir = Vector2.up;
        rb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }

    public void Flip()
    {
        Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;

		IsFacingRight = !IsFacingRight;
    }

    IEnumerator Grow(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("GrowthMushroom") && !isBuff)
        {
            FindObjectOfType<AudioManager>().Play("Power Up");

            Vector2 scale = transform.localScale;
            scale *= 1.5f;
            transform.localScale = scale;

            jumpForce *= 1.5f;
            moveSpeed *= 1.1f;
            attackDamage = 100f;
            currentHealth += 50;
        
            isBuff = true;
            
            yield return new WaitForSeconds(buffTimer);

            if(IsFacingRight)
            {
                scale = new Vector2(1, 1);
            }
            else
            {
                scale = new Vector2(-1, 1);
            }
            transform.localScale = scale;

            jumpForce = 13f;
            moveSpeed = 7.5f;
            attackDamage = 25f;

            isBuff = false;

        }

    }

    private void Die()
    {
        FindObjectOfType<AudioManager>().Play("Mario Die");
        gameObject.SetActive(false);
        deathMenu.SetActive(true);
    }

    private IEnumerator Win()
    {
        canWin = false;
        FindObjectOfType<AudioManager>().Play("Stage Clear");
        yield return new WaitForSeconds(5f);
        winMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
