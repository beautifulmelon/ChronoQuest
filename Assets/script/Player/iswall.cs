using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iswall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)//자식 혼자만 적용
    {
        if (collision.CompareTag("Ground"))
        {
            Player.instance.iswall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            Player.instance.iswall = false;
        }
    }
}
