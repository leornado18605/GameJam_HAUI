using UnityEngine;
using Cinemachine;
using System;
using System.Collections;
using DG.Tweening;
using UnityEngine.Animations;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    private UnityEngine.PostProcessing.PostProcessingProfile profile;

    public CinemachineVirtualCamera[] cams;
    public float[]                    delays = { 3, 2, 2, 3, 5, 3 };

    private                  Action                    onFinished;
    public                   Animator                  aiAnimator; // ← thêm dòng này
    public                   FOVZoomAccelerated        zoomScript; // gán trên Inspector cho camera số 2
    [SerializeField] private Animator                  playerAnimatorr;
    public                   ScriptableObject          postProcess;
    [SerializeField] private AudioSource               screamAudio;
    [SerializeField] private AIController              aiController;
    [SerializeField] private GameObject                lightObject;
    [SerializeField] private Light                     targetLight;
    [SerializeField] private GameObject                aiObjectThay;
    [SerializeField] private GameObject                aiObjectbd;
    [SerializeField] private RuntimeAnimatorController newController;

    private                  Animator                  anim;
    public GameObject cube;
    [SerializeField] private RuntimeAnimatorController originalController;
    [SerializeField] private GameObject                screamParticle;
    [SerializeField] private Transform                 screamSpawnPoint;
    [SerializeField] private GameObject                Ob1;
    [SerializeField] private GameObject Ob2;
    private void Awake()
    {
        profile            = postProcess as UnityEngine.PostProcessing.PostProcessingProfile;
        this.aiObjectThay.gameObject.SetActive(false);
        foreach (var cam in cams)
        {
            cam.gameObject.SetActive(true);
            cam.Priority = 0;
        }
    }
    private void SpawnScreamParticle()
    {
        if (screamParticle == null) return;

        Vector3    pos = screamSpawnPoint != null ? screamSpawnPoint.position : aiAnimator.transform.position;
        Quaternion rot = screamSpawnPoint != null ? screamSpawnPoint.rotation : aiAnimator.transform.rotation;

        GameObject p = Instantiate(screamParticle, pos, rot);
        Destroy(p, 3f);
    }

    private void Start()
    {
        anim               = playerAnimatorr;

        originalController = anim.runtimeAnimatorController;

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

            if (i == 6)
            {
                DOVirtual.DelayedCall(12f, () =>
                    {
                        Ob1.SetActive(true);
                        Ob2.SetActive(false);
                    }

                );
            }
            if (i == 1)
            {
                anim.runtimeAnimatorController = newController;
            }

            if (i == 2)
            {
                anim.runtimeAnimatorController = originalController;
            }
            // CAMERA SỐ 2
            if (i == 3)
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

                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                            screamParticle.SetActive(true);
                        });
                        // Tự tắt lại sau 2 giây

                        // Delay 2 giây rồi bật tiếng hét
                        DOVirtual.DelayedCall(0.5f, () =>
                        {
                                this.screamAudio.gameObject.SetActive(true);
                                screamAudio.Play();
                        });
                        DOVirtual.DelayedCall(3f, () =>
                        {
                            screamParticle.SetActive(false);
                        });

                        PlayVignetteSmoothnessEffect();

                    });

                }
            }

            if (i == 4)
            {
                DOVirtual.DelayedCall(3f, () =>
                    {
                        aiController.enabled = true;
                    }
                );
            }

            if (i == 5)
            {
                DOVirtual.DelayedCall(3f, () =>
                {
                    lightObject.SetActive(true);
                });


                DOVirtual.DelayedCall(3.4f, () =>
                {
                    cube.gameObject.SetActive(true);
                    if (targetLight != null)
                    {
                        // Gán giá trị ban đầu
                        targetLight.range     = 1.54f;
                        targetLight.intensity = 14.07f;

                        // Tween Range
                        DOTween.To(
                            () => targetLight.range,
                            x => targetLight.range = x,
                            235.92f, // giá trị cuối
                            1f       // thời gian
                        ).SetEase(Ease.OutCubic);

                        // Tween Intensity
                        DOTween.To(
                            () => targetLight.intensity,
                            x => targetLight.intensity = x,
                            1668f,          // giá trị cuối
                            1f
                        ).SetEase(Ease.OutCubic);
                    }
                });


            }
            if (i == 6)
            {
                this.aiObjectbd.gameObject.SetActive(false);
                this.aiObjectThay.gameObject.SetActive(true);
                lightObject.SetActive(false);
                DOVirtual.DelayedCall(2f, () =>
                {
                    playerAnimatorr.SetTrigger("Look");
                });
            }

            yield return new WaitForSeconds(delays[i]);

            if (i == 3)
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