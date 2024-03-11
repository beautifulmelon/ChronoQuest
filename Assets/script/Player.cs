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
    public GameObject Parrying;
    public GameObject PerfectParrying;
    float ParryingCount = 0.5f;//�Ϻ��� �и� ���� �ð� �ؿ� 174�ٿ� �ִ� �͵� �ٲ�� �� �Ȱ��� ������

    public Transform parent;//prefab�θ�����
    GameObject ParryingManager;
    bool isparrying = false;
    bool stop = false;
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
        defense();

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


        if (Input.GetKeyDown(KeyCode.C) && isparrying == false && rigid.velocity.x < 0.01f)
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
    void defense()
    {//���� �߿��� ���� �� ������ X 
        if (Input.GetKeyDown(KeyCode.X))
        {
            isparrying = true;
            speed = speed - 3f;//���� �� ����

        }
        if (Input.GetKey(KeyCode.X))
        {
            //Debug.Log(ParryingCount);
            ParryingCount = ParryingCount - Time.deltaTime;
            if (ParryingCount > 0 && stop == false)
            {
                ParryingManager = Instantiate(PerfectParrying, parent);
                stop = true;
            }
            else if (ParryingCount < 0 && stop == true)
            {
                Debug.Log("�����ڰ�����~");
                Destroy(ParryingManager);
                ParryingManager = Instantiate(Parrying, parent);
                stop = false;
            }


        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            Destroy(ParryingManager);
            isparrying = false;
            speed = speed + 3f;
            ParryingCount = 0.5f;//�� ���� �ٲ�� ��
            stop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("��");
            if (isGround == true)
            {
                jumpTime = 0f;
                isjump = false;
                jumpcount = 0;
            }

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (collision.gameObject.tag == "Ground")
        {
            if (isGround == false && isjump == false)
            {
                jumpTime = 5f;
                if(jumpcount == 0)
                {
                    jumpcount = 1;
                }
                //������ ���� �ʰ� �������� �� ����Ű�� ������ �ʰԶ������� ���� ����
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }

        }

    }

    
}
