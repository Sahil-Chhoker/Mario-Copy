using UnityEngine;

public class MushroomFollowScript : MonoBehaviour
{
    private PlayerManager _playerManagerScript;

    //Follow Player
    [Header("Follow Player")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 5f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerManagerScript = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            MoveTowardsPlayer();
        }

    }

    void MoveTowardsPlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
