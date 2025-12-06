using UnityEngine;
using Cinemachine;

public class FOVZoomAccelerated : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    [Header("FOV Settings")]
    public float startFOV  = 60f;
    public float targetFOV = 12f;

    [Header("Speed Control")]
    public float initialSpeed = 5f;  // tốc độ ban đầu (chậm)
    public float acceleration = 10f; // tốc độ tăng dần mỗi giây

    private float currentSpeed;

    private void Start()
    {
        vcam.m_Lens.FieldOfView = startFOV;
        currentSpeed            = initialSpeed;
    }

    private void Update()
    {
        // Tăng tốc độ mỗi frame
        currentSpeed += acceleration * Time.deltaTime;

        // Zoom từ FOV hiện tại → targetFOV với tốc độ tăng dần
        vcam.m_Lens.FieldOfView = Mathf.MoveTowards(
            vcam.m_Lens.FieldOfView,
            targetFOV,
            currentSpeed * Time.deltaTime
        );
    }
}