using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    float rage;
    public GameObject Enemy;
    public bool Hit_left;
    public bool Hit_right;
    SpriteRenderer spriteRenderer;
    public Material[] mat = new Material[2];
    public GameObject spark;
    //iswall = wall.GetComponent<iswall>().wallreach;
    private void Start()
    {
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
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
            Enemy.GetComponent<EnemyAi>().hp = Enemy.GetComponent<EnemyAi>().hp - 1;
            StartCoroutine(ChangewhiteMat());
            if (Player.instance.rightleftAtk)
            {
                if(Hit_right) { Instantiate(spark, transform.position, Quaternion.Euler(0, 90, -90)); }
                else { Instantiate(spark, transform.position, Quaternion.Euler(180, 90, -90)); }
            }
            else if (Player.instance.upAtk) { Instantiate(spark, transform.position, Quaternion.Euler(-90, 90, -90)); }
            else if (Player.instance.downAtk) { Instantiate(spark, transform.position, Quaternion.Euler(90, 90, -90));}
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

    void ChangeMaterial(int mode)
    {
        //mode 0 : ±âº» / 1 : ÇÏ¾á
        spriteRenderer.material = mat[mode];
    }
    IEnumerator ChangewhiteMat()
    {
        ChangeMaterial(1);
        yield return new WaitForSeconds(0.1f);
        ChangeMaterial(0);
    }
}
