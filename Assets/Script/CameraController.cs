using UnityEngine;
using Cinemachine;
using System;
using System.Collections;
using DG.Tweening;
public class CameraController : MonoBehaviour
{
    private UnityEngine.PostProcessing.PostProcessingProfile profile;

    public CinemachineVirtualCamera[] cams;
    public float[]                    delays = { 3, 2, 2, 3, 5, 3 };

    private                  Action             onFinished;
    public                   Animator           aiAnimator; // ← thêm dòng này
    public                   FOVZoomAccelerated zoomScript; // gán trên Inspector cho camera số 2
    [SerializeField] private Animator           playerAnimatorr;
    public                   ScriptableObject   postProcess;
    [SerializeField] private AudioSource        screamAudio;

    private void Awake()
    {
        profile = postProcess as UnityEngine.PostProcessing.PostProcessingProfile;

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

        for (int i = 0; i < cams.Length; i++)
        {
            ActivateOnly(i);

            if (i == 0)
            {
                playerAnimatorr.SetFloat("Speed", 2.5f);
            }
            // CAMERA SỐ 2
            if (i == 2)
            {
                if (zoomScript != null)
                {
                    zoomScript.enabled = true; // BẮT BUỘC BẬT
                    zoomScript.ResetZoom();    // RESET MỖI LẦN DÙNG

                    DOVirtual.DelayedCall(2f, () =>
                    {
                        // 🔥 Xoay về 0 độ trong 1 giây
                        aiAnimator.transform
                            .DORotate(
                                new Vector3(0, 0, 0),    // Góc cần xoay tới
                                1f,                      // Thời gian
                                RotateMode.FastBeyond360 // Cho phép xoay vượt góc để mượt
                            )
                            .SetEase(Ease.OutCubic);

                        aiAnimator?.SetBool("Scream", true);

                        // Delay 2 giây rồi bật tiếng hét
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                                this.screamAudio.gameObject.SetActive(true);
                                screamAudio.Play();
                        });


                        PlayVignetteSmoothnessEffect();

                    });

                }
            }

            if (i == 5)
            {
                DOVirtual.DelayedCall(2f, () =>
                {
                    playerAnimatorr.SetTrigger("Look");
                });
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

    private void PlayVignetteSmoothnessEffect()
    {
        if (profile == null) return;

        var vignette = profile.vignette;
        if (vignette == null) return;

        // Bật hiệu ứng
        vignette.enabled = true;

        float randomValue = UnityEngine.Random.Range(0.38f, 0.86f);

        // Lấy settings kiểu struct
        var settings = vignette.settings;

        // Tween trực tiếp trên DOTween
        DOTween.To(
                () => settings.smoothness,
                x =>
                {
                    settings.smoothness = x;
                    vignette.settings   = settings; // Gán lại vì đây là struct
                },
                randomValue,
                5f
            )
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // Tắt hiệu ứng sau khi xong
                settings.smoothness = 0f;
                vignette.settings   = settings;
                vignette.enabled    = false;
            });
    }


}