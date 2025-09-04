using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private GameObject orbFocus;
    [SerializeField]
    private GameObject orbObject;
    [SerializeField]
    private GameObject mouseFocus;

    [SerializeField]
    private float moveSpeed = 1.0f;

    [Space]

    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float shotCooldown = 0.3f;
    private float currentCooldown = 0f;

    private Ray ray;
    private RaycastHit hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
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
        float verMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        float horMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;

        transform.Translate(horMove, 0, verMove);
    }

    void MoveOrb()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            mouseFocus.transform.position = new Vector3 (hit.point.x, 0, hit.point.z);
        }

        orbFocus.transform.LookAt(mouseFocus.transform);
    }
}
