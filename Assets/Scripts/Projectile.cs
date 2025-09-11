using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float duration = 3f;

    [Space]
    [SerializeField] private float enemyShotDamage = 10;

    [Space]
    [SerializeField] private ParticleSystem playerHitFX;
    [SerializeField] private ParticleSystem shieldHitFX;
    [SerializeField] private ParticleSystem enemyHitFX;

    private PlayerControl player;
    private GameManager gameManager;

    void Start()
    {
        // Get reference to singleton player script
        player = PlayerControl.Instance;

        // Get reference to singleton GameManager script
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        // Move projectile forward at constant speed
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        // Destroy projectile after set duration
        if (duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Logic for player projectiles hitting enemies
        if (gameObject.CompareTag("Shot_Player") && other.CompareTag("Enemy"))
        {
            Instantiate(playerHitFX, transform.position, Quaternion.identity, gameManager.particlesGroupObject);
            gameManager.AddScore(1);

            Destroy(other.gameObject);
            Destroy(gameObject);

            gameManager.enemiesOnScreen--;
        }

        // Logic for enemy projectiles
        else if (gameObject.CompareTag("Shot_Enemy"))
        {
            // Logic for hitting shield
            if (other.CompareTag("Shield"))
            {
                Instantiate(shieldHitFX, transform.position, Quaternion.identity, gameManager.particlesGroupObject);

                player.ModifyShield(-enemyShotDamage);
                Destroy(gameObject);
            }

            //Logic for hitting player
            else if (other.CompareTag("Player"))
            {
                Instantiate(enemyHitFX, transform.position, Quaternion.identity, gameManager.particlesGroupObject);

                player.ModifyHealth(-enemyShotDamage);
                Destroy(gameObject);
            }
        }
    }
}
