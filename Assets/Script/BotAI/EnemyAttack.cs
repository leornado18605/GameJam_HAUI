using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float health = 100f;
    public float damage = 10f;

    public void GetDamage(float damage)
    {
        Debug.Log(damage);
        health -= damage;
        SoundManager.Instance.PlaySFX(SoundManager.Instance.hitClip);
        gameObject.GetComponent<AIController>().Hit();
        if (health <= 0)
        {
            gameObject.GetComponent<AIController>().Die();
            Destroy(gameObject,4f);
        }
    }

    public void TakeDamage()
    {
        GetComponent<AIController>().TakeDamage(damage);
    }
    
}
