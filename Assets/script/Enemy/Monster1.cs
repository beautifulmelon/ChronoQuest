using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : EnemyAi
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool dead;
    public Material[] mat = new Material[2];
    float timer = 3;
    private void Awake()
    {
        first();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        EnemyFrame();
        if(nextMove != 0)
        {
            animator.SetBool("walk", true);
        }
        else { animator.SetBool("walk", false); }
        
        if(hp <=0 && !dead)
        {
            EnemyDeath();
            dead = true;
        }
        if (dead)
        {
            timer -= Time.deltaTime;
            if(timer < 1) { spriteRenderer.color = new Color(1, 1, 1, timer); }
        }
    }
    void EnemyDeath()
    {
        animator.SetBool("dead", true);
        Destroy(this.gameObject, 3f);
    }
}
