using UnityEngine;
using Cinemachine;

public class FOVZoomSmooth : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float                    startFOV   = 60f;
    public float                    targetFOV  = 12f;
    public float                    smoothTime = 0.5f;

    private float currentVelocity;

    private void Start()
    {
        vcam.m_Lens.FieldOfView = startFOV;
    }

    private void Update()
    {
        vcam.m_Lens.FieldOfView = CalculateSmoothFOV(
            vcam.m_Lens.FieldOfView,
            targetFOV
        );
    }

    private float CalculateSmoothFOV(float current, float target)
    {
        return Mathf.SmoothDamp(
            current,
            target,
            ref currentVelocity,
            smoothTime
        );
    }

}