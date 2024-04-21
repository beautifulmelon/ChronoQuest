using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Energyball : MonoBehaviour
{
    public float speed = 3;
    public int damage = 3;
    private Vector2 direction;
    Animator animator;
    Rigidbody2D rigid;
    private bool moving;
    public GameObject ballend;
    private void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Energyball_move") == true && !moving)
        {
            Vector2 v2 = Player.instance.transform.position - transform.position;
            direction = v2.normalized;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg - 180);
            moving = true;
            rigid.velocity = direction * speed;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.instance.Damaged(damage);
            Cam_Move.instance.DamagedEffect();
            Instantiate(ballend, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Instantiate(ballend, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("sword"))
        {
            Player.instance.Parrying();
            Instantiate(ballend, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
