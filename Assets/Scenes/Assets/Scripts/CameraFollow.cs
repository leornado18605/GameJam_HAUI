using UnityEngine;

public class MouseFollowCamera : MonoBehaviour
{
    public Transform player;

    [Header("Camera Settings")]
    public float mouseSensitivity = 300f;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 10f;

    [Header("Collision Settings")]
    public float collisionOffset = 0.3f;   // khoảng cách đẩy camera ra khỏi tường
    public LayerMask collisionMask; 
    
    private Vector3 currentVelocity;   // cho SmoothDamp
    public float smoothTime = 0.15f; // chọn layer tường / map

    private float yaw;
    private float pitch;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if (!player) return;

        // --- INPUT CHUỘT ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // --- TÍNH VỊ TRÍ CAMERA LÝ TƯỞNG ---
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 idealOffset = rot * new Vector3(0, 0, -distance);
        Vector3 target = player.position + Vector3.up * height;

        Vector3 idealPos = target + idealOffset;

        // --- CAMERA COLLISION ---
        Vector3 finalPos = idealPos;
        RaycastHit hit;

        if (Physics.Raycast(target, (idealPos - target).normalized, out hit, distance, collisionMask))
        {
            finalPos = hit.point + hit.normal * collisionOffset;
            finalPos.y += 0.3f;
        }

    // --- SMOOTH CAMERA (fix jitter 100%) ---
        transform.position = Vector3.SmoothDamp(transform.position, finalPos, ref currentVelocity, smoothTime);

    // --- LOOK AT ---
        transform.LookAt(target);
    }
}