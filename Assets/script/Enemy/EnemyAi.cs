using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public int hp;
    public int attackDamage;
    public float EnemySpeed;
    public float Hit_rage;//넉백하는 거리
    public GameObject EnemyMovement;
    public GameObject AttackReaction;
    public GameObject EnemyHitBox;
    GameObject Player;
    Vector3 Playerpos;
    int nextMove = 0;
    bool detect = false;
    bool EAttack = false;
    public float rage = 0f;
    bool Hit_left = false;
    bool Hit_right = false;
    public Rigidbody2D rigid;


    public void first()
    {
        rigid = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player");
        Invoke("Think", 1f);
    }
    public void Think()
    {
        nextMove = Random.Range(-1, 2);//(최솟값, 최댓값 +1) 최댓값은 안나옴(올림해서그런듯)
        Invoke("Think", 1f);
    }
    public void EnemyMove()
    {
        Hit_left = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_left;
        Hit_right = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_right;

        EAttack = AttackReaction.GetComponent<AttackReaction>().EnemyAttack;
        detect = EnemyMovement.GetComponent<EnemyMovement>().Movement;//이렇게 계속 다른컴포넌트가져오는건 최적화에서 애바일듯

        if (detect == false)
        {
            transform.Translate(EnemySpeed * nextMove * Time.deltaTime, 0, 0);
            if (nextMove < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else if (nextMove > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else
            {

            }
        }

        else if (detect == true && EAttack == false)
        {
            Playerpos =  Player.transform.position;
            rage = Playerpos.x - transform.position.x;
            if (rage < 0f)
            {
                transform.Translate(-EnemySpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector2(-1, 1);
            }
            else if (rage > 0f)
            {
                transform.Translate(EnemySpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector2(1, 1);
            }
        }
    }
    public void Hit()
    {
        if(Hit_left == true)//적보다 오른쪽에 있을 때 맞음
        {
            rigid.AddForce(Vector2.left * Hit_rage * Time.deltaTime, ForceMode2D.Impulse);
  
        }
        else if(Hit_right == true)//적보다 왼쪽에 있을 때 맞음
        {
            rigid.AddForce(Vector2.right * Hit_rage * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    public void EnemyDeath()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void EnemyFrame()
    {
        EnemyMove();
        Hit();
        EnemyDeath();
    }
}
