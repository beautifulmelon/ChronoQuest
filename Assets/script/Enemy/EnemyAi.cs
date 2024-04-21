using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public int hp;
    public float EnemySpeed;
    public float Hit_rage;//�˹��ϴ� �Ÿ�
    public GameObject EnemyMovement;
    public GameObject AttackReaction;
    public GameObject EnemyHitBox;
    public Transform Playerpos;
    public int nextMove = 0;
    bool detect = false;
    public bool EAttack = false;
    public float rage = 0f;
    bool Hit_left = false;
    bool Hit_right = false;
    public Rigidbody2D rigid;
    public bool attackstop;

    public void first()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 1f);
    }
    public void Think()
    {
        nextMove = Random.Range(-1, 2);//(�ּڰ�, �ִ� +1) �ִ��� �ȳ���(�ø��ؼ��׷���)
        Invoke("Think", 1f);
    }
    public void EnemyMove()
    {
        Hit_left = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_left;
        Hit_right = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_right;

        EAttack = AttackReaction.GetComponent<AttackReaction>().EnemyAttack;
        detect = EnemyMovement.GetComponent<EnemyMovement>().Movement;//�̷��� ��� �ٸ�������Ʈ�������°� ����ȭ���� �ֹ��ϵ�
        if(EAttack && !attackstop)
        {
            StartCoroutine(Attackstop());
        }

        if (detect == false)
        {
            //transform.Translate(EnemySpeed * nextMove * Time.deltaTime, 0, 0);
            if (nextMove < 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (nextMove > 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
        }

        else if (detect == true && EAttack == false && !attackstop)
        {
            rage = Playerpos.position.x - transform.position.x;
            if (rage < 0f)
            {
                //transform.Translate(-EnemySpeed * Time.deltaTime, 0, 0);
                nextMove = 1;
                transform.localScale = new Vector2(-1, 1);
            }
            else if (rage > 0f)
            {
                //transform.Translate(EnemySpeed * Time.deltaTime, 0, 0);
                nextMove = -1;
                transform.localScale = new Vector2(1, 1);
            }
        }
        else { nextMove = 0; }
    }
    public void Hit()
    {
        if(Hit_left == true)//������ �����ʿ� ���� �� ����
        {
            rigid.AddForce(Vector2.left * Hit_rage * Time.deltaTime, ForceMode2D.Impulse);
  
        }
        else if(Hit_right == true)//������ ���ʿ� ���� �� ����
        {
            rigid.AddForce(Vector2.right * Hit_rage * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
    /*
    public void EnemyDeath()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject, 3f);
        }
    }
    */
    public void EnemyFrame()
    {
        EnemyMove();
        Hit();
        //EnemyDeath();
        Move();
    }
    void Move()//�¿� �̵�
    {
        float targetSpeed;
        if (nextMove == 1)
        {
            targetSpeed = -EnemySpeed;
        }
        else if (nextMove == -1)
        {
            targetSpeed = EnemySpeed;
        }
        else
        {
            targetSpeed = 0;
        }
        float accelRate;
        targetSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, 1);
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? 10 : 10;
        
        float speedDif = targetSpeed - rigid.velocity.x;
        float movement = speedDif * accelRate;
        rigid.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    IEnumerator Attackstop()
    {
        attackstop = true;
        yield return new WaitForSeconds(1.6f);
        attackstop = false;
    }
}
