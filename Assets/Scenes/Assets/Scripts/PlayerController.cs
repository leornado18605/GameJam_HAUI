using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Third Person Controller References
    [SerializeField]
    private Animator playerAnim;


    //Equip-Unequip parameters
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private GameObject swordOnShoulder;
    public bool isEquipping;
    public bool isEquipped;


    //Blocking Parameters
    public bool isBlocking;

    //Kick Parameters
    public bool isKicking;

    //Attack Parameters
    public bool isAttacking;
    private float timeSinceAttack;
    public int currentAttack = 0;

    


    private void Update()
    {
        timeSinceAttack += Time.deltaTime;

        Attack();


        Equip();
        Block();
        Kick();
    }

    private void Equip()
    {
        if (Input.GetKeyDown(KeyCode.R) && playerAnim.GetBool("Grounded"))
        {
            isEquipping = true;
            playerAnim.SetTrigger("Equip");
        }
    }

    public void ActiveWeapon()
    {
        if (!isEquipped)
        {
            sword.SetActive(true);
            swordOnShoulder.SetActive(false);
            isEquipped = !isEquipped;
        }
        else
        {
            sword.SetActive(false);
            swordOnShoulder.SetActive(true);
            isEquipped = !isEquipped;
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
        if (Input.GetKey(KeyCode.LeftControl) && playerAnim.GetBool("Grounded"))
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
