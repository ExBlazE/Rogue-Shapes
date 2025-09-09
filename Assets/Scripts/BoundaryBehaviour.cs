using UnityEngine;

public class BoundaryBehaviour : MonoBehaviour
{
    [SerializeField] private float healthPenalty = 5f;
    [SerializeField] private bool isInPlayArea;

    PlayerControl player;

    void Start()
    {
        // Get singleton reference to player
        player = PlayerControl.Instance;
        isInPlayArea = true;
    }

    void Update()
    {
        // If player not in play area, reduce health
        if (!isInPlayArea)
        {
            float reduceHealth = healthPenalty * Time.deltaTime;
            player.ModifyHealth(-reduceHealth);
        }
    }

    // Detect when player leaves play area
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInPlayArea = false;
    }

    // Detect when player enters play area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInPlayArea = true;
    }
}
