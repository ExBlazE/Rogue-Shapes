using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private GameObject orbFocus;
    [SerializeField] private GameObject orbObject;
    [SerializeField] private LayerMask targetLayers;

    [SerializeField] private float moveSpeed = 1.0f;

    [Space]

    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotCooldown = 0.3f;
    private float currentCooldown = 0f;

    void FixedUpdate()
    {
        Move();
        MoveOrb();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentCooldown == 0)
        {
            Instantiate(projectilePrefab, orbObject.transform.position, orbFocus.transform.rotation);
            currentCooldown = shotCooldown;
        }

        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
            if (currentCooldown < 0)
                currentCooldown = 0;
        }
    }

    void Move()
    {
        float verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        Vector3 movement = new Vector3(horizontalMove, 0, verticalMove);

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
