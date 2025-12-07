/*using UnityEngine;
using TMPro;

namespace Script
{
    public class PlayerDetectGame : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text hintText;

        [Header("Detect Settings")]
        [SerializeField] private float detectRadius = 2f;
        [SerializeField] private LayerMask safeLayer;    // Layer két sắt
        [SerializeField] private float hideDelay = 2f;

        private bool  isNearSafe = false;
        private float hideTimer  = 0f;

        private void Start()
        {
            if (hintText != null)
                hintText.gameObject.SetActive(false);
        }

        private void Update()
        {
            DetectSafe();

            if (isNearSafe)
            {
                hintText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log("Đã mở khóa");
                    HideHint();
                }
            }

            if (hintText.gameObject.activeSelf)
            {
                hideTimer -= Time.deltaTime;
                if (hideTimer <= 0)
                {
                    HideHint();
                }
            }
        }

        private void DetectSafe()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectRadius, safeLayer);

            bool foundSafe = hits.Length > 0;

            if (foundSafe && !isNearSafe)
            {
                isNearSafe = true;
                ShowHint("Nhấn E để mở khóa két sắt");
            }
            else if (!foundSafe && isNearSafe)
            {
                isNearSafe = false;
                HideHint();
            }
        }

        private void ShowHint(string msg)
        {
            hintText.text = msg;
            hintText.gameObject.SetActive(true);
            hideTimer = hideDelay;
        }

        private void HideHint()
        {
            hintText.gameObject.SetActive(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectRadius);
        }
    }
}*/