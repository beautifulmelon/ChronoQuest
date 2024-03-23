using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float EnemySpeed;
    public float Hit_rage;//�˹��ϴ� �Ÿ�
    public GameObject EnemyMovement;
    public GameObject AttackReaction;
    public GameObject EnemyHitBox;
    public Transform Playerpos;
    int nextMove = 0;
    bool detect = false;
    bool EAttack = false;
    public float rage = 0f;
    bool Hit_left = false;
    bool Hit_right = false;
    public Rigidbody2D rigid;
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 1f);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        EnemyMove();
        Hit();
        
    }
    public void EnemyMove()
    {
        Hit_left = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_left;
        Hit_right = EnemyHitBox.GetComponent<EnemyHitBox>().Hit_right;

        EAttack = AttackReaction.GetComponent<AttackReaction>().EnemyAttack;
        detect = EnemyMovement.GetComponent<EnemyMovement>().Movement;//�̷��� ��� �ٸ�������Ʈ�������°� ����ȭ���� �ֹ��ϵ�

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
        }

        else if (detect == true && EAttack == false)
        {
            rage = Playerpos.position.x - transform.position.x;
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
    public void Think()
    {
        nextMove = Random.Range(-1, 2);//(�ּڰ�, �ִ� +1) �ִ��� �ȳ���(�ø��ؼ��׷���)
        Invoke("Think", 1f);
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
    
}
