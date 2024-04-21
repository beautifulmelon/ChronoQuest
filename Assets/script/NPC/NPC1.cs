using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC1 : MonoBehaviour
{
    private bool playerin;
    private bool dialog;
    private int line;
    public Animator animator;

    private void Start()
    {
        animator = transform.parent.GetComponent<Animator>();
        InvokeRepeating("AnimationPlay", 2f, 2f);
    }
    private void Update()
    {
        if (playerin && !dialog)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Player.instance.unable_control = true;
                Player.instance.ChangeAnimationState("player_idle");
                dialog = true;
                line = 4;
                CancelInvoke("AnimationPlay");
                animator.Play("npc1_turnhead");
            }
        }
        else if(dialog)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                line -= 1;
                Debug.Log(line);
                if(line == 0)
                {
                    animator.Play("npc1_turnhead 0");
                    InvokeRepeating("AnimationPlay", 2f, 2f);
                }
            }
        }
        if(line == 0)
        {
            Player.instance.unable_control = false;
            dialog = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            playerin = true;
            Player.instance.caninteraction = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            playerin = false;
            Player.instance.caninteraction = false;
        }
    }
    void AnimationPlay()
    {
        int state = Random.Range(0, 3);
        Debug.Log(state);
        if(state == 0)
        {
            animator.Play("npc1_blink");
        }
        else if(state == 1)
        {
            animator.Play("npc1_paper");
        }
        else if(state == 2)
        {
            animator.Play("npc1_idle");
        }
    }
}
