using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public float EnemySpeed;
    public GameObject EnemyMovement;
    public Transform Playerpos;
    int nextMove = 0;
    bool detect = false;
    float rage = 0f;
    // Start is called before the first frame update
    private void Awake()
    {
        Invoke("Think", 1f);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        detect = EnemyMovement.GetComponent<EnemyMovement>().Movement;
        if(detect ==false )
        {
            transform.Translate(EnemySpeed * nextMove * Time.deltaTime, 0, 0);
        }

        else if (detect == true)
        {
            rage = Playerpos.position.x - transform.position.x;
            if(rage < 0f)
            {
                transform.Translate(-EnemySpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector2(-1, 1);
            }
            else if(rage > 0f)
            {
                transform.Translate(EnemySpeed * Time.deltaTime, 0, 0);
                transform.localScale = new Vector2(1, 1);
            }
        }
        
    }
    void Think()
    {
        nextMove = Random.Range(-1, 2);//(ÃÖ¼Ú°ª, ÃÖ´ñ°ª +1) ÃÖ´ñ°ªÀº ¾È³ª¿È(¿Ã¸²ÇØ¼­±×·±µí)
        Invoke("Think", 1f);
    }
}
