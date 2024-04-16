using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    float rage;
    public GameObject Enemy;
    public bool Hit_left;
    public bool Hit_right;
    public GameObject Player;
    int player_atk;
    //iswall = wall.GetComponent<iswall>().wallreach;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        player_atk = Player.GetComponent<Player>().player_atk;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "sword")
        {
            rage = Enemy.GetComponent<EnemyAi>().rage;
            if(rage < 0)
            {
                Hit_right = true;
                Invoke("INVOKE_Hit_right", 0.2f);
            }
            else if(rage > 0)
            {
                Hit_left = true;
                Invoke("INVOKE_Hit_left", 0.2f);
            }
            //¾Æ¾æ
            Enemy.GetComponent<EnemyAi>().hp = Enemy.GetComponent<EnemyAi>().hp - player_atk;
        }
    }
    void INVOKE_Hit_right()
    {
        Hit_right = false;
    }
    void INVOKE_Hit_left()
    {
        Hit_left = false;
    }

}
