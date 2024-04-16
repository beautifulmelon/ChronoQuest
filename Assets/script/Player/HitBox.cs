using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyAttack" && !Player.instance.rightleftAtk && !Player.instance.upAtk && !Player.instance.downAtk && !Player.instance.damaged)
        {
            Vector2 heatdirection;
            int xdirection;
            int ydirection = 0;

            if(transform.position.x < other.gameObject.transform.position.x)
            {
                xdirection = -1;
            }
            else
            {
                xdirection= 1;
            }
            if (!Player.instance.isGround)
            {
                if(transform.position.y > other.gameObject.transform.position.y)
                {
                    ydirection = 1;
                }
            }
            heatdirection = new Vector2(xdirection, ydirection);
            Player.instance.Knock_Back(heatdirection);
            Player.instance.Damaged(other.gameObject.GetComponent<EnemyAttack>().attackDamage);
            Cam_Move.instance.DamagedEffect();
        }
    }
}
