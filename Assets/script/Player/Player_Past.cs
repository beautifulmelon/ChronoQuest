using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Past : MonoBehaviour
{
    public Transform Player;
    private Vector3[] pos = new Vector3[360];
    int i = 0;
    int x = 0;
    private void Start()
    {
        for (int j = 0; j < 360; j++)
        {
            pos[j] = Player.transform.position;
        }
    }
    private void Update()
    {
        //pastTime = pastTime + Time.deltaTime;
        
        

        pos[i] = Player.transform.position;//매 프레임 저장 위치 저장
        i++;
        

        
        if(i >= 360)
        {
            i = 0;
        }
        if(i > 200 ) 
        {
            x = i - 200;
            transform.position = pos[x];
        }
        else
        {
            x = (i + 160) % 360;
            transform.position = pos[x];
        }
        if (Input.GetKey(KeyCode.A))
        {
            Player.transform.position = pos[x];
        }
      

        
    }
}
