using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;
    public float maxHP;
    public float currentHP;
    public float moveSpeed;

    public abstract void TakeDamage(float amount);
    public abstract void Die();
}
