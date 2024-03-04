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

    void Move()//좌우 이동
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
                rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프   
                isjump = true;
                jumpcount--;

            }
        }
        if (Input.GetKey(KeyCode.C))
        {

            if (isjump == true)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);//그 이후 쭉 눌렀을 때 증가하는 점프량
                jumpTime = jumpTime + Time.deltaTime;

                if (jumpTime > 0.5f) //0.5f는 0.5초간 누를 수 있음
                {

                    isjump = false;
                }

            }
        }
        if (Input.GetKeyUp(KeyCode.C))//점프를 살짝만 누르고 땠을 때 다시 누르면 힘을 줄 수 있던 것을 방지
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
        if (isjump == false)//점프를 하지 않고 떨어졌을 때 점프키를 누르면 늦게떨어지는 것을 방지
        {
            jumpTime = 5f;
            jumpcount--;
        }


    }

}
