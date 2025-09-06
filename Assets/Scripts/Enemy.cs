using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotateSpeed = 180f;

    [Space]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotRange = 15f;
    private float shotRangeSqr;

    [SerializeField] private float shotCooldown = 2.5f;
    private float currentCooldown = 0f;

    void Start()
    {
        // Calculate square of shot range
        shotRangeSqr = shotRange * shotRange;
    }

    void Update()
    {
        // Get player's current position via singleton
        Vector3 playerPosition = PlayerControl.Instance.transform.position;

        MoveTowardsPlayer(playerPosition);

        // Calculate distance-squared to player
        float distanceToPlayerSqr = (playerPosition - transform.position).sqrMagnitude;

        // If within range and cooldown is zero, shoot projectile
        // We use square of both numbers because Vector3.magnitude takes more time for CPU
        if (distanceToPlayerSqr < shotRangeSqr)
        {
            if (currentCooldown == 0)
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

    // Method to move enemy towards player's position
    void MoveTowardsPlayer(Vector3 playerPosition)
    {
        // Get direction of player from enemy position
        Vector3 direction = playerPosition - transform.position;

        // Create new rotation facing player and apply it to enemy
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        // Move enemy towards player
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
