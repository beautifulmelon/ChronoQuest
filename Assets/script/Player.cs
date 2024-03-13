using UnityEngine;

public class Player : MonoBehaviour
{    
    public float speed = 6f;//�̵��ӵ�
    public float dashSpeed = 8f;//�뽬�Ÿ�

    public int jumpcount = 2;//����Ƚ��
    public float jump = 17f;//���� ��
    public float jumpPower = 0.05f;//�� ������ �� �� ������� ��

    public GameObject wall;
    public GameObject Ground;
    public GameObject sword;
    float attackTime = 0.2f;//���ݹ��������ð� 
    float attackcultime = 1f;//���� �� ��Ÿ��
    bool isattack = false;
    bool attackOn = true;
    public Transform parent;//prefab�θ�����
    GameObject attackManager;
    bool iswall = false;
    bool isGround = false;
    float jumpTime = 0f;
    bool isjump = false;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();


    }
    void Update()
    {

        Move();
        Jump();
        avoid();
        attack();
        if(iswall == true && isGround == false)
        {
            //��Ÿ��
            if(rigid.velocity.y < -0.5f)//0.5�� maxspeed
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -0.5f);
            }
        }
    }
    private void FixedUpdate()
    {
        
    }

    void Move()//�¿� �̵�
    {
        iswall = wall.GetComponent<iswall>().wallreach;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {

            if (iswall == false)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);//���� �� ����� ���� �̵�����
            }
            transform.localScale = new Vector2(-1, 1);//������ȯ
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

            if (iswall == false)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            transform.localScale = new Vector2(1, 1);
        }

        if (isGround == true)
        {
            jumpcount = 2;
        }


    }

    void Jump()
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (Input.GetKeyDown(KeyCode.C))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);

            if (jumpcount > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//����   
                isjump = true;
                jumpcount--;


            }

        }
        if (Input.GetKey(KeyCode.C))
        {

            if (isjump == true)
            {
                rigid.AddForce(Vector2.up * jumpPower * Time.deltaTime, ForceMode2D.Impulse);//�� ���� �� ������ �� �����ϴ� ������
                jumpTime = jumpTime + Time.deltaTime;
                if (jumpTime > 0.25f) //0.25f�� 0.25�ʰ� ���� �� ����
                {
                    isjump = false;
                }

            }
        }
        if (Input.GetKeyUp(KeyCode.C))//������ ��¦�� ������ ���� �� �ٽ� ������ ���� �� �� �ִ� ���� ����
        {
            isjump = false;

        }

        if (isGround == true)
        {
            jumpTime = 0f;
            jumpcount = 2;

        }
        if (isGround == false && isjump == false)
        {
            //jumpTime = 5f;
            if (jumpcount == 2)
            {
                jumpcount = 1;
            }
            //������ ���� �ʰ� �������� �� ����Ű�� ������ 2�������Ǵ°� ����
            
        }

    }
    void avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            if(iswall == false)
            {
                //���⿡ ������ ��Ǹ� �ֱ�
                if (transform.localScale.x == -1)
                {
                    rigid.velocity = new Vector2(-dashSpeed, rigid.velocity.y);


                }
                else if (transform.localScale.x == 1)
                {
                    rigid.velocity = new Vector2(dashSpeed, rigid.velocity.y);
                    //rigid.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);

                }
            }


            

            //�ִϸ��̼����� ������ �� ���� ���� �ֱ�(������ ����϶� ����) + ������ ��Ÿ�� + ������ ����� �� ������ȯ x
        }



    }
    void attack()
    {

        if (Input.GetKeyDown(KeyCode.X) && attackOn == true)
        {
            isattack = true;
            attackOn = false;
            attackManager = Instantiate(sword, parent);

        }
        if(attackOn == false)
        {
            attackcultime = attackcultime - Time.deltaTime;
            if (attackcultime < 0)
            {
                attackOn = true;
                attackcultime = 1f;//�� ���� ó�� ���� �������� �� ����ߴ� ���̶� ���ƾ� ��
            }
        }

        if (isattack == true)
        {
            attackTime = attackTime - Time.deltaTime;
            
            if(attackTime < 0)
            {
                Destroy(attackManager);
                attackTime = 0.2f;//�� ���� ó�� ���� �������� �� ����ߴ� ���̶� ���ƾ� ��
                isattack = false;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)//�ڽ� �� ���� ��� �ݶ��̴����� ����
    {

    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    
}
