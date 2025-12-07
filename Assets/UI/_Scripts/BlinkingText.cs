using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlinkingText : MonoBehaviour
{
    [Header("Cài đặt nhấp nháy")]
    [SerializeField] private Text targetText;  // Kéo Text UI vào đây
    [Range(0f, 1f)]
    [SerializeField] private float minAlpha = 0.2f;       // Alpha min (50/255 ≈ 0.2)
    [Range(0f, 1f)]
    [SerializeField] private float maxAlpha = 1f;         // Alpha max (255/255 = 1)
    [Range(0.1f, 5f)]
    [SerializeField] private float blinkSpeed = 1f;       // Tốc độ chậm (1 = 2s/lượt)

    private Color originalColor;
    private Coroutine blinkCoroutine;

    void Start()
    {
        // Tự động lấy Text nếu chưa kéo
        if (targetText == null)
            targetText = GetComponent<Text>();

        if (targetText != null)
        {
            originalColor = targetText.color;
            StartBlinking();  // Bắt đầu nhấp nháy
        }
    }

    /// <summary>
    /// Bắt đầu nhấp nháy
    /// </summary>
    public void StartBlinking()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    /// <summary>
    /// Dừng nhấp nháy
    /// </summary>
    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        // Reset alpha max
        targetText.color = new Color(originalColor.r, originalColor.g, originalColor.b, maxAlpha);
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            // Fade IN: từ min → max (chậm)
            float elapsed = 0f;
            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime * blinkSpeed;
                float alpha = Mathf.Lerp(minAlpha, maxAlpha, elapsed);
                SetAlpha(alpha);
                yield return null;
            }

            // Fade OUT: từ max → min (chậm)
            elapsed = 0f;
            while (elapsed < 1f)
            {
                elapsed += Time.deltaTime * blinkSpeed;
                float alpha = Mathf.Lerp(maxAlpha, minAlpha, elapsed);
                SetAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetAlpha(float alpha)
    {
        Color newColor = originalColor;
        newColor.a = alpha;
        targetText.color = newColor;
    }

    void OnDestroy()
    {
        StopBlinking();
    }
}