using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera[] cams;
    public float[] delays = { 3, 2, 2, 3, 5, 3 };

    private int currentIndex = -1;

    private void Awake()
    {
        // Disable toàn bộ camera để Cinemachine không blend từ chúng
        foreach (var cam in cams)
        {
            if (cam != null)
            {
                cam.Priority = 0;
                cam.gameObject.SetActive(false);
            }
        }
    }

    // Hàm để GameManager gọi
    public void StartSequence()
    {
        StartCoroutine(RunSequence());
    }

    private IEnumerator RunSequence()
    {
        for (int i = 0; i < cams.Length; i++)
        {
            if (i == 3)
            {
                yield return StartCoroutine(DoSpecialFlicker(i));
            }
            else
            {
                Activate(i);
                yield return new WaitForSeconds(delays[i]);
            }
        }
    }

    private IEnumerator DoSpecialFlicker(int index)
    {
        Activate(index);
        yield return new WaitForSeconds(0.3f);

        Deactivate(index);
        yield return new WaitForSeconds(0.2f);

        Activate(index);
        yield return new WaitForSeconds(0.3f);

        Deactivate(index);
        yield return new WaitForSeconds(0.15f);

        Activate(index);
        yield return new WaitForSeconds(delays[index]);
    }

    private void Activate(int index)
    {
        // Tắt camera cũ
        if (currentIndex >= 0)
        {
            cams[currentIndex].Priority = 0;
            cams[currentIndex].gameObject.SetActive(false);
        }

        // Bật camera mới
        cams[index].gameObject.SetActive(true);
        cams[index].Priority = 20;

        currentIndex = index;
    }

    private void Deactivate(int index)
    {
        cams[index].Priority = 0;
        cams[index].gameObject.SetActive(false);

        if (currentIndex == index)
            currentIndex = -1;
    }
}