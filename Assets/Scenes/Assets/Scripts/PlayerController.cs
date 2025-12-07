using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //Third Person Controller References
    [SerializeField]
    private Animator playerAnim;


    //Equip-Unequip parameters
    [SerializeField]
    private GameObject weapon;
    [SerializeField]
    private List<Weapon> weapons;
    public GameObject Weapon
    {
        get => weapon;
        set => weapon = value;
    }

    [SerializeField]
    private GameObject weaponOnShoulder;
    [SerializeField]
    private List<Weapon> weaponOnShoulders;
    [SerializeField]
    private List<GameObject> weaponOnShouldersPrefabs;
    public bool isEquipping;
    public bool isEquipped;


    //Blocking Parameters
    public bool isBlocking;

    public bool isDeath = false;

    //Kick Parameters
    public bool isKicking;

    //Attack Parameters
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;
    
    
    public float maxHealth = 100f;
    public float healAmount = 5f;   // hồi mỗi tick
    public float healDelay = 5f;    // 5 giây sau khi bị đánh
    public float timeSinceDamage = 0f;

    private void Start()
    {
        maxHealth = gameObject.GetComponent<PlayerAttackController>().heath;
    }

    private void Update()
    {
        if(isDeath) return;
        timeSinceAttack += Time.deltaTime;

        timeSinceDamage += Time.deltaTime; 

        RegenHealth();  

        Attack();
        Equip();
        Block();
        Kick();
        if (Input.GetKeyDown(KeyCode.L))
            UIManager.Instance.ReturnMenu();
    }

    private void RegenHealth()
    {
        // nếu nhân vật chết thì không hồi
        if (isDeath) return;

        // chưa đủ 5s → không hồi
        if (timeSinceDamage < healDelay) return;
        
        if(maxHealth ==  gameObject.GetComponent<PlayerAttackController>().heath) return;

        // hồi máu từ từ
        gameObject.GetComponent<PlayerAttackController>().heath += healAmount ;
        UpdateStatus.Instance.OnUpdateHealth(gameObject.GetComponent<PlayerAttackController>().heath/maxHealth);
        timeSinceDamage = 2f;
        gameObject.GetComponent<PlayerAttackController>().heath = Mathf.Clamp(gameObject.GetComponent<PlayerAttackController>().heath, 0, maxHealth);
    }

    public void Die()
    {
        isDeath = true;
        if(weapon != null)
            weapon.SetActive(false);
        if(weaponOnShoulder != null)
            weaponOnShoulder.SetActive(false);
        gameObject.GetComponent<PlayerInput>().enabled = false;
        playerAnim.SetTrigger("isDeath"); 
        gameObject.GetComponent<PlayerAttackController>().enabled = false;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<NavMeshAgent>().enabled = false;
        }

        DOVirtual.DelayedCall(4f, () =>
        {
            UIManager.Instance.ReturnMenu();
        });
    }


    private void Equip()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded") && weapon != null)
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            weapon.SetActive(true);
            weaponOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else
        {
            weapon.SetActive(false);
            weaponOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
        }
    }

    public void TakeWeapon(GameObject weapon)
    {
        isEquipped = false;
        if(weapon == null) return;
        var currentWeapon = this.weapon;
        var currentOnShoulder = this.weaponOnShoulder;
        this.weapon = weapon;
        if (this.weapon != null)
        {
            this.weapon = this.weapons[this.weapon.GetComponent<Weapon>().id - 1].gameObject;
            weaponOnShoulder = this.weaponOnShoulders[this.weapon.GetComponent<Weapon>().id - 1].gameObject;
            if (currentWeapon != null && currentOnShoulder != null)
            {
                var newWeapon = Instantiate(weaponOnShouldersPrefabs[currentWeapon.GetComponent<Weapon>().id-1],gameObject.transform.position+Vector3.up*0.1f,weapon.transform.rotation);
                newWeapon.gameObject.layer = LayerMask.NameToLayer("Weapon");
                newWeapon.SetActive(true);
                currentWeapon.SetActive(false);
                currentOnShoulder.SetActive(false);
            }
            weaponOnShoulder.SetActive(true);
            this.weapon.SetActive(false);
            
            weapon.gameObject.layer = LayerMask.NameToLayer("Default");
            UpdateStatus.Instance.OnUpdateWeapon(this.weapon.GetComponent<Weapon>().id - 1);
            weapon.SetActive(false);
            
        }
    }

    public void Equipped()
    {
        isEquipping = false;
    }

    private void Block()
    {
        if (Input.GetKey(KeyCode.Mouse1) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Block", true);
            isBlocking = true;
        }
        else
        {
            playerAnim.SetBool("Block", false);
            isBlocking = false;
        }
    }

    public void Kick()
    {
        if (Input.GetKey(KeyCode.K) && playerAnim.GetBool("Grounded"))
        {
            playerAnim.SetBool("Kick", true);
            isKicking = true;
        }
        else
        {
            playerAnim.SetBool("Kick", false);
            isKicking = false;
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && playerAnim.GetBool("Grounded") && timeSinceAttack > 0.8f)
        {
            var target = GetComponent<PlayerAttackController>().Target;

            // 👉 Nếu có target thì xoay về phía target
            if (target != null)
            {
                Vector3 dir = (target.gameObject.transform.position - transform.position).normalized;
                dir.y = 0; 

                if (dir != Vector3.zero)
                {
                    Quaternion lookRot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 0.7f);
                }
            }

            // Combo đánh
            currentAttack++;
            isAttacking = true;

            if (currentAttack > 3)
                currentAttack = 1;

            // Reset combo nếu quá chậm
            if (timeSinceAttack > 1.0f)
                currentAttack = 1;

            // Animation Attack1, Attack2, Attack3
            playerAnim.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0;
        }
    }
    

    //This will be used at animation event
    public void ResetAttack()
    {
        isAttacking = false;
    } 
}   
