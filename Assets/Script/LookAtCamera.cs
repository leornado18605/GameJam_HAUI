using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCam == null) return;

        // Luôn xoay hướng về camera
        transform.LookAt(mainCam.transform);
        transform.rotation = Quaternion.LookRotation(mainCam.transform.forward);
    }
}