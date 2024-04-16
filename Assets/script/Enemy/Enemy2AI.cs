using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2AI : MonoBehaviour
{
    public int accelAmount = 10;
    public float speed = 5f;
    private int nextMove;
    Rigidbody2D rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Enemy2Move();
    }
    void Enemy2Move()
    {
        float targetSpeed = speed;
        Vector2 direction;
        if(nextMove == 1) {  direction = new Vector2(-1,1).normalized; }
        else if(nextMove == 2) { direction = Vector2.up;}
        else if(nextMove == 3) {  direction = new Vector2(1, 1).normalized; }
        else if(nextMove == 4) {  direction = Vector2.left;}
        else if(nextMove == 5) {  direction = Vector2.zero;}
        else if(nextMove == 6) {  direction = Vector2.right;}
        else if (nextMove == 7) {  direction = new Vector2(-1, -1).normalized; }
        else if(nextMove == 8) {  direction = Vector2.down;}
        else if(nextMove == 9) {  direction = new Vector2(1, -1).normalized; }
        else
        {
            direction = Vector2.zero;
        }

        float accelRate;
        targetSpeed = Mathf.Lerp(rigid.velocity.magnitude, targetSpeed, 1);
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accelAmount : accelAmount;
        float speedDif = targetSpeed - rigid.velocity.magnitude;
        float movement = speedDif * accelRate;
        rigid.AddForce(movement * direction, ForceMode2D.Force);
    }
    public void Think()
    {
        nextMove = Random.Range(1, 10);
        Invoke("Think", 1f);
    }
}
