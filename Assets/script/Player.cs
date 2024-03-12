using UnityEngine;

public class Player : MonoBehaviour
{    
    public float speed = 6f;//이동속도
    public float dashSpeed = 8f;//대쉬거리

    public int jumpcount = 2;//점프횟수
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
            //벽타기
            if(rigid.velocity.y < -0.5f)//0.5는 maxspeed
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -0.5f);
            }
        }
    }
    private void FixedUpdate()
    {
        
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

            if (iswall == false)
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);//벽에 안 닿았을 때만 이동가능
            }
            transform.localScale = new Vector2(-1, 1);//방향전환
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
                rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프   
                isjump = true;
                jumpcount--;


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
            //점프를 하지 않고 떨어졌을 때 점프키를 누르면 2번점프되는걸 방지
            
        }

    }
    void avoid()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            if(iswall == false)
            {
                //여기에 구르기 모션만 넣기
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


            

            //애니메이션으로 굴렀을 때 무적 상태 넣기(구르기 모션일때 무적) + 구르기 쿨타임 + 구르기 모션일 때 방향전환 x
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

    }
    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    
}
