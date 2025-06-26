using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 5f;

    private void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
