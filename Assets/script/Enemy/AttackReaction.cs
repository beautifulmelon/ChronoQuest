using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AttackReaction : MonoBehaviour
{
    public GameObject EnemyAttack;
    public Transform EnemyParent;
    public float ReationTime = 1f;
    public float actionTime = 0.5f;
    bool action = false;
    bool Reaction = false;
    GameObject attackManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Reaction == true)
        {
            ReationTime = ReationTime - Time.deltaTime;
            if (ReationTime < 0f)
            {
                attackManager = Instantiate(EnemyAttack, EnemyParent);
                Reaction = false;
                ReationTime = 0f;
                action = true;
            }
        }

        if(action == true)
        {
            actionTime = actionTime - Time.deltaTime;
            if(actionTime < 0f)
            {
                Destroy(attackManager);
                Reaction = false;
                action = false;
                ReationTime = 1f;
                actionTime = 0.5f;
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
}
