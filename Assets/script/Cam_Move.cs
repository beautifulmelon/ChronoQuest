using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cam_Move : MonoBehaviour
{
    public int cammode =0;
    public float smoothspeed = 10;
    Vector3 campos;
    public float shakeTime = 0;
    public float shakeAmount = 1;
    private bool isshaking;

    public float camsize;
    public bool targeton;
    public Vector3 targetpos;
    public float targetsize = 5;
    public float[] camends = new float[3];
    public Vector3 startpos;

    public bool cammapmove;

    public static Cam_Move instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void LateUpdate()
    {
        //if (targeton) { campos = Vector3.Lerp(transform.position, new Vector3((Player.instance.parent.position.x + targetpos.x)/2.0f, (Player.instance.parent.position.y + targetpos.y)/2.0f, -10), smoothspeed * Time.deltaTime); }
        //else { campos = Vector3.Lerp(transform.position, new Vector3(Player.instance.parent.position.x, Player.instance.parent.position.y, -10), smoothspeed * Time.deltaTime); }
        campos = Vector3.Lerp(transform.position, new Vector3(Player.instance.parent.position.x, Player.instance.parent.position.y, -10), smoothspeed * Time.deltaTime);
        if (cammapmove)
        {
            if (campos.x < camends[0])
            {
                campos.x += camends[0] * Time.deltaTime;
            }
            else if (campos.x > camends[1])
            {
                campos.x -= camends[1] * Time.deltaTime;
            }
            else if (campos.y < camends[2])
            {
                campos.y += camends[2] * Time.deltaTime;
            }
            else if (campos.y > camends[3])
            {
                campos.y -= camends[3] * Time.deltaTime;
            }
            else { cammapmove = false; }
        }
        else
        {
            if (campos.x < camends[0])
            {
                campos.x = camends[0];
            }
            else if (campos.x > camends[1])
            {
                campos.x = camends[1];
            }

            if (campos.y < camends[2])
            {
                campos.y = camends[2];
            }
            else if (campos.y > camends[3])
            {
                campos.y = camends[3];
            }
        }

        transform.position = campos;

        CamZoom(targetsize);
    }
    private void FixedUpdate()
    {
        if (shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;
            ShakeCam();
            isshaking = true;
        }
    }

    public void DamagedEffect() 
    {
        shakeTime = 0.1f;
        shakeAmount = 0.2f;
        CancelInvoke("StopShaking");
        Invoke("StopShaking", shakeTime);
    }
    public void HardAtkEffect()
    {
        shakeTime = 0.1f;
        shakeAmount = 0.4f;
        CancelInvoke("StopShaking");
        Invoke("StopShaking", shakeTime);
    }
    void ShakeCam()
    {
        Vector3 campos = new Vector3(transform.position.x, transform.position.y, -10);
        Camera.main.transform.position =  campos + (Vector3)UnityEngine.Random.insideUnitCircle * shakeAmount;
    }
    void StopShaking()
    {
        isshaking = false;
    }

    public void SetstartCampos()
    {
        campos = startpos;
    }
    public void CamZoom(float targetsize)
    {
        float camsize = Camera.main.orthographicSize - targetsize;
        if (camsize < 0 && Camera.main.orthographicSize < targetsize)
        {
            Camera.main.orthographicSize -= camsize * Time.deltaTime;
        }
        else if(camsize >= 0 && Camera.main.orthographicSize > targetsize)
        {
            Camera.main.orthographicSize -= camsize * Time.deltaTime;
        }
    }
}
