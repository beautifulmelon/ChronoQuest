using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cam_Move : MonoBehaviour
{
    public GameObject player;
    public int cammode =0;
    public float smoothspeed = 2;
    Vector3 campos;

    void LateUpdate()
    {
        if(cammode == 0) { setcampos1(); }
        else if(cammode == 1) {  setcampos2(); }
    }

    void setcampos1()
    {
        campos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        transform.position = campos;
    }
    void setcampos2()
    {
        campos = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, campos, smoothspeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
