using UnityEngine;

public class BoundaryBehaviour : MonoBehaviour
{
    [SerializeField] private float healthPenalty = 5f;
    [SerializeField] private bool isInPlayArea;

    PlayerControl player;

    void Start()
    {
        player = PlayerControl.Instance;
        isInPlayArea = true;
    }

    void Update()
    {
        if (!isInPlayArea)
        {
            float reduceHealth = healthPenalty * Time.deltaTime;
            player.ModifyHealth(-reduceHealth);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            isInPlayArea = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isInPlayArea = true;
    }
}
