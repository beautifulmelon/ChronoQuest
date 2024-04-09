using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public GameObject Player;
    Rigidbody2D rigid;
    float HitPushForce;
    bool isHit = false;
    int hp;

    void Start()
    {
        rigid = Player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

            //맞았을 때 잠깐동안 못움직이게 만들기 
            
       
    }
    void Hit()
    {
        isHit = false;
        Player.GetComponent<Player>().isHit = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyAttack" && !isHit)
        {
            HitPushForce = Player.GetComponent<Player>().HitPushForce;

            //맞았을 때 애니메이션 넣기
            Player.GetComponent<Player>().hp = Player.GetComponent<Player>().hp - 1;//체력 닳기
            if (transform.position.x < other.gameObject.transform.position.x)
            {        
                rigid.velocity = new Vector2(-HitPushForce, rigid.velocity.y);
                isHit = true;
                Player.GetComponent<Player>().isHit = true;
                Invoke("Hit", 0.5f);//이 맞았을 떄 못움직어야 함
            }
            else if (transform.position.x > other.gameObject.transform.position.x)
            {
                rigid.velocity = new Vector2(HitPushForce, rigid.velocity.y);
                isHit = true;
                Player.GetComponent<Player>().isHit = true;
                Invoke("Hit", 0.5f);
            }
            
            

        }
    }
}
