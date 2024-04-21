using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AttackReaction : MonoBehaviour
{
    public GameObject EnemyArm;
    public Animator animator;
    public Transform EnemyParent;
    public float ReationTime = 1f;
    public float actionTime = 0.5f;
    public bool Reaction = false;
    public bool EnemyAttack = false;
    GameObject attackManager;
    bool isAt = false;
    bool attacking;
    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
    }
    void Update()
    {
 
        if(Reaction == true && isAt == true && !attacking)
        {
            Reaction = false;
            Invoke("reaction", 1f);//선딜
            animator.SetBool("attackready", true);
            attacking = true;
        }
        if(Player.instance.player_hp<=0) { CancelInvoke("reaction"); }

    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            if(EnemyAttack == false)
            {
                Reaction = true;
                EnemyAttack = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAt = true;
            EnemyAttack = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            isAt=false;
        }

    }

    void reaction()
    {
        animator.SetBool("attack", true);
        animator.SetBool("attackready", false);
        attackManager = Instantiate(EnemyArm, EnemyParent);
        Destroy(attackManager, 0.1f);//공격판정 시간
        Invoke("EAttack", 0.5f);
        Invoke("reac", 0.5f);

    }
    void EAttack()
    {
        EnemyAttack = false;
    }
    void reac()
    {
        Reaction = true;
        attacking = false;
        animator.SetBool("attack", false);
    }
}
