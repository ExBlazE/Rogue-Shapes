using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] private GameObject orbFocus;
    [SerializeField] private GameObject orbObject;
    [SerializeField] private LayerMask targetLayers;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1f;

    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotCooldown = 0.3f;
    private float currentShotCooldown = 0f;

    [Header("Shield")]
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private float shieldRegenPerSecond = 10f;
    [SerializeField] private float shieldCostPerSecond = 5f;
    [SerializeField] private float shieldRegenCooldown = 3f;
    private float currentShieldRegenCooldown;
    private bool isShieldActive;

    [Header("UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider shieldSlider;

    [Header("Info (Do not change)")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float maxShield = 50f;
    [SerializeField] private float health;
    [SerializeField] private float shield;

    private Rigidbody playerRb;

    private float verticalInput;
    private float horizontalInput;

    public static PlayerControl Instance;

    void Awake()
    {
        // Singleton reference to this script
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Get reference to player rigidbody component
        playerRb = GetComponent<Rigidbody>();

        // Set max frame rate to avoid errors in editor
        Application.targetFrameRate = 144;
    }

    void Start()
    {
        // Set starting health and shield to max
        health = maxHealth;
        shield = maxShield;

        // Set health UI
        healthSlider.minValue = 0f;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;

        // Set shield UI
        shieldSlider.minValue = 0f;
        shieldSlider.maxValue = maxShield;
        shieldSlider.value = shield;

        // Disable shield by default at start
        shieldObject.SetActive(false);
        isShieldActive = false;
        currentShieldRegenCooldown = 0;
    }

    void Update()
    {
        // Get player input via WASD (used in FixedUpdate)
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        // Rotate orb to face mouse position
        MoveOrb();
        
        // Shoot projectile on left-click and when cooldown is zero
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentShotCooldown == 0)
        {
            Instantiate(projectilePrefab, orbObject.transform.position, orbFocus.transform.rotation);
            currentShotCooldown = shotCooldown;
        }

        // Reduce cooldown to zero
        if (currentShotCooldown > 0)
        {
            currentShotCooldown -= Time.deltaTime;
            if (currentShotCooldown < 0)
                currentShotCooldown = 0;
        }

        // Enable shield when right-click is held down
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (!isShieldActive && shield > 0)
            {
                ShieldActive(true);
            }
        }

        // Disable shield when right-click is released
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            if (isShieldActive)
            {
                ShieldActive(false);
            }
        }

        // Disable shield when value below zero
        if (shield <= 0)
        {
            ShieldActive(false);
        }

        // Decrease shield value when active
        if (isShieldActive)
        {
            float shieldReduce = shieldCostPerSecond * Time.deltaTime;
            ModifyShield(-shieldReduce);
        }

        // Reduce shield regen cooldown to zero
        if (currentShieldRegenCooldown > 0 && !isShieldActive)
        {
            currentShieldRegenCooldown -= Time.deltaTime;
            if (currentShieldRegenCooldown < 0)
                currentShieldRegenCooldown = 0;
        }

        // Regen shield when shield < 0 and regen = 0
        if (shield < maxShield && currentShieldRegenCooldown == 0 && !isShieldActive)
        {
            float shieldRegen = shieldRegenPerSecond * Time.deltaTime;
            ModifyShield(shieldRegen);
        }

        // Update Health and Shield UI
        healthSlider.value = health;
        shieldSlider.value = shield;

        // Temp code to freeze game when health is zero
        if (health == 0 && Time.timeScale > 0)
        {
            Time.timeScale = 0;
            currentShotCooldown = shotCooldown;
        }
    }

    void FixedUpdate()
    {
        // Move player using rigidbody physics
        Move();
    }

    // Method to move player
    void Move()
    {
        // Create direction vector based on player input
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput);

        // Clamp direction vector magnitude at 1 to prevent faster diagonal speed
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        // Move player (non-physics movement)
        // transform.Translate(movement * moveSpeed * Time.deltaTime);

        // Move player (via physics movement)
        playerRb.linearVelocity = movement * moveSpeed;
    }

    // Method to spin orb around player to always point at mouse position
    void MoveOrb()
    {
        // Draw a ray from camera through the mouse pointer
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Cast the ray until it hits an object in the selected layers
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

    // Method to add or remove health
    public void ModifyHealth(float changeHealth)
    {
        health += changeHealth;

        // Restrict value between zero and max
        if (health < 0)
            health = 0;
        else if (health > maxHealth)
            health = maxHealth;
    }

    // Method to add or remove shield
    public void ModifyShield(float changeShield)
    {
        shield += changeShield;

        // Restrict value between zero and max
        if (shield < 0)
            shield = 0;
        else if (shield > maxShield)
            shield = maxShield;
    }

    // Method to enable or disable shield
    void ShieldActive(bool changeToState)
    {
        if (changeToState)
        {
            shieldObject.SetActive(true);
            isShieldActive = true;
            currentShieldRegenCooldown = shieldRegenCooldown;
        }
        else if (!changeToState)
        {
            shieldObject.SetActive(false);
            isShieldActive = false;
        }
    }
}
