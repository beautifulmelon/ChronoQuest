using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGround : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Player.instance.isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Player.instance.isGround = false;
        }
    }
}
