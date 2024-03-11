using UnityEngine;

public class Player : MonoBehaviour
{    
    public float speed = 6f;//�̵��ӵ�
    public float dashSpeed = 8f;//�뽬�Ÿ�

    public int jumpcount = 0;//����Ƚ��
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
    bool isparrying = false;
    bool isleft = false;
    bool isright = false;
    bool iswall = false;
    bool isGround = false;
    float jumpTime = 0f;
    bool isjump = false;
    Rigidbody2D rigid;
    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();


    }
    void Update()
    {

        Move();
        Jump();
        avoid();
        attack();

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
            isleft = true;
            isright = false;

            if (iswall == false)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);//���� �� ����� ���� �̵�����
            }
            transform.localScale = new Vector3(-1, 1, 1);//������ȯ
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            isleft = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            isright = true;
            isleft = false;
            if (iswall == false)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            isright = false;
        }
        if (isGround == true)
        {
            jumpcount = 1;
        }


    }
    void Jump()
    {


        if (Input.GetKeyDown(KeyCode.C) && isparrying == false)
        {
            isGround = Ground.GetComponent<isGround>().Groundreach;//isGround ���� �޾ƿ���
            if (jumpcount < 2)
            {

                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//����   
                isjump = true;
                jumpcount++;

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
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (isGround == true)
        {
            jumpTime = 0f;
            isjump = false;
            jumpcount = 0;
        }
        if (isGround == false && isjump == false)
        {
            jumpTime = 5f;
            if (jumpcount == 0)
            {
                jumpcount = 1;
            }
            //������ ���� �ʰ� �������� �� ����Ű�� ������ �ʰԶ������� ���� ����
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

    }
    void avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true && isparrying == false)
        {
            if (isleft == true)
            {
                rigid.velocity = new Vector2 (-dashSpeed, rigid.velocity.y);
                //rigid.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);

            }
            else if (isright == true)
            {
                rigid.velocity = new Vector2(dashSpeed, rigid.velocity.y);
                //rigid.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);

            }

        }
        if (rigid.velocity.x > 0)
        {
            //���⿡ ������ �� �������� �ֱ�
        }


    }
    void attack()
    {//���� �߿��� ���� �� ������ X 

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

        Debug.Log("�ƹ��ų� ����");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    
}
