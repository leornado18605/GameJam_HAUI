using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform triggerPoint;
    [SerializeField] private Transform                            player;
    [SerializeField] private CameraController                     cameraController;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera playerCamera;

    [Header("Settings")]
    [SerializeField] private float triggerDistance = 1.5f;

    private bool cutsceneStarted = false;

    private void Start()
    {
        EnablePlayerCamera(); // Player camera bật trước
    }

    private void Update()
    {
        if (cutsceneStarted) return;

        if (IsPlayerReachedTrigger())
        {
            StartCutscene();
        }
    }

    private bool IsPlayerReachedTrigger()
    {
        return Vector3.Distance(player.position, triggerPoint.position) <= triggerDistance;
    }

    private void StartCutscene()
    {
        cutsceneStarted = true;

        // Tắt camera player để chuyển sang cutscene
        DisablePlayerCamera();

        // Gọi cutscene
        cameraController.StartSequence();
    }

    private void EnablePlayerCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.Priority = 20;
            playerCamera.gameObject.SetActive(true);
        }
    }

    private void DisablePlayerCamera()
    {
        if (playerCamera != null)
        {
            playerCamera.Priority = 0;
            playerCamera.gameObject.SetActive(false);
        }
    }
}