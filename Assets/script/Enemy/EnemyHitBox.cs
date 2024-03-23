using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    float rage;
    public GameObject Enemy;
    public bool Hit_left;
    public bool Hit_right;
    //iswall = wall.GetComponent<iswall>().wallreach;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "sword")
        {
           
            rage = Enemy.GetComponent<EnemyAi>().rage;
            if(rage < 0)
            {
                Hit_right = true;
                Invoke("INVOKE_Hit_right", 0.2f);
            }
            else if(rage > 0)
            {
                Hit_left = true;
                Invoke("INVOKE_Hit_left", 0.2f);
            }
            //¾Æ¾æ
            
        }
    }
    void INVOKE_Hit_right()
    {
        Hit_right = false;
    }
    void INVOKE_Hit_left()
    {
        Hit_left = false;
    }

}
