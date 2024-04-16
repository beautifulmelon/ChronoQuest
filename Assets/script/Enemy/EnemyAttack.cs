using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int attackDamage;
    GameObject Enemy;
    private void Awake()
    {
        Enemy = transform.parent.gameObject;
        attackDamage = Enemy.GetComponent<EnemyAi>().attackDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("sword"))
        {
            Destroy(gameObject);
        }

    }
}
