using UnityEngine;
using Cinemachine;
using System;

public class FOVZoomAccelerated : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;

    public float startFOV  = 60f;
    public float targetFOV = 12f;

    public float initialSpeed = 5f;
    public float acceleration = 10f;

    public Action OnZoomFinished;

    private float currentSpeed;
    private bool  zoomFinished;

    private void OnEnable()
    {
        ResetZoom();
    }

    public void ResetZoom()
    {
        zoomFinished = false;
        currentSpeed = initialSpeed;

        if (vcam != null)
            vcam.m_Lens.FieldOfView = startFOV;
    }

    private void Update()
    {
        if (zoomFinished) return;

        currentSpeed += acceleration * Time.deltaTime;

        vcam.m_Lens.FieldOfView = Mathf.MoveTowards(
            vcam.m_Lens.FieldOfView,
            targetFOV,
            currentSpeed * Time.deltaTime
        );

        if (Mathf.Abs(vcam.m_Lens.FieldOfView - targetFOV) < 0.01f)
        {
            zoomFinished = true;
            OnZoomFinished?.Invoke();
        }
    }
}