using UnityEngine;

public class Player : MonoBehaviour
{

    public float speed = 6f;
    bool iswall = false;
    bool isGround = false;
    public int jumpcount = 2;
    public float jump = 12f;
    public float jumpPower = 0.025f;
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

    }

    void Move()//좌우 이동
    {

        iswall = wall.GetComponent<iswall>().wallreach;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (iswall == false)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);//벽에 안 닿았을 때만 이동가능
            }
            transform.localScale = new Vector3(-1, 1, 1);//방향전환
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (iswall == false)
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            transform.localScale = new Vector3(1, 1, 1);
        }

    }
    void Jump()
    {


        if (Input.GetKeyDown(KeyCode.C))
        {

            isGround = Ground.GetComponent<isGround>().Groundreach;//isGround 정보 받아오기
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
                if (jumpTime > 0.25f) //0.5f는 0.5초간 누를 수 있음
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
                //Debug.Log("땅닿음");
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
                //점프를 하지 않고 떨어졌을 때 점프키를 누르면 늦게떨어지는 것을 방지
            }

        }

    }


}
