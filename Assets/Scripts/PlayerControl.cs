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
        Instance = this;
        shieldObject.SetActive(false);
    }

    void Update()
    {
        Move();
        MoveOrb();
        
        // Shoot projectile on mouse left-click and when cooldown is zero
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

        // Enable shield when mouse right-click is held down
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

    void Move()
    {
        float verticalMove = Input.GetAxis("Vertical");
        float horizontalMove = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);

        // Normalize method only allows magnitude 0 or 1. Use only one.
        // movement.Normalize();

        // ClampMagnitude method allows magnitude range of 0~1. Use only one.
        movement = Vector3.ClampMagnitude(movement, 1.0f);

        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    void MoveOrb()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, targetLayers))
        {
            Vector3 mousePosition = hit.point;
            Vector3 mouseDirection = mousePosition - orbFocus.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(mouseDirection);
            orbFocus.transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);
        }
    }
}
