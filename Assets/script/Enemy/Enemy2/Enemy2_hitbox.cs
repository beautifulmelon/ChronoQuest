using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2_hitbox : MonoBehaviour
{
    public GameObject Enemy;
    public int enemyhp = 5;
    public float hit_rage = 1f;
    Rigidbody2D rigid;

    SpriteRenderer spriteRenderer;
    public Material[] mat = new Material[2];
    public GameObject spark;
    //iswall = wall.GetComponent<iswall>().wallreach;
    private void Start()
    {
        spriteRenderer = Enemy.GetComponent<SpriteRenderer>();
        rigid = Enemy.GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "sword")
        {
            enemyhp -= Player.instance.player_atk;
            StartCoroutine(ChangewhiteMat());
            if (Player.instance.rightleftAtk)
            {
                if (Enemy.transform.position.x > Player.instance.transform.position.x) 
                { 
                    Instantiate(spark, transform.position, Quaternion.Euler(0, 90, -90));
                    rigid.AddForce(Vector2.right * hit_rage, ForceMode2D.Impulse);
                }
                else 
                { 
                    Instantiate(spark, transform.position, Quaternion.Euler(180, 90, -90));
                    rigid.AddForce(Vector2.left * hit_rage, ForceMode2D.Impulse);
                }
            }
            else if (Player.instance.upAtk) 
            { 
                Instantiate(spark, transform.position, Quaternion.Euler(-90, 90, -90));
                rigid.AddForce(Vector2.up * hit_rage, ForceMode2D.Impulse);
            }
            else if (Player.instance.downAtk) 
            { 
                Instantiate(spark, transform.position, Quaternion.Euler(90, 90, -90));
                rigid.AddForce(Vector2.down * hit_rage, ForceMode2D.Impulse);
            }
        }
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
