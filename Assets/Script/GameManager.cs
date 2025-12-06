using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinematicGameManager : MonoBehaviour
{
    [Header("References")]
    public Transform triggerPoint;
    public Transform player;

    public CameraController cameraController;

    [Header("Settings")]
    public float triggerDistance = 1.5f;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private MouseFollowCamera mouse;
    private                  bool        cutsceneStarted = false;
    private void Awake()
    {
        // CameraController ban đầu phải tắt
        cameraController.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (cutsceneStarted) return;

        if (Vector3.Distance(player.position, triggerPoint.position) <= triggerDistance)
        {
            StartCutscene();
        }
    }

    private void StartCutscene()
    {
        cutsceneStarted = true;

        // Tắt input của player
        if (playerInput != null)
            playerInput.enabled = false;

        // Bật CameraController
        this.mouse.enabled = false;
        cameraController.gameObject.SetActive(true);

        // Gọi cutscene + callback khi xong
        cameraController.StartSequence(OnCutsceneFinished);
    }

    private void OnCutsceneFinished()
    {
        // Bật lại PlayerInput khi cutscene xong
        if (playerInput != null)
            playerInput.enabled = true;
        this.mouse.enabled = true;

        cameraController.DisableAllCameras();
        // Nếu muốn tắt CameraController sau cutscene
        cameraController.gameObject.SetActive(false);

    }
}