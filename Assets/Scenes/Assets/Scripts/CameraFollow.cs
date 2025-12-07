using DG.Tweening;
using UnityEngine;

public class MouseFollowCamera : Singleton<MouseFollowCamera>
{
    private UnityEngine.PostProcessing.PostProcessingProfile profile;
    public Transform player;
    public                   ScriptableObject          postProcess;
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

    public void OnEnable()
    {
        profile            = postProcess as UnityEngine.PostProcessing.PostProcessingProfile;
        SoundManager.Instance.StopMusic();
    }
    
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
    public void PlayVignetteSmoothnessEffect()
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
                0.1f
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