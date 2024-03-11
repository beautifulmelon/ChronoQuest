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
    public GameObject sword;
    float attackTime = 0.2f;//공격범위생성시간 
    float attackcultime = 1f;//공격 후 쿨타임
    bool isattack = false;
    bool attackOn = true;
    public Transform parent;//prefab부모지정
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


        if (Input.GetKeyDown(KeyCode.C) && isparrying == false)
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
            //점프를 하지 않고 떨어졌을 때 점프키를 누르면 늦게떨어지는 것을 방지
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
            //여기에 구르기 시 무적상태 넣기
        }


    }
    void attack()
    {//막기 중에는 점프 밑 구르기 X 

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
                attackcultime = 1f;//이 값과 처음 변수 선언했을 때 사용했던 값이랑 같아야 함
            }
        }

        if (isattack == true)
        {
            attackTime = attackTime - Time.deltaTime;
            
            if(attackTime < 0)
            {
                Destroy(attackManager);
                attackTime = 0.2f;//이 값과 처음 변수 선언했을 때 사용했던 값이랑 같아야 함
                isattack = false;
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)//자식 및 본인 모든 콜라이더에게 적용
    {

        Debug.Log("아무거나 닿음");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    
}
