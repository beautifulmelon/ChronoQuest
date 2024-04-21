using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Mapinfo : MonoBehaviour
{
    public float camsize;
    public float[] camends = new float[4];
    public GameObject[] blacks = new GameObject[4];
    public int blackon;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Cam_Move.instance.cammapmove = false;
            Cam_Move.instance.targetsize = camsize;
            Cam_Move.instance.camends = camends;
            for( int i = 0; i < blacks.Length; i++)
            {
                if(i != blackon)
                    blacks[i].SetActive(false);
            }
            blacks[blackon].SetActive(true);
        }
    }
}
