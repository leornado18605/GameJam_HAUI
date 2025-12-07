using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockPick : MonoBehaviour
{
    public Camera cam;
    public GameObject player;
    public Transform innerLock;
    public Transform pickPosition;

    public float maxAngle = 90;
    public float lockSpeed = 10;

    [Range(1,25)]
    public float lockRange = 10;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    public  GameObject safe;
    private float      keyPressTime = 0;

    public GameObject lock1;
    private bool movePick = true;

    public Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = pickPosition.position;

        if(movePick)
        {
            Vector3 dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);

            eulerAngle = Vector3.Angle(dir, Vector3.up);

            Vector3 cross = Vector3.Cross(Vector3.up, dir);
            if (cross.z < 0) { eulerAngle = -eulerAngle; }

            eulerAngle = Mathf.Clamp(eulerAngle, -maxAngle, maxAngle);

            Quaternion rotateTo = Quaternion.AngleAxis(eulerAngle, Vector3.forward);
            transform.rotation = rotateTo;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            movePick = false;
            keyPressTime = 1;
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            movePick = true;
            keyPressTime = 0;
        }

        float percentage = Mathf.Round(100 - Mathf.Abs(((eulerAngle - unlockAngle) / 100) * 100));
        float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;
        float maxRotation = (percentage / 100) * maxAngle;

        float lockLerp = Mathf.Lerp(innerLock.eulerAngles.z, lockRotation, Time.deltaTime * lockSpeed);
        innerLock.eulerAngles = new Vector3(0, 0, lockLerp);

        if(lockLerp >= maxRotation -1)
        {
            if (Mathf.Abs(eulerAngle - unlockAngle) < lockRange)

            {
                Debug.Log("Unlocked!");
                Win();
                return;


                movePick = true;
                keyPressTime = 0;
            }
            else
            {
                float randomRotation = Random.insideUnitCircle.x;
                transform.eulerAngles += new Vector3(0, 0, Random.Range(-randomRotation, randomRotation));
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            safe.transform.gameObject.tag = "Safe";
            this.gameObject.SetActive(false);
            lock1.gameObject.SetActive(false);
            this.mainCamera.gameObject.SetActive(true);
            this.cam.gameObject.SetActive(false);
            player.SetActive(true);

        }

        
    }

    public void Play()
    {
        Debug.Log("Play");
        this.gameObject.SetActive(true);
        lock1.gameObject.SetActive(true);
        this.mainCamera.gameObject.SetActive(false);
        this.cam.gameObject.SetActive(true);
        player.SetActive(false);
    }

    void Win()
    {
        Cursor.lockState = CursorLockMode.Locked;
        this.lock1.gameObject.SetActive(false);
        safe.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        this.mainCamera.gameObject.SetActive(true);
        this.cam.gameObject.SetActive(false);
        player.SetActive(true);
        // Hien thu
        TextManager.Instance.gameObject.SetActive(true);
        player.transform.gameObject.GetComponent<PlayerAttackController>().win2 = true;
        TextManager.Instance.text.text = "Con gái yêu quý,\n\nKhi con bước vào căn phòng phía trước, con sẽ thấy 'chúng ta'. Con sẽ thấy ba, mẹ và con đang cười đùa trong khu vườn ngập nắng.\n\nNhưng làm ơn, đừng chạm vào nó. Đó là lời nói dối ngọt ngào nhất thế gian.\n\nĐể cứu con, để cứu linh hồn của những người dân đang bị mắc kẹt ngoài kia, ba phải làm một điều tàn nhẫn. Ba phải đập vỡ 'hy vọng' đó. Ba phải phá hủy Viên Đá.\n\nCó thể ba sẽ biến mất vĩnh viễn, không còn luân hồi, không còn ký ức. Nhưng ít nhất, con sẽ có một tương lai thật sự.\n\nSống tốt nhé, Elly. Đừng nhìn lại.";
    }
}