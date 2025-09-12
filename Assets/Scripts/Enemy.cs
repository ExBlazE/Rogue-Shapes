using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed = 2.5f;

    [Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private bool canShoot = true;
    [SerializeField] private float shotRange = 15f;
    [SerializeField] private float shotCooldown = 2.5f;
    [SerializeField] private float collisionDamage = 20f;

    private float shotRangeSqr;
    private float currentCooldown = 0f;

    [Space]
    [SerializeField] private Animator enemyAnim;

    [Space]
    [SerializeField] private ParticleSystem playerCollideFX;
    [SerializeField] private ParticleSystem shieldCollideFX;

    private Rigidbody enemyRb;

    private GameManager gameManager;
    private PlayerControl player;
    
    void Awake()
    {
        // Get reference to rigidbody and animator components
        enemyRb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // Get reference to player script via singleton
        player = PlayerControl.Instance;

        // Get reference to game manager via singleton
        gameManager = GameManager.Instance;

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
                Vector3 spawnPos = focus.transform.position;
                Quaternion spawnRot = transform.rotation;
                Transform spawnParent = gameManager.projectileGroupObject;

                Instantiate(projectilePrefab, spawnPos, spawnRot, spawnParent);
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

        // Set movement animation parameters
        enemyAnim.SetFloat("f_speed", enemyRb.linearVelocity.magnitude);
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
            Instantiate(playerCollideFX, focus.transform.position, transform.rotation, gameManager.particlesGroupObject);

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
            Instantiate(shieldCollideFX, focus.transform.position, transform.rotation, gameManager.particlesGroupObject);

            player.ModifyShield(-collisionDamage);
            Destroy(gameObject);
        }
    }
}
