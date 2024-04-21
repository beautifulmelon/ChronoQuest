using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Cam_Info : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            Cam_Move.instance.targeton = true;
            Cam_Move.instance.targetpos = collision.transform.position;
            Debug.Log("Asdf");

            Cam_Move.instance.targetsize = 3.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            Cam_Move.instance.targeton = false;

            Cam_Move.instance.targetsize = 5.0f;
        }
    }

    
}
