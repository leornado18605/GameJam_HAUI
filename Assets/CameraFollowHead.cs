using UnityEngine;

public class CameraFollowHead : MonoBehaviour
{
    [Header("References")]
    public Transform head; // Bone đầu (Head)
    public Transform cam;  // Camera cần xoay

    [Header("Settings")]
    public float followSpeed = 5f;
    public Vector3 rotationOffset; // Offset tinh chỉnh góc camera

    void LateUpdate()
    {
        if (head == null || cam == null) return;

        // Lấy hướng nhìn theo bone đầu
        Vector3 lookDir = head.forward;

        // Tạo hướng xoay camera theo hướng đầu
        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        // Áp dụng offset nếu muốn lệch một chút
        targetRot *= Quaternion.Euler(rotationOffset);

        // Xoay mượt theo đầu
        cam.rotation = Quaternion.Slerp(
            cam.rotation,
            targetRot,
            followSpeed * Time.deltaTime
        );
    }
}