using UnityEngine;

public class MouseFollowCamera : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 300f;
    public float distance = 5f;
    public float height = 2f;
    public float smoothSpeed = 10f;

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
        if (player == null) return;

        // --- 1. Input chuột ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // --- 2. AUTO CAMERA TURN khi nhấn A/D/S ---
        float horizontal = Input.GetAxis("Horizontal");   // A/D
        //float vertical = Input.GetAxis("Vertical");       // W/S

        if (horizontal != 0 )   // Nhấn A/D hoặc đi lùi
        {
            // Camera auto xoay về hướng player đang facing
            yaw = Mathf.Lerp(yaw, player.eulerAngles.y, Time.deltaTime * 5f);
        }

        // --- 3. Tính vị trí camera ---
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rot * new Vector3(0, 0, -distance);
        Vector3 targetPos = player.position + Vector3.up * height + offset;

        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);

        // --- 4. Camera nhìn player ---
        transform.LookAt(player.position + Vector3.up * height);

        // --- 5. Player xoay theo camera khi có input ---
        if (horizontal != 0 )
        {
            player.rotation = Quaternion.Euler(0, yaw, 0);
        }
    }
}