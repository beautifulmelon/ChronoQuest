using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject Player;
    Rigidbody2D rigid;
    float HitPushForce;
    int hp;
    // Start is called before the first frame update
    void Start()
    {
        rigid = Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyAttack")
        {
            if (transform.position.x < other.gameObject.transform.position.x)
            {
                HitPushForce = Player.GetComponent<Player>().HitPushForce;
                rigid.velocity = new Vector2(-HitPushForce, rigid.velocity.y);
            }
            else if (transform.position.x > other.gameObject.transform.position.x)
            {
                rigid.velocity = new Vector2(HitPushForce, rigid.velocity.y);
            }
            Player.GetComponent<Player>().hp = Player.GetComponent<Player>().hp -1;
            

        }
    }
}
