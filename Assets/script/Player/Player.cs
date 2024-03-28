using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 조작감 설정")]
    public float speed = 6f;//이동속도
    public float speedair; //점프했을 때 이동속도
    public float dashSpeed = 8f;//대쉬거리
    public int hp;
    public float runAccelAmount = 4;
    public float runDeccelAmount = 4;
    public float accelInAir = 1.5f;
    public float deccelInAir = 1.5f;

    private int jumpcount = 2;//점프횟수
    public float jump = 17f;//점프 힘
    public float jump_wall;
    public float jumpPower = 0.05f;//쭉 눌렀을 때 더 띄워지는 값
    private bool jumpcut; //점프 중단
    private bool iswalljumping;
    public float maxfallspeed; //낙하 최대속도
    public float wallslidespeed;

    public float gravityscale = 9.8f;
    public float fallgravityscale = 2f;

    [Header("플레이어에 넣어줘야 할 것들")]
    public GameObject wall;
    public GameObject Ground;
    public GameObject sword;
    float attackcultime = 1f;//공격 후 쿨타임
    float attackTime = 0.2f;//공격범위생성시간 
    bool isattack = false;
    bool attackOn = true;
    public Transform parent;//prefab부모지정
    GameObject attackManager;
    bool iswall = false;
    bool isGround = false;
    float jumpTime = 0f;
    bool isjump = false;
    private bool iswalljump; //벽점프
    Rigidbody2D rigid;

    //쿨타임들
    [Header("쿨타임 설정")]
    public float cooltime_roll = 0.5f;
    public float cooltime_attack = 0.3f;
    public float cooltime_attack_air = 0.6f;

    private float rollcooltime = 0;
    private float attackcultime = 1f;

    public float HitPushForce;
    //애니메이션 ---------------------------------------------------------------------------------
    Animator animator;
    private string currentState;

    const string PLAYER_IDLE = "player_idle";
    const string PLAYER_RUN = "player_run";
    const string PLAYER_STOP = "player_stop";
    const string PLAYER_JUMP = "player_jump";
    const string PLAYER_FALL = "player_fall";
    const string PLAYER_LAND = "player_land";
    const string PLAYER_ATTACK1 = "player_attack1";
    const string PLAYER_ATTACK2 = "player_attack2";
    const string PLAYER_ATTACK3 = "player_attack3";
    const string PLAYER_ATTACK_UP = "player_attack_up";
    const string PLAYER_JUMP_ATTACK = "player_jump_attack";
    const string PLAYER_JUMP_ATTACK_UP = "player_jump_attack_up";
    const string PLAYER_JUMP_ATTACK_DOWN = "player_jump_attack_down";
    const string PLAYER_ROLL = "player_roll";
    const string PLAYER_WALLSLIDE = "player_wallslide";
    const string PLAYER_WALLGRAB = "player_wallgrab";
    private bool isattacking;
    private bool isjumpattacking;
    private bool islanding;
    private bool isfalling;
    private bool isstopping;
    private bool isrolling;
    private float attackformchange = 0;
    private int attackform = 1;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }
    void Update()
    {
        death();
        Move();
        Jump();
        avoid();
        attack();
        if(iswall == true && isGround == false && rigid.velocity.y < 0 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            ChangeAnimationState(PLAYER_WALLSLIDE); //애니메이션
            //벽타기
            if (rigid.velocity.y < -wallslidespeed)//0.5는 maxspeed
            {
                rigid.velocity = new Vector2(rigid.velocity.x, -wallslidespeed);
            }
        }
        else
        {
            rigid.velocity = new Vector2(rigid.velocity.x, Mathf.Max(rigid.velocity.y, maxfallspeed));
        }

        //이동 애니메이션
        if (!isrolling && !isattacking)
        {
            if (isGround && !isrolling)
            {
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
                {
                    ChangeAnimationState(PLAYER_RUN);
                }
                else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
                {
                    ChangeAnimationState(PLAYER_STOP);
                    CancelInvoke("StopComplete");
                    isstopping = true;
                    Invoke("StopComplete", animator.GetCurrentAnimatorStateInfo(0).length);
                }
                else if (isfalling)
                {
                    isfalling = false;
                    ChangeAnimationState(PLAYER_LAND);
                    CancelInvoke("LandComplete");
                    islanding = true;
                    Invoke("LandComplete", animator.GetCurrentAnimatorStateInfo(0).length);

                }
                else
                {
                    if (!isstopping && !islanding)
                    {
                        ChangeAnimationState(PLAYER_IDLE);
                    }
                }
            }
        }
    }

    void Move()//좌우 이동
    {
        if (!isrolling && !isattacking)
        {
            float targetSpeed;

            iswall = wall.GetComponent<iswall>().wallreach;

            if (Input.GetKey(KeyCode.LeftArrow) && !iswalljump)
            {
                if (isGround)
                    targetSpeed = -speed;
                else
                    targetSpeed = -speedair;
                
                transform.localScale = new Vector2(-1, 1);//방향전환
            }
            else if (Input.GetKey(KeyCode.RightArrow) && !iswalljump)
            {
                if (isGround)
                    targetSpeed = speed;
                else
                    targetSpeed = speedair;
                transform.localScale = new Vector2(1, 1);//방향전환
            }
            else
            {
                targetSpeed = 0;
            }

            //소스코드보고 짠 코드 이동
            #region
            float accelRate;
            if (isGround)
            {
                targetSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, 1);
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
            }
            else
            {
                targetSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, 1);
                accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
            }

            if(Mathf.Abs(rigid.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigid.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && !isGround)
            {
                accelRate = 0;
            }
            float speedDif = targetSpeed - rigid.velocity.x;
            float movement = speedDif * accelRate;
            rigid.AddForce(movement * Vector2.right, ForceMode2D.Force);
            #endregion

            //이전 코드
            /*
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

            
            */
        }


    }

    void Jump()
    {
        isGround = Ground.GetComponent<isGround>().Groundreach;
        if (Input.GetKeyDown(KeyCode.C) && !isrolling)
        {
            //rigid.velocity = new Vector2(0, rigid.velocity.y);
            
            if (jumpcount > 0)
            {
                if (iswall && !isGround && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) //벽 점프
                {
                    float force = jump;
                    if (rigid.velocity.y < 0)
                    {
                        force -= rigid.velocity.y;
                    }
                    Vector2 walljumpforce = new Vector2(- jump_wall * transform.localScale.x, force*0.9f);
                    rigid.AddForce(walljumpforce, ForceMode2D.Impulse);
                    isjump = true;
                    jumpcount--;
                    jumpcut = false;
                    iswalljump = true;
                    accelInAir = 0.3f;
                    deccelInAir = 0.3f;
                    /*
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프
                    rigid.AddForce(Vector2.left * jump * 0.5f * transform.localScale.x, ForceMode2D.Impulse); //옆으로 점프
                    isjump = true;
                    iswalljump = true;
                    jumpcount--;
                    */
                }
                else
                {
                    if (rigid.velocity.y > 0)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    }
                    //소스코드보고 짠 코드
                    #region
                    float force = jump;
                    if (rigid.velocity.y < 0)
                    {
                        force -= rigid.velocity.y;
                    }
                    rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                    isjump = true;
                    jumpcount--;
                    jumpcut = false;
                    #endregion
                    /*
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    rigid.AddForce(Vector2.up * jump, ForceMode2D.Impulse);//점프   
                    isjump = true;
                    jumpcount--;
                    */
                }
            }
        }
        /*
        if (Input.GetKey(KeyCode.C))
        {
            if (isjump == true)
            {

                rigid.AddForce(Vector2.up * jumpPower * Time.deltaTime, ForceMode2D.Impulse);//그 이후 쭉 눌렀을 때 증가하는 점프량
                jumpTime += Time.deltaTime;

                if (jumpTime > 0.25f) //0.25f는 0.25초간 누를 수 있음
                {
                    isjump = false;
                }

            }
        }
        */
        if (Input.GetKeyUp(KeyCode.C))//점프를 살짝만 누르고 땠을 때 다시 누르면 힘을 줄 수 있던 것을 방지
        {
            isjump = false;
            jumpcut = true;
        }

        if(jumpcut || rigid.velocity.y < 0)
        {
            rigid.gravityScale = gravityscale * fallgravityscale;
        }
        else
        {
            rigid.gravityScale = gravityscale;
        }

        if (isGround == true)
        {
            jumpTime = 0f;
            jumpcount = 2;
        }
        else if (iswall && ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))))
        {
            jumpTime = 0f;
            jumpcount = 1;
        }
        else if (iswalljump) //벽점프시 점프 1번만 가능
        {
            jumpcount = 0;
            iswalljump = false;
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

        //애니메이션
        if (rigid.velocity.y > 0 && !isGround && !isrolling && !isjumpattacking)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        if(rigid.velocity.y < 0 && !isGround && !(iswall && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) && !isrolling && !isjumpattacking)
        {
            isfalling = true;
            ChangeAnimationState(PLAYER_FALL);

            accelInAir = 1;
            deccelInAir = 1.1f;
        }
    }
    void avoid()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGround == true && rollcooltime <= 0)
        {
            //애니메이션
            ChangeAnimationState(PLAYER_ROLL);
            CancelInvoke("RollComplete");
            isrolling = true;
            isstopping = false; //이거때문에 딜레이 될 때가 있어서 
            Invoke("RollComplete", 0.4f);
            //Invoke("RollComplete", animator.GetCurrentAnimatorStateInfo(0).length / 4f);
            rollcooltime = cooltime_roll;
            if (iswall == false)
            {
                if (Input.GetKey(KeyCode.RightArrow))
                    transform.localScale = new Vector2(1, 1);
                else if (Input.GetKey(KeyCode.RightArrow))
                    transform.localScale = new Vector2(-1, 1);

                rigid.velocity = new Vector2(dashSpeed * transform.localScale.x, rigid.velocity.y);

                /*
                if (transform.localScale.x == -1)
                {
                    rigid.velocity = new Vector2(-dashSpeed, rigid.velocity.y);


                }
                else if (transform.localScale.x == 1)
                {
                    rigid.velocity = new Vector2(dashSpeed, rigid.velocity.y);
                    //rigid.AddForce(Vector2.right * dashSpeed, ForceMode2D.Impulse);

                }
                */
            }
            //애니메이션으로 굴렀을 때 무적 상태 넣기(구르기 모션일때 무적) + 구르기 쿨타임 + 구르기 모션일 때 방향전환 x
            //굴렀을 때 무적은 isrolling 값을 이용하면 될 듯 나머지 구현 완료 
        }
        else //구르기 쿨타임 돌리기
        {
            rollcooltime -= Time.deltaTime;
        }



    }
    void attack()
    {

        if(Input.GetKeyDown(KeyCode.X) && attackcultime <= 0 && !isrolling)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
                transform.localScale = new Vector2(-1, 1);

            if (isGround) //땅애서 공격할때
            {
                attackcultime = cooltime_attack;

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0, 0.25f, 0);
                    Quaternion newRotation = Quaternion.Euler(0, 0, 90);
                    attackManager = Instantiate(sword, newPosition, newRotation, parent.transform);
                    Destroy(attackManager, 0.1f);

                    attackformchange = 0; // 공격모션 123 초기화
                    //애니메이션
                    ChangeAnimationState(PLAYER_ATTACK_UP);
                    CancelInvoke("AttackComplete");
                    isattacking = true;
                    Invoke("AttackComplete", cooltime_attack - 0.02f);
                }

                else
                {

                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0.5f * transform.localScale.x, -0.2f, 0);
                    attackManager = Instantiate(sword, newPosition, Quaternion.identity, parent.transform);
                    Destroy(attackManager, 0.1f);

                    if (Input.GetKey(KeyCode.RightArrow))
                    {
                        rigid.velocity = Vector2.zero;
                        rigid.AddForce(Vector2.right * 5, ForceMode2D.Impulse);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rigid.velocity = Vector2.zero;
                        rigid.AddForce(Vector2.left * 5, ForceMode2D.Impulse);
                    }

                    //애니메이션
                    attackformchange = 0.5f;
                    if (attackform == 1)
                    {
                        ChangeAnimationState(PLAYER_ATTACK1);
                        attackform = 2;
                    }
                    else if (attackform == 2)
                    {
                        ChangeAnimationState(PLAYER_ATTACK2);
                        attackform = 3;
                    }
                    else if (attackform == 3)
                    {
                        ChangeAnimationState(PLAYER_ATTACK3);
                        attackform = 1;
                    }
                    CancelInvoke("AttackComplete");
                    isattacking = true;
                    Invoke("AttackComplete", 0.38f);
                    //Invoke("AttackComplete", animator.GetCurrentAnimatorStateInfo(0).length);
                }
            }
            else if(!isGround && !iswall)
            {
                attackcultime = cooltime_attack_air;
                attackformchange = 0; // 공격모션 123 초기화

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0, 0.5f, 0);
                    Quaternion newRotation = Quaternion.Euler(0, 0, 90);
                    attackManager = Instantiate(sword, newPosition, newRotation, parent.transform);
                    Destroy(attackManager, 0.1f);

                    ChangeAnimationState(PLAYER_JUMP_ATTACK_UP); //애니메이션
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0, -0.5f, 0);
                    Quaternion newRotation = Quaternion.Euler(0, 0, -90);
                    attackManager = Instantiate(sword, newPosition, newRotation, parent.transform);
                    Destroy(attackManager, 0.1f);

                    ChangeAnimationState(PLAYER_JUMP_ATTACK_DOWN); //애니메이션
                }
                else
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0.5f * transform.localScale.x, 0, 0);
                    attackManager = Instantiate(sword, newPosition, Quaternion.identity, parent.transform);
                    Destroy(attackManager, 0.1f);

                    ChangeAnimationState(PLAYER_JUMP_ATTACK);
                }

                //애니메이션
                CancelInvoke("JumpAttackComplete");
                isjumpattacking = true;
                Invoke("JumpAttackComplete", cooltime_attack_air - 0.02f);
            }
            

            //attackManager = Instantiate(sword, parent);
            //Destroy(attackManager, 0.1f);
        }
        else
        {
            attackcultime -= Time.deltaTime;
        }

        if(attackformchange >0)// 공격 모션 123 바꾸는 코드
        {
            attackformchange -= Time.deltaTime;
        }
        else
        {
            attackformchange = 0;
            attackform = 1;
        }
        /*
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
        //.
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
        */
    }
    void death()
    {
        if (hp == 0)
        {
            Destroy(this.gameObject);//사망
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)//자식 및 본인 모든 콜라이더에게 적용
    {

    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {


    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }
    void AttackComplete()
    {
        isattacking = false;
    }
    void JumpAttackComplete()
    {
        isjumpattacking = false;
    }
    void LandComplete()
    {
        islanding = false;
    }
    void StopComplete()
    {
        isstopping = false;
    }
    void RollComplete()
    {
        isrolling = false;
    }
}
