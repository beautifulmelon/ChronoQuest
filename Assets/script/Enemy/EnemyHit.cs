using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
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
            }
            else if(rage > 0)
            {
                Hit_left = true;
            }
            //¾Æ¾æ
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "sword")
        {
                Hit_right = false;
                Hit_left = false;
        }
    }
}
