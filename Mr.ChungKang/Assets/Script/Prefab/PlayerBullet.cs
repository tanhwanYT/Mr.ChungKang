using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public RuntimeAnimatorController animator;
    public float damage = 5f;

    private void Start()
    {
        GetComponent<Animator>().runtimeAnimatorController = animator;

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(gameObject);        
        }
    }
}
