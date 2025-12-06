using UnityEngine;
using Cinemachine;
using System;
using System.Collections;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public float[]                    delays = { 3, 2, 2, 3, 5, 3 };

    private Action             onFinished;
    public  Animator           aiAnimator; // ← thêm dòng này
    public  FOVZoomAccelerated zoomScript; // gán trên Inspector cho camera số 2

    private void Awake()
    {
        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(true);
            cam.Priority = 0;
        }
    }

    public void StartSequence(Action callback)
    {
        onFinished = callback;
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        ActivateOnly(0);
        yield return new WaitForSeconds(delays[0]);

        for (int i = 1; i < cams.Length; i++)
        {
            ActivateOnly(i);

            // CAMERA SỐ 2
            if (i == 2)
            {
                if (zoomScript != null)
                {
                    zoomScript.enabled = true; // BẮT BUỘC BẬT
                    zoomScript.ResetZoom();    // RESET MỖI LẦN DÙNG

                    DOVirtual.DelayedCall(2f, () =>
                    {
                        aiAnimator?.SetBool("Scream", true);
                    });

                }
            }

            yield return new WaitForSeconds(delays[i]);

            if (i == 2)
            {
                aiAnimator?.SetBool("Scream", false);
                zoomScript.enabled = false; // tắt đi khi xong
            }
        }

        DeactivateAll();
        onFinished?.Invoke();
    }


    private void ActivateOnly(int index)
    {
        for (int i = 0; i < cams.Length; i++)
            cams[i].Priority = 0;

        cams[index].Priority = 20;
    }

    private void DeactivateAll()
    {
        foreach (var cam in cams)
            cam.Priority = 0;
    }

    public void DisableAllCameras()
    {
        foreach (var cam in cams)
        {
            if (cam != null)
            {
                cam.Priority = 0;
                cam.gameObject.SetActive(false);
            }
        }
    }

}