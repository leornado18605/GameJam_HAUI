using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatus : Singleton<UpdateStatus>
{
    public Image Health;
    public Image Weapon;
    public Sprite[] ListWeapons;

    // Thời gian để thanh máu chạy từ mức cũ sang mức mới (0.5 giây)
    public float transitionDuration = 0.5f;

    // Biến lưu trữ Coroutine đang chạy
    private Coroutine currentHealthCoroutine;

    

    public void OnUpdateHealth(float targetHealth)
    {
        // Nếu đang có hiệu ứng chạy thì dừng lại để chạy cái mới ngay lập tức
        if (currentHealthCoroutine != null) StopCoroutine(currentHealthCoroutine);

        // Bắt đầu Coroutine mới để xử lý cả fillAmount và color
        currentHealthCoroutine = StartCoroutine(AnimateHealthChange(targetHealth));
    }

    public void OnUpdateWeapon(int id)
    {
        
        if (id >= 0 && id < ListWeapons.Length)
        {
            Weapon.sprite = ListWeapons[id];
            Weapon.color = new Color(Weapon.color.r, Weapon.color.g, Weapon.color.b, 1f);
            // Weapon.SetNativeSize(); // Bật dòng này nếu muốn ảnh đúng tỉ lệ gốc
        }
        else
        {
            Debug.LogWarning("Không tìm thấy vũ khí ID: " + id);
        }
    }

    // --- LOGIC MỚI: Xử lý thay đổi máu và màu mượt mà ---
    IEnumerator AnimateHealthChange(float targetHealth)
    {
        float startHealth = Health.fillAmount;
        float timer = 0f;

        // 1. Xác định màu mục tiêu: Giảm thì Đỏ, Tăng thì Xanh
        Color targetColor;
        if (targetHealth < startHealth)
        {
            targetColor = Color.red;   // Máu giảm
        }
        else
        {
            targetColor = Color.green; // Máu tăng
        }

        // 2. GIAI ĐOẠN 1: Thay đổi FillAmount và chuyển sang màu Đỏ/Xanh
        while (timer < transitionDuration)
        {
            timer += Time.deltaTime;
            float t = timer / transitionDuration;

            // Lerp Fill: Chạy từ mức cũ sang mức mới
            Health.fillAmount = Mathf.Lerp(startHealth, targetHealth, t);

            // Lerp Color: Chuyển dần từ màu hiện tại sang Đỏ hoặc Xanh
            // (t * 2 để màu chuyển nhanh hơn thanh máu một chút cho rõ hiệu ứng)
            Health.color = Color.Lerp(Health.color, targetColor, t * 2);

            yield return null;
        }

        // Đảm bảo kết thúc Giai đoạn 1 chỉ số chính xác
        Health.fillAmount = targetHealth;

        // 3. GIAI ĐOẠN 2: Trả màu về Trắng (White)
        timer = 0f;
        float resetDuration = 0.3f; // Thời gian để màu phai về trắng
        Color currentColor = Health.color;

        while (timer < resetDuration)
        {
            timer += Time.deltaTime;
            // Chuyển từ màu hiện tại (Đỏ/Xanh) về Trắng
            Health.color = Color.Lerp(currentColor, Color.white, timer / resetDuration);
            yield return null;
        }

        // Chốt lại là màu trắng tinh
        Health.color = Color.white;
    }
}