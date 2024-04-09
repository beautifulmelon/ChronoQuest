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

            //�¾��� �� ��񵿾� �������̰� ����� 
            
       
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

            //�¾��� �� �ִϸ��̼� �ֱ�
            Player.GetComponent<Player>().hp = Player.GetComponent<Player>().hp - 1;//ü�� ���
            if (transform.position.x < other.gameObject.transform.position.x)
            {        
                rigid.velocity = new Vector2(-HitPushForce, rigid.velocity.y);
                isHit = true;
                Player.GetComponent<Player>().isHit = true;
                Invoke("Hit", 0.5f);//�� �¾��� �� ��������� ��
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
