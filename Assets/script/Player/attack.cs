using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("����");
            //���̱�
        }
        if(collision.gameObject.tag == "EnemyAttack")
        {
            Player.instance.Parrying();
        }
    }
}
