using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject focus;
    [SerializeField] private float moveSpeed = 2.5f;
    [SerializeField] private float rotateSpeed = 90;

    void Update()
    {
        Vector3 playerPosition = PlayerControl.Instance.gameObject.transform.position;
        Vector3 direction = playerPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        focus.transform.Rotate(new Vector3(0, 0, rotateSpeed * Time.deltaTime), Space.World);
    }
}
