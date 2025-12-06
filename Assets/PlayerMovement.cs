using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AutoMoveToPoint : MonoBehaviour
{
    public Transform targetPoint;  // Điểm B
    public float     moveSpeed    = 5f;
    public float     stopDistance = 0.2f;

    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (targetPoint == null) return;

        // Vector hướng từ nhân vật tới điểm B
        Vector3 direction = targetPoint.position - transform.position;
        direction.y = 0f;                 // Không bị nghiêng theo trục Y
        float distance = direction.magnitude;

        // Nếu còn xa hơn khoảng dừng thì tiếp tục di chuyển
        if (distance > stopDistance)
        {
            Vector3 move = direction.normalized;

            // Xoay nhân vật theo hướng đi
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.LookRotation(move),
                10f * Time.deltaTime
            );

            controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }
}