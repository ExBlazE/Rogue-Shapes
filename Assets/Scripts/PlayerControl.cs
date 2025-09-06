using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] private GameObject orbFocus;
    [SerializeField] private GameObject orbObject;
    [SerializeField] private LayerMask targetLayers;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.0f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotCooldown = 0.3f;
    private float currentCooldown = 0f;

    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;

    public static PlayerControl Instance;

    void Awake()
    {
        // Singleton reference to this script
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Set max frame rate to avoid errors in editor
        Application.targetFrameRate = 144;        
    }

    void Start()
    {
        // Disable shield by default at start
        shieldObject.SetActive(false);
    }

    void Update()
    {
        // Move player and rotate orb
        Move();
        MoveOrb();
        
        // Shoot projectile on left-click and when cooldown is zero
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentCooldown == 0)
        {
            Instantiate(projectilePrefab, orbObject.transform.position, orbFocus.transform.rotation);
            currentCooldown = shotCooldown;
        }

        // Reduce cooldown to zero
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
                currentCooldown = 0;
        }

        // Enable shield when right-click is held down
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!shieldObject.activeInHierarchy)
                shieldObject.SetActive(true);
        }

        // Disable shield when right-click is released
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (shieldObject.activeInHierarchy)
                shieldObject.SetActive(false);
        }
    }

    // Method to move player
    void Move()
    {
        // Get player input via WASD
        float verticalMove = Input.GetAxis("Vertical");
        float horizontalMove = Input.GetAxis("Horizontal");

        // Create direction vector based on player input
        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);

        // Clamp direction vector magnitude at 1 to prevent faster diagonal speed
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        // Move player
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    // Method to spin orb around player to always point at mouse position
    void MoveOrb()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayers))
        {
            // Get mouse position on ground level and direction from player
            Vector3 mousePosition = hit.point;
            Vector3 mouseDirection = mousePosition - orbFocus.transform.position;

            // Create new rotation facing mouse direction and apply it to orbFocus
            Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);
            orbFocus.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }
}
