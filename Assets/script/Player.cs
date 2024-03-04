using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 6f;
    public int jumpcount = 2;
    public float jump = 17f;
    public float jumpPower = 0.05f;
    float jumpTime = 0f;
    bool isjump = false;

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
        Debug.Log(jumpTime);


    }

    void Move()//�¿� �̵�
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
        }

    }
    void Jump()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
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

                if (jumpTime > 0.5f) //0.5f�� 0.5�ʰ� ���� �� ����
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
    private void OnCollisionEnter2D(Collision2D collision)
    {

        jumpTime = 0f;
        isjump = false;
        jumpcount = 2;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isjump == false)//������ ���� �ʰ� �������� �� ����Ű�� ������ �ʰԶ������� ���� ����
        {
            jumpTime = 5f;
            jumpcount--;
        }


    }

}
