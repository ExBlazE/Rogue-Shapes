using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float duration = 3f;

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
            Destroy(other.gameObject);
            Destroy(gameObject);
        }

        else if (gameObject.CompareTag("Shot_Enemy"))
        {
            // Logic for enemy projectiles hitting shield
            if (other.CompareTag("Shield"))
            {
                Destroy(gameObject);
            }

            //Logic for enemy projectiles hitting player
            else if (other.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
        }
    }
}
