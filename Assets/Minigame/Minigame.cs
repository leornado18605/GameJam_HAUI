using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayMiniGame : MonoBehaviour
{
    public List<Image> images;
    public List<Transform> positions;
    public CanvasGroup finalImg;

    public float moveDuration = 0.5f;
    public Ease moveEase = Ease.OutBack;

    public void Start()
    {
        CheckWin();
    }
    

    public void CheckWin()
    {
        // Kiểm tra đủ phần tử
        if (images.Count != 3 || positions.Count != 3)
        {
            Debug.LogWarning("Không đủ images hoặc positions.");
            return;
        }

        // Animate từng image đến đúng vị trí
        for (int i = 0; i < images.Count; i++)
        {
            images[i].transform.DOMove(positions[i].position, moveDuration)
                .SetEase(moveEase);
        }
        DOVirtual.DelayedCall(1.0f, () =>
        {
            finalImg.DOFade(0f, 0.5f);
        }).OnComplete(()=>
        {
            DOVirtual.DelayedCall(0.5f,()=>gameObject.SetActive(false));
            // Hien thu
            TextManager.Instance.gameObject.SetActive(true);
            TextManager.Instance.text.text = "Elly... hay bất cứ ai đọc được dòng này.\n\nNếu con đang cầm tờ giấy này trên tay, nghĩa là ba đã thất bại. Lại một lần nữa.\n\nBa vừa nhìn thấy một cái xác trong góc phòng này. Nó mặc chiếc áo khoác của ba. Nó đeo cái đồng hồ của ba. Elly ơi,xin con đừng thất vọng về người cha.\n\nBa nhận ra sự thật rồi. Viên Đá không đưa ta về quá khứ để cứu vãn. Nó chỉ 'reset' lại nỗi đau. Ba đã đến đây hàng trăm lần, chết hàng trăm kiểu khác nhau, và rồi lại tỉnh dậy như chưa có gì xảy ra.\n\nĐừng tin những gì Viên Đá cho con thấy. Nó muốn giữ chúng ta ở lại đây  lấy nỗi sợ của mỗi ta để làm thức ăn cho nó vĩnh viễn.\n??/???";
        });
        

        Debug.Log("Win animation started!");
    }
}