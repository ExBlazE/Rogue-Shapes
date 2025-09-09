using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotateSpeed = 180f;

    [Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private float shotRange = 15f;
    [SerializeField] private float shotCooldown = 2.5f;
    [SerializeField] private float collisionDamage = 20f;

    private float shotRangeSqr;
    private float currentCooldown = 0f;

    private Rigidbody enemyRb;
    private PlayerControl player;

    void Awake()
    {
        player = PlayerControl.Instance;
        enemyRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Calculate square of shot range
        shotRangeSqr = shotRange * shotRange;
    }

    void Update()
    {
        // Calculate distance-squared to player
        float distanceToPlayerSqr = (player.transform.position - transform.position).sqrMagnitude;

        // If within range and cooldown is zero, shoot projectile
        // We use square of both numbers because Vector3.magnitude takes more time for CPU
        if (distanceToPlayerSqr < shotRangeSqr)
        {
            if (currentCooldown == 0 && canShoot)
            {
                Instantiate(projectilePrefab, focus.transform.position, transform.rotation);
                currentCooldown = shotCooldown;
            }
        }

        // Reduce cooldown to zero
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
                currentCooldown = 0;
        }

        // Temp code to rotate mesh for visual effect
        focus.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime), Space.World);
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();
    }

    // Method to move enemy towards player's position
    void MoveTowardsPlayer()
    {
        // If movement is enabled in inspector
        if (canMove)
        {
            // Get direction of player from enemy position
            Vector3 direction = player.transform.position - enemyRb.position;

            // Create new rotation facing player and apply it to enemy
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

            // Move enemy towards player (non-physics method)
            // transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Move enemy towards player (physics method)
            enemyRb.linearVelocity = enemyRb.transform.forward * moveSpeed;
        }

        // If movement is disabled in inspector
        else
        {
            enemyRb.linearVelocity = Vector3.zero;
        }
    }

    // Logic to handle enemy touching player
    void OnCollisionEnter(Collision collision)
    {
        // If touching player, deplete health
        if (collision.gameObject.CompareTag("Player"))
        {
            player.ModifyHealth(-collisionDamage);
            Destroy(gameObject);
        }
    }

    // Logic to handle enemy touching shield
    void OnTriggerEnter(Collider other)
    {
        // If touching shield, deplete shield
        if (other.CompareTag("Shield"))
        {
            player.ModifyShield(-collisionDamage);
            Destroy(gameObject);
        }
    }
}
