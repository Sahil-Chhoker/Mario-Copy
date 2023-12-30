using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private PlayerManager _playerManagerScript;
    Rigidbody2D rb;

    //Follow Player
    [Header("Follow Player")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float stopDistance;
    private Transform player;

    //Attack
    [Header("Attack")]
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackForce;
    [SerializeField] private float attackInterval = 3f;
    [SerializeField] private float damageRadius = 1.5f;
    private float timeSinceLastAttack;

    //Health
    [Header("Health")]
    [SerializeField] private float enemyHealth;
    [HideInInspector] public float currentHealth;

    //BlockDetector
    [Header("Block Detecter")]
    private RaycastHit2D hit1;
    private RaycastHit2D hit2;
    [SerializeField] private float distance;
    [SerializeField] private float jumpForce;
    [SerializeField] LayerMask groundLayer;



    void Awake()
    {   
        //Initialize the enemy health
        currentHealth = enemyHealth;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerManagerScript = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
        Die();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange && distanceToPlayer > stopDistance)
        {
            // Move towards the player
            MoveTowardsPlayer();

            // If the player is close, attack every attackInterval seconds
            if (distanceToPlayer < damageRadius && Time.time - timeSinceLastAttack > attackInterval)
            {
                DealDamageToPlayer();
                timeSinceLastAttack = Time.time;
            }
        }

        enemyHealth = currentHealth;

        hit1 = Physics2D.Raycast(transform.position, Vector2.right, distance, groundLayer);
        hit2 = Physics2D.Raycast(transform.position, -Vector2.right, distance, groundLayer);
        if(hit1.collider != null || hit2.collider != null)
        {
            rb.AddForce(jumpForce * Time.deltaTime * Vector2.up);
        }
    }

    void MoveTowardsPlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void DealDamageToPlayer()
    {
        int randomNumber = Random.Range(0, 2);

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, damageRadius);

        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player") && !_playerManagerScript.IsEnemyBeneath())
            {
                // Deal damage to the player
                PlayerManager playerManager = hitCollider.GetComponent<PlayerManager>();
                if (playerManager != null)
                {
                    if(randomNumber == 0)
                        FindObjectOfType<AudioManager>().Play("Hurt1");
                    else
                        FindObjectOfType<AudioManager>().Play("Hurt2");

                    playerManager.currentHealth -= attackDamage;
                }
            }
        }
    }

    void Die()
    {
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}