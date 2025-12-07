using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttackController : Singleton<PlayerAttackController>
{

    public bool win1;
    public bool win2;
    public bool win3;
    
    public GameObject minigame1;
    public LockPick minigame2;
    
    [SerializeField] private int counter = 0;
    [Header("Stats")]
    public GameObject hitEffect;
    public float attackRange = 1.5f; 
    public float damage = 10f;
    public float heath = 100f;
    public float attackRate = 1f;   // 1 hit / giây
    
    [SerializeField] private EnemyAttack target;
    [SerializeField] private GameObject textI;

    public EnemyAttack Target
    {
        get => target;
        set => target = value;
    }

    public LayerMask enemyLayer;
    public LayerMask weaponLayer;

    private void Update()
    {
        target = FindEnemy();
        
        GameObject obj1 = GetWeapon();
        if (obj1 == null)
        {
            textI.SetActive(false);
            return;
        }
        textI.SetActive(true);
        // UI hienej text
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject obj = GetWeapon();
            if (obj == null) return;
                
            int layer = obj.layer;

            if (layer == LayerMask.NameToLayer("Weapon"))
            {
                gameObject.GetComponent<PlayerController>().TakeWeapon(obj);
            }
            else if (layer == LayerMask.NameToLayer("ImageMini"))
            {
                counter++;
                obj.SetActive(false);
            }
            else if (layer == LayerMask.NameToLayer("UiMinigame1"))
            {
                if (counter == 3)
                {
                    minigame1.SetActive(true);
                    obj.SetActive(false);
                    win1 = true;
                }
                    
                else
                {
                    
                }
            }
            else if (layer == LayerMask.NameToLayer("minigame3"))
            {
                TextManager.Instance.gameObject.SetActive(true);
                obj.SetActive(false);
                win3 = true;
                TextManager.Instance.text.text = "12/10/1970 - 02:30 PM]\nElly à,\n\nBa đã sai rồi. Chúng ta cứ nghĩ Thời Gian là nguồn năng lượng vô tận, là món quà của Chúa. Nhưng hôm nay, khi tia sét đánh trúng tháp dẫn, thứ chất lỏng màu xanh đó đã trào ra... hòa vào cơn mưa.\n\nBa nhìn qua cửa sổ phòng Lab. Mọi người bên dưới không chạy trốn. Khi mưa chạm vào da, họ đứng khựng lại. Họ không chết, con à. Họ bị 'đóng băng' trong nỗi đau đớn tột cùng. Thời gian trong cơ thể họ bị bẻ gãy, khiến họ giật lắc như những con rối đứt dây.\n\nBa vô tình đã biến thị trấn này thành địa ngục. Ba phải xuống đó sửa chữa sai lầm này. Đừng đến tìm ba.\nYêu con";
            }
            
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
            minigame2.Play();
            other.gameObject.tag = "Default";
        
        
        Debug.Log(other.gameObject.name);
    }

    public EnemyAttack FindEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

        if (hits.Length > 0)
            return hits[0].GetComponent<EnemyAttack>();

        return null;
    }

    public GameObject GetWeapon()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, weaponLayer);

        if (hits.Length > 0)
        {
            return hits[0].gameObject;
        }
            

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage()
    {
        if (target != null)
        {
            gameObject.GetComponent<PlayerController>().timeSinceDamage = 0f;
            target.GetDamage(damage);
           SoundManager.Instance.PlaySFX(SoundManager.Instance.playClip);
            var effect = Instantiate(hitEffect, target.transform.position, Quaternion.identity);
            effect.GetComponent<ParticleSystem>().Play();
            effect.transform.localScale *= 1.0f;
            effect.transform.position += Vector3.up * 1f;
            Destroy(effect,1f);
        }
            
    }

    public void GetDamage(float damage)
    {
        gameObject.GetComponent<PlayerController>().timeSinceDamage = 0f;
        if(gameObject.GetComponent<PlayerController>().isDeath) return;
        MouseFollowCamera.Instance.PlayVignetteSmoothnessEffect();
        heath -= damage;
        UpdateStatus.Instance.OnUpdateHealth(heath/gameObject.GetComponent<PlayerController>().maxHealth);
        if (heath <= 0)
        {
            gameObject.GetComponent<PlayerController>().Die();
        }
    }
    
}