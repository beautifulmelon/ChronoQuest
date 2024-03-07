using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 6f;//�̵��ӵ�
    bool isleft = false;
    bool isright = false;
    public float dashSpeed = 8f;//�뽬�Ÿ�
    bool iswall = false;
    bool isGround = false;
    public int jumpcount = 2;//����Ƚ��
    public float jump = 17f;//���� ��
    public float jumpPower = 0.05f;//�� ������ �� �� ������� ��
    float jumpTime = 0f;
    bool isjump = false;
    public GameObject wall;
    public GameObject Ground;
    Rigidbody2D rigid;
    void Awake()
    {

        rigid = GetComponent<Rigidbody2D>();

    }
    void Start()
    {

    }

    void Update()
    {

        Move();
        Jump();
        avoid();

    }

    void Move()//�¿� �̵�
    {
        iswall = wall.GetComponent<iswall>().wallreach;
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
            isright = true;
        }

    }
    void Jump()
    {


        if (Input.GetKeyDown(KeyCode.C))
        {
            isGround = Ground.GetComponent<isGround>().Groundreach;//isGround ���� �޾ƿ���
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
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//�� ���� �� ������ �� �����ϴ� ������
                jumpTime = jumpTime + Time.deltaTime;
                if (jumpTime > 0.25f) //0.5f�� 0.5�ʰ� ���� �� ����
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
        if (isGround == true && Input.GetKeyDown(KeyCode.Space))
        {
            if (isleft == true)
            {
                rigid.AddForce(Vector2.left * dashSpeed, ForceMode2D.Impulse);

            }
            else if (isright == true)
            {
                rigid.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);

            }

        }
        if (rigid.velocity.x > 0)
        {
            //���⿡ ������ �� �������� �ֱ�
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (collision.gameObject.tag == "Ground")
        {
            if (isGround == true)
            {
                jumpTime = 0f;
                isjump = false;
                jumpcount = 2;
                //Debug.Log("������");
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
                jumpcount--;
                //������ ���� �ʰ� �������� �� ����Ű�� ������ �ʰԶ������� ���� ����
            }

        }

    }


}
