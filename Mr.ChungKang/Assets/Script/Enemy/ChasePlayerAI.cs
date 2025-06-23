using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/AI/ChasePlayerAI")]
public class ChasePlayerAI : EnemyAI
{
    public override void Execute(Enemy enemy)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector2 dir = (player.transform.position - enemy.transform.position).normalized;

            Vector2 direction = (player.transform.position - enemy.transform.position).normalized;
            enemy.transform.position += (Vector3)direction * enemy.moveSpeed * Time.deltaTime;

            SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.flipX = dir.x < 0;
        }
    }
}
