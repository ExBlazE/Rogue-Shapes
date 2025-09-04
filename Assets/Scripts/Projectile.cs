using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 15f;
    [SerializeField]
    private float duration = 3f;

    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        if (duration > 0)
        {
            duration -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
