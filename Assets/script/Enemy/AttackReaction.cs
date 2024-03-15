using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AttackReaction : MonoBehaviour
{
    public GameObject EnemyArm;
    public Transform EnemyParent;
    public float ReationTime = 1f;
    public float actionTime = 0.5f;
    public bool Reaction = false;
    public bool EnemyAttack = false;
    GameObject attackManager;

    void Start()
    {
        
    }

    void Update()
    {
        if(Reaction == true)
        {
            EnemyAttack = true;

            ReationTime = ReationTime - Time.deltaTime;
            if (ReationTime < 0f)
            {
                attackManager = Instantiate(EnemyArm, EnemyParent);
                ReationTime = 1f;
                Invoke("EAttack", 0.5f);

                Destroy(attackManager, 0.5f);
            }
        }

 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            Reaction = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("Rtion", ReationTime) ;
            Invoke("EAttack", 1.5f);
            //ReationTime = 1f;

        }
    }
    void Rtion()
    {
        Reaction = false;   
    }
    void EAttack()
    {
        EnemyAttack = false;
    }
}
