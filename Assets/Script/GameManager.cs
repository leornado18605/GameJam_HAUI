using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicGameManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform        teleportTarget;
    public CameraController cameraController;

    [SerializeField] private PlayerInput       playerInput;
    [SerializeField] private MouseFollowCamera mouse;

    private bool cutsceneStarted = false;

    private void Awake()
    {
        cameraController.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (cutsceneStarted) return;

        // ⭐ Chỉ Player mới kích hoạt
        if (other.CompareTag("Player"))
        {
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        cutsceneStarted = true;

        // ⭐ DỊCH CHUYỂN PLAYER NGAY LÚC CHẠM TRIGGER ⭐
        if (teleportTarget != null)
        {
            var cc = player.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled      = false;                   // ⭐ TẮT CC
                player.position = teleportTarget.position; // ⭐ TELEPORT OK
                player.rotation = teleportTarget.rotation;
                cc.enabled      = true;                            // ⭐ BẬT LẠI
            }
            else
            {
                player.position = teleportTarget.position;
                player.rotation = teleportTarget.rotation;
            }
        }

        // Tắt input của player
        if (playerInput != null)
            playerInput.enabled = false;

        mouse.enabled = false;
        cameraController.gameObject.SetActive(true);

        cameraController.StartSequence(OnCutsceneFinished);
    }

    private void OnCutsceneFinished()
    {
        if (playerInput != null)
            playerInput.enabled = true;

        mouse.enabled = true;

        cameraController.DisableAllCameras();
        cameraController.gameObject.SetActive(false);
    }
}