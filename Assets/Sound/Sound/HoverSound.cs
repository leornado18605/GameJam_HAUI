using UnityEngine;
using UnityEngine.EventSystems;

public class HoverSound : MonoBehaviour, IPointerEnterHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.btnClip);
    }
}