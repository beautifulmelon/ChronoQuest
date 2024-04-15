using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using System.ComponentModel.Design;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    [Header("플레이어 능력치")]
    public int player_hp = 100;
    public int player_tf = 100;
    public bool player_transformed = false;
    private bool istransfromCool;
    public bool player_dead = false;
    public int player_atk = 10;

    [Header("플레이어 조작감 설정")]
    public float speed = 6f;//이동속도
    public float speedair; //점프했을 때 이동속도
    public float dashSpeed = 8f;//대쉬거리
    public float runAccelAmount = 4;
    public float runDeccelAmount = 4;
    public float accelInAir = 1.5f;
    public float deccelInAir = 1.5f;

    public int jumpcount = 1;//점프횟수
    public float jump = 17f;//점프 힘
    public float jump_wall;
    public float jumpPower = 0.05f;//쭉 눌렀을 때 더 띄워지는 값
    private bool jumpcut; //점프 중단
    public float maxfallspeed; //낙하 최대속도
    public float wallslidespeed;
    public float attackTime = 0.1f;//공격범위생성시간 
    public float attackmoveforce = 10;

    public float gravityscale = 9.8f;
    public float fallgravityscale = 2f;

    public float knockbackforce = 10;
    public bool damaged;
    public float damagedTime = 2f;

    [Header("플레이어에 넣어줘야 할 것들")]
    public GameObject wall;
    public GameObject Ground;
    public GameObject sword;
    float attackcultime = 1f;//공격 후 쿨타임
    public Transform parent;//prefab부모지정
    public GameObject attackManager;
    public ShockWaveManager shockWaveManager;

    public bool isGround = false;
    bool isjump = false;
    public bool isHit = false;//맞았을 때 
    private bool iswalljump; //벽점프
    Rigidbody2D rigid;

    //쿨타임들
    [Header("쿨타임 설정")]
    public float cooltime_roll = 0.5f;
    public float cooltime_attack = 0.3f;
    public float cooltime_attack_air = 0.6f;
    public float transform_time = 3f;
    private float rollcooltime = 0;

    public float HitPushForce;

    [Header("플레이어 능력 얻었는지 확인")]
    public bool get_secondjump;
    private bool secondjumped;

    [Header("이것 저것")]
    public bool iswall = false;
    public bool parrying;
    private bool parryingAtkReady;
    private float parryingTimer;
    public bool rightleftAtk;
    public bool upAtk;
    public bool downAtk;
    public float parryingForce = 10;
    public Material[] mat = new Material[2];

    //애니메이션 ---------------------------------------------------------------------------------
    Animator animator;
    private string currentState;

    private string PLAYER_IDLE = "player_idle";
    private string PLAYER_RUN = "player_run";
    private string PLAYER_STOP = "player_stop";
    private string PLAYER_JUMP = "player_jump";
    private string PLAYER_FALL = "player_fall";
    private string PLAYER_LAND = "player_land";
    private string PLAYER_ATTACK1 = "player_attack1";
    private string PLAYER_ATTACK2 = "player_attack2";
    private string PLAYER_ATTACK3 = "player_attack3";
    private string PLAYER_ATTACK_UP = "player_attack_up";
    private string PLAYER_JUMP_ATTACK = "player_jump_attack";
    private string PLAYER_JUMP_ATTACK_UP = "player_jump_attack_up";
    private string PLAYER_JUMP_ATTACK_DOWN = "player_jump_attack_down";
    private string PLAYER_ROLL = "player_roll";
    private string PLAYER_WALLSLIDE = "player_wallslide";
    private string PLAYER_WALLGRAB = "player_wallgrab";

    private bool isattacking;
    private bool isjumpattacking;
    private bool islanding;
    private bool isfalling;
    private bool isstopping;
    private bool isrolling;
    private bool isdashing;
    private float attackformchange = 0;
    private int attackform = 1;

    public static Player instance;
    SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if(instance == null)
        {
            instance = this;
        }
    }
    void Update()
    {
        if (!player_dead)
        {
            Jump();
            avoid();
            attack();
            //플레이어 변신
            if (Input.GetKeyDown(KeyCode.S) && !istransfromCool)
            {
                Player_Transform();
                StartCoroutine(TransformCool());
            }


            //벽 슬라이드 애니메이션
            if (iswall == true && isGround == false && rigid.velocity.y < 0 && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
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
        
    }

    private void FixedUpdate()
    {
        if (!player_dead)
        {
            Move();
        }
    }

    void Move()//좌우 이동
    {
        float targetSpeed;
        if (Input.GetKey(KeyCode.LeftArrow) && !iswalljump && !isattacking && !isdashing && !isrolling)
        {
            if (isGround)
                targetSpeed = -speed;
            else
                targetSpeed = -speedair;

            transform.localScale = new Vector2(-1, 1);//방향전환
        }
        else if (Input.GetKey(KeyCode.RightArrow) && !iswalljump && !isattacking && !isdashing && !isrolling)
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
        if (isGround && !isrolling)
        {
            targetSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, 1);
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        }
        else
        {
            targetSpeed = Mathf.Lerp(rigid.velocity.x, targetSpeed, 1);
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
        }

        /*if (Mathf.Abs(rigid.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rigid.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && !isGround)
        {
            accelRate = 0;
        }*/
        float speedDif = targetSpeed - rigid.velocity.x;
        float movement = speedDif * accelRate;
        rigid.AddForce(movement * Vector2.right, ForceMode2D.Force);
        #endregion
    }

    void Jump()
    {
        if(Input.GetKeyDown(KeyCode.C) && !isrolling )
        {
            if(iswall && !isGround && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) //벽점프
            {
                float force = jump;
                if (rigid.velocity.y < 0)
                {
                    force -= rigid.velocity.y;
                }
                Vector2 walljumpforce = new Vector2(-jump_wall * transform.localScale.x, force);
                rigid.AddForce(walljumpforce, ForceMode2D.Impulse);
                isjump = true;
                jumpcount--;
                jumpcut = false;
                iswalljump = true;
                accelInAir = 0.2f;
                deccelInAir = 0.2f;
            }
            else if(isGround)
            {
                if (rigid.velocity.y > 0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                }
                float force = jump;
                if (rigid.velocity.y < 0)
                {
                    force -= rigid.velocity.y;
                }
                rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                isjump = true;
                jumpcount--;
                jumpcut = false;
            }
            else if (!secondjumped && get_secondjump) //2단 점프
            {
                secondjumped = true;

                if (rigid.velocity.y > 0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 0);
                }
                float force = jump;
                if (rigid.velocity.y < 0)
                {
                    force -= rigid.velocity.y;
                }
                rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                isjump = true;
                jumpcount--;
                jumpcut = false;
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.C) && !isrolling)
        {
            if (jumpcount > 0)
            {
                if (iswall && !isGround && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) //벽 점프
                {
                    float force = jump;
                    if (rigid.velocity.y < 0)
                    {
                        force -= rigid.velocity.y;
                    }
                    Vector2 walljumpforce = new Vector2(-jump_wall * transform.localScale.x, force);
                    rigid.AddForce(walljumpforce, ForceMode2D.Impulse);
                    isjump = true;
                    jumpcount--;
                    jumpcut = false;
                    iswalljump = true;
                    accelInAir = 0.2f;
                    deccelInAir = 0.2f;
                }
                else
                {
                    if (rigid.velocity.y > 0)
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, 0);
                    }
                    float force = jump;
                    if (rigid.velocity.y < 0)
                    {
                        force -= rigid.velocity.y;
                    }
                    rigid.AddForce(Vector2.up * force, ForceMode2D.Impulse);
                    isjump = true;
                    jumpcount--;
                    jumpcut = false;
                }
            }
        }
        */
        if (Input.GetKeyUp(KeyCode.C))//점프를 살짝만 누르고 땠을 때 다시 누르면 힘을 줄 수 있던 것을 방지
        {
            isjump = false;
            jumpcut = true;
        }

        if (jumpcut || rigid.velocity.y < 0)
        {
            rigid.gravityScale = gravityscale * fallgravityscale;
        }
        else
        {
            rigid.gravityScale = gravityscale;
        }

        if (isGround == true)
        {
            if (get_secondjump) { jumpcount = 2; }
            else { jumpcount = 1; }
            secondjumped = false;
        }
        else if (iswall && ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))))
        {
            jumpcount = 1;
        }
        else if (iswalljump) //벽점프시 점프 1번만 가능
        {
            if (get_secondjump) { jumpcount = 1; }
            else { jumpcount = 0; }
            iswalljump = false;
        }
        if (isGround == false && isjump == false)
        {
            if (jumpcount == 2)
            {
                jumpcount = 1;
            }
            //점프를 하지 않고 떨어졌을 때 점프키를 누르면 2번점프되는걸 방지
        }

        //애니메이션
        if (rigid.velocity.y > 0 && !isGround && !isrolling && !isjumpattacking && !isdashing)
        {
            ChangeAnimationState(PLAYER_JUMP);
        }
        if (rigid.velocity.y < 0 && !isGround && !(iswall && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))) && !isrolling && !isjumpattacking && !isdashing)
        {
            isfalling = true;
            ChangeAnimationState(PLAYER_FALL);

            accelInAir = 1;
            deccelInAir = 1.1f;
        }
    }
    void avoid()
    {
        if (Input.GetKeyDown(KeyCode.Z) && isGround == true && rollcooltime <= 0 && !player_transformed)
        {
            //애니메이션
            ChangeAnimationState(PLAYER_ROLL);
            CancelInvoke("RollComplete");
            isrolling = true;
            isstopping = false; //이거때문에 딜레이 될 때가 있어서 
            Invoke("RollComplete", 0.4f);
            rollcooltime = cooltime_roll;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && rollcooltime <= 0 && player_transformed)
        {
            //애니메이션
            ChangeAnimationState(PLAYER_ROLL);
            CancelInvoke("RollComplete");
            isrolling = true;
            isstopping = false; //이거때문에 딜레이 될 때가 있어서 
            Invoke("RollComplete", 0.2f);
            rollcooltime = cooltime_roll;
            StartCoroutine(Player2_Dash());
        }
        else //구르기 쿨타임 돌리기
        {
            rollcooltime -= Time.deltaTime;
        }

        if (isdashing)
        {
            if (iswall == false)
            {
                rigid.velocity = new Vector2(dashSpeed * 1.5f * transform.localScale.x, 0);
            }
            else { isdashing = false; }
        }

        if (isrolling)
        {
            if (iswall == false)
            {
                rigid.velocity = new Vector2(dashSpeed * transform.localScale.x, rigid.velocity.y);
            }
            else { isrolling = false; }
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
                    Destroy(attackManager, attackTime);
                    upAtk = true;

                    attackformchange = 0; // 공격모션 123 초기화
                    //애니메이션
                    ChangeAnimationState(PLAYER_ATTACK_UP);
                    CancelInvoke("AttackComplete");
                    isattacking = true;
                    Invoke("AttackComplete", cooltime_attack - 0.02f);
                    Invoke("AttackDestroy", 0.1f);
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
                        rigid.AddForce(Vector2.right * attackmoveforce, ForceMode2D.Impulse);
                    }
                    else if (Input.GetKey(KeyCode.LeftArrow))
                    {
                        rigid.velocity = Vector2.zero;
                        rigid.AddForce(Vector2.left * attackmoveforce, ForceMode2D.Impulse);
                    }
                    rightleftAtk = true;

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
                    Invoke("AttackDestroy", 0.1f);
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
                    upAtk = true;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0, -0.5f, 0);
                    Quaternion newRotation = Quaternion.Euler(0, 0, -90);
                    attackManager = Instantiate(sword, newPosition, newRotation, parent.transform);
                    Destroy(attackManager, 0.1f);

                    ChangeAnimationState(PLAYER_JUMP_ATTACK_DOWN); //애니메이션
                    downAtk = true;
                }
                else
                {
                    Vector3 parentPosition = parent.transform.position;
                    Vector3 newPosition = parentPosition + new Vector3(0.5f * transform.localScale.x, 0, 0);
                    attackManager = Instantiate(sword, newPosition, Quaternion.identity, parent.transform);
                    Destroy(attackManager, 0.1f);

                    ChangeAnimationState(PLAYER_JUMP_ATTACK);
                    rightleftAtk = true;
                }


                //애니메이션
                CancelInvoke("JumpAttackComplete");
                isjumpattacking = true;
                Invoke("JumpAttackComplete", cooltime_attack_air - 0.02f);
                Invoke("AttackDestroy", 0.1f);
            }
            //패링공격 효과
            if (parryingAtkReady)
            {
                Cam_Move.instance.HardAtkEffect();
                ChangeMaterial(0);
            }
            parrying = false;
        }
        else
        {
            attackcultime -= Time.deltaTime;
        }

        //패링
        if (parrying)
        {
            parryingTimer += Time.deltaTime;
            if(parryingTimer > 0.7f)
            {
                parrying = false;
                parryingAtkReady = false;
                parryingTimer = 0;
                ChangeMaterial(0);
            }   
            else if(parryingTimer > 0.6f)
            {
                parryingAtkReady = true;
                ChangeMaterial(1);
            }
        }
        else
        {
            parryingAtkReady = false;
            parryingTimer = 0;
        }

        if (attackformchange >0)// 공격 모션 123 바꾸는 코드
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
    public void Knock_Back(Vector2 direction)
    {
        //rigid.AddForce(direction.normalized * knockbackforce, ForceMode2D.Impulse);
        rigid.velocity = direction.normalized * knockbackforce;
    }
    public void Damaged(int enemyatk)
    {
        player_hp -= enemyatk;
        damaged = true;
        Invoke("DamagedEnd", damagedTime);
        spriteRenderer.color = new Color(1,1,1,0.5f);
    }
    public void Parrying()
    {
        parrying = true;
        rigid.velocity = Vector2.zero;
        if (upAtk) { rigid.AddForce(Vector2.down * parryingForce /4, ForceMode2D.Impulse); }
        else if(downAtk) { rigid.AddForce(Vector2.up * parryingForce/1.5f, ForceMode2D.Impulse); }
        else { rigid.AddForce(Vector2.left * transform.localScale.x * parryingForce, ForceMode2D.Impulse); }
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
    void AttackDestroy()
    {
        rightleftAtk = false;
        upAtk = false;
        downAtk = false;
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
    void DamagedEnd()
    {
        damaged = false;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    void ChangeMaterial(int mode)
    {
        //mode 0 : 기본 / 1 : 하얀
        spriteRenderer.material = mat[mode];
    }
    void Player_Transform()
    {
        if (player_transformed) //변신 풀기
        {
            player_transformed = false;

            PLAYER_IDLE = "player_idle";
            PLAYER_RUN = "player_run";
            PLAYER_STOP = "player_stop";
            PLAYER_JUMP = "player_jump";
            PLAYER_FALL = "player_fall";
            PLAYER_LAND = "player_land";
            PLAYER_ATTACK1 = "player_attack1";
            PLAYER_ATTACK2 = "player_attack2";
            PLAYER_ATTACK3 = "player_attack3";
            PLAYER_ATTACK_UP = "player_attack_up";
            PLAYER_JUMP_ATTACK = "player_jump_attack";
            PLAYER_JUMP_ATTACK_UP = "player_jump_attack_up";
            PLAYER_JUMP_ATTACK_DOWN = "player_jump_attack_down";
            PLAYER_ROLL = "player_roll";
            PLAYER_WALLSLIDE = "player_wallslide";
            PLAYER_WALLGRAB = "player_wallgrab";

            if (shockWaveManager != null)
            {
                shockWaveManager.CallShockWave2();
            }
        }
        else //변신 하기
        {
            player_transformed = true;

            PLAYER_IDLE = "player2_idle";
            PLAYER_RUN = "player2_run";
            PLAYER_STOP = "player2_stop";
            PLAYER_JUMP = "player2_jump";
            PLAYER_FALL = "player2_fall";
            PLAYER_LAND = "player2_land";
            PLAYER_ATTACK1 = "player2_attack1";
            PLAYER_ATTACK2 = "player2_attack2";
            PLAYER_ATTACK3 = "player2_attack3";
            PLAYER_ATTACK_UP = "player2_attack_up";
            PLAYER_JUMP_ATTACK = "player2_jump_attack";
            PLAYER_JUMP_ATTACK_UP = "player2_jump_attack_up";
            PLAYER_JUMP_ATTACK_DOWN = "player2_jump_attack_down";
            PLAYER_ROLL = "player2_roll";
            PLAYER_WALLSLIDE = "player2_wallslide";
            PLAYER_WALLGRAB = "player2_wallgrab";

            if(shockWaveManager != null)
            {
                shockWaveManager.CallShockWave();
            }
        }
    }

    IEnumerator TransformCool()
    {
        istransfromCool = true;
        yield return new WaitForSeconds(transform_time);
        istransfromCool = false;
    }
    IEnumerator Player2_Dash()
    {
        isdashing = true;
        yield return new WaitForSeconds(0.2f);
        isdashing = false;
    }
}
