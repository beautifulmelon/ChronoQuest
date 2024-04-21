using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2AI : MonoBehaviour
{
    public int accelAmount = 10;
    public float speed = 5f;
    private int nextMove;
    private bool attackready = true;
    private bool playerin;
    bool dead;
    public Transform attackpos;
    public GameObject energyball;
    private Animator animator;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rigid.gravityScale = 0;
        InvokeRepeating("Think", 2, 3);
    }

    void Update()
    {
        Enemy2Move();
        if(attackready &&!dead && playerin)
        {
            StartCoroutine(Attack());
        }
    }
    void Enemy2Move()
    {
        float targetSpeed = speed;
        Vector2 direction;
        if(nextMove == 1) {  direction = new Vector2(-1,1).normalized; transform.localScale = new Vector2(1, 1); }
        else if(nextMove == 2) { direction = Vector2.up;}
        else if(nextMove == 3) {  direction = new Vector2(1, 1).normalized; transform.localScale = new Vector2(-1, 1); }
        else if(nextMove == 4) {  direction = Vector2.left; transform.localScale = new Vector2(1, 1); }
        else if(nextMove == 5) {  direction = Vector2.zero;}
        else if(nextMove == 6) {  direction = Vector2.right; transform.localScale = new Vector2(-1, 1); }
        else if (nextMove == 7) {  direction = new Vector2(-1, -1).normalized; transform.localScale = new Vector2(1, 1); }
        else if(nextMove == 8) {  direction = Vector2.down;}
        else if(nextMove == 9) {  direction = new Vector2(1, -1).normalized; transform.localScale = new Vector2(-1, 1); }
        else
        {
            direction = Vector2.zero;
        }

        float accelRate;
        targetSpeed = Mathf.Lerp(Vector2.Dot(rigid.velocity, direction), targetSpeed, 1);
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelAmount : accelAmount;
        float speedDif = targetSpeed - Vector2.Dot(rigid.velocity, direction);
        float movement = speedDif * accelRate;
        if(direction.magnitude == 0) { rigid.AddForce(-rigid.velocity, ForceMode2D.Force); }
        else { rigid.AddForce(movement * direction, ForceMode2D.Force); }
    }
    public void Think()
    {
        nextMove = Random.Range(1, 10);
        Invoke("Stopmove", 0.2f);
    }
    void Stopmove()
    {
        nextMove = 0;
    }
    IEnumerator Attack()
    {
        if(Player.instance.transform.position.x < transform.position.x) { transform.localScale = new Vector2(1, 1); }
        else { transform.localScale = new Vector2(-1, 1); }
        animator.SetBool("attack", true);
        Instantiate(energyball, attackpos.position, attackpos.rotation);
        CancelInvoke("Think");
        Invoke("Attackend", 0.1f);
        Stopmove();
        attackready = false;
        yield return new WaitForSeconds(3);
        InvokeRepeating("Think", 2, 3);
        attackready = true;
    }
    void Attackend()
    {
        animator.SetBool("attack", false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerin = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerin = false;
        }
    }
}
