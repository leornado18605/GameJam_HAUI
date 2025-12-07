using UnityEngine;

public class MouseFollowCamera : Singleton<MouseFollowCamera>
{
    public Transform player;

    [Header("Camera Settings")]
    public float mouseSensitivity = 300f;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 10f;
    public float smoothCam = 10f;

    [Header("Collision Settings")]
    public float collisionOffset = 0.3f;   // khoảng cách đẩy camera ra khỏi tường
    public LayerMask collisionMask;        // chọn layer tường / map

    private float yaw;
    private float pitch;
    public float minPitch = -20f;
    public float maxPitch = 60f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", mouseSensitivity);
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
        

        // --- SMOOTH CAMERA ---
        transform.position = Vector3.Lerp(transform.position, finalPos, smoothCam );

        // --- NHÌN PLAYER ---
        transform.LookAt(target);
    }
}