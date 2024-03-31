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
        if(cammode == 0) { setcampos1(); }
        else if(cammode == 1) {  setcampos2(); }
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

    void setcampos1() //캐릭터 위치에 카메라 고정
    {
        campos = new Vector3(Player.instance.parent.position.x, Player.instance.parent.position.y, -10);
        transform.position = campos;
    }
    void setcampos2() //lerp로 캐릭터 쫓아가기
    {
        campos = new Vector3(Player.instance.parent.position.x, Player.instance.parent.position.y, -10);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, campos, smoothspeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
    //플레이어 데미지
    public void DamagedEffect() 
    {
        shakeTime = 0.1f;
        shakeAmount = 0.2f;
        CancelInvoke("StopShaking");
        Invoke("StopShaking", shakeTime);
    }
    //플레이어 강공격
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
}
