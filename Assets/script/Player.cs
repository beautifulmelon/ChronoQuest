using UnityEngine;

public class Player : MonoBehaviour
{    
    public float speed = 6f;//이동속도
    public float dashSpeed = 8f;//대쉬거리

    public int jumpcount = 0;//점프횟수
    public float jump = 17f;//점프 힘
    public float jumpPower = 0.05f;//쭉 눌렀을 때 더 띄워지는 값

    public GameObject wall;
    public GameObject Ground;
    public GameObject Parrying;
    public GameObject PerfectParrying;
    float ParryingCount = 0.5f;//완벽한 패링 가능 시간 밑에 174줄에 있는 것도 바꿔야 함 똑같은 값으로

    public Transform parent;//prefab부모지정
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

    void Move()//좌우 이동
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
                transform.Translate(-speed * Time.deltaTime, 0, 0);//벽에 안 닿았을 때만 이동가능
            }
            transform.localScale = new Vector3(-1, 1, 1);//방향전환
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
            isGround = Ground.GetComponent<isGround>().Groundreach;//isGround 정보 받아오기
            if (jumpcount < 2)
            {

                rigid.velocity = new Vector2(rigid.velocity.x, 0);
                rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프   
                isjump = true;
                jumpcount++;

            }

        }
        if (Input.GetKey(KeyCode.C))
        {

            if (isjump == true)
            {
                rigid.AddForce(Vector2.up * jumpPower * Time.deltaTime, ForceMode2D.Impulse);//그 이후 쭉 눌렀을 때 증가하는 점프량
                jumpTime = jumpTime + Time.deltaTime;
                if (jumpTime > 0.25f) //0.25f는 0.25초간 누를 수 있음
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
            //여기에 구르기 시 무적상태 넣기
        }


    }
    void defense()
    {//막기 중에는 점프 밑 구르기 X 
        if (Input.GetKeyDown(KeyCode.X))
        {
            isparrying = true;
            speed = speed - 3f;//막기 시 감속

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
                Debug.Log("가보자가보자~");
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
            ParryingCount = 0.5f;//이 값을 바꿔야 함
            stop = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("볃");
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
                //점프를 하지 않고 떨어졌을 때 점프키를 누르면 늦게떨어지는 것을 방지
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }

        }

    }

    
}
