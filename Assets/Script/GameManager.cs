using System;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
public class CinematicGameManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform        teleportTarget;
    public CameraController cameraController;
    //public GameObject Cine;
    public GameObject Trigger;
    public GameObject Player;

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
                cc.enabled      = true;
            }
            else
            {
                player.position = teleportTarget.position;
                player.rotation = teleportTarget.rotation;
            }
        }

        // Tắt input của player
        /*if (playerInput != null)
            playerInput.enabled = false;*/

        //mouse.enabled = false;
        cameraController.gameObject.SetActive(true);
       // cameraController.SetCutsceneAnimator();

        cameraController.StartSequence(OnCutsceneFinished);
    }

    private void OnCutsceneFinished()
    {
        //  cameraController.RevertAnimator();
        /*if (playerInput != null)
            playerInput.enabled = true;*/
    
//        mouse.enabled = true;
        /*Player.gameObject.SetActive(true);
        Trigger.gameObject.SetActive(false);*/
        cameraController.DisableAllCameras();
        cameraController.gameObject.SetActive(false);
        
    }
}