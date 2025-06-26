using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/AI/ChasePlayerAI")]
public class ChasePlayerAI : EnemyAI
{
    public Vector2 detectionBoxSize = new Vector2(10f, 10f);
    public LayerMask playerLayer;

    public override void Execute(Enemy enemy)
    {
        if (!IsPlayerInSight(enemy)) return;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(enemy.transform.position, player.transform.position);

            Vector2 direction = (player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += (Vector3)direction * enemy.moveSpeed * Time.deltaTime;

            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.flipX = direction.x < 0;

            if (distance > enemy.attackRange)
            {
                enemy.transform.position += (Vector3)direction * enemy.moveSpeed * Time.deltaTime;
            }
            else
            {
                TryShoot(enemy, direction);
            }
        }
    }
    private bool IsPlayerInSight(Enemy enemy)
    {
        Vector2 center = enemy.transform.position;
        Collider2D hit = Physics2D.OverlapBox(center, detectionBoxSize, 0, playerLayer);
        return hit != null && hit.CompareTag("Player");
    }

    private void TryShoot(Enemy enemy, Vector2 direction)
    {
        if (Time.time - enemy.lastFireTime < enemy.fireCooldown)
            return;

        enemy.lastFireTime = Time.time;

        if (enemy.bulletPrefab != null && enemy.firePoint != null)
        {
            GameObject bullet = GameObject.Instantiate(enemy.bulletPrefab, enemy.firePoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = direction.normalized * 10f;
        }
    }
}
