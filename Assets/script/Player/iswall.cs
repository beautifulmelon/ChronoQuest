using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iswall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)//�ڽ� ȥ�ڸ� ����
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
