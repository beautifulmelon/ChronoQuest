using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_camfollow : MonoBehaviour
{
    public float x_follow = 0.7f;
    public float y_follow = 0.7f;
    private float campos_x;
    private float campos_y;
    float xpos;
    float ypos;
    private void Start()
    {
        campos_x = Camera.main.transform.position.x;
        campos_y = Camera.main.transform.position.y;
    }
    void LateUpdate()
    {
        xpos = campos_x - Camera.main.transform.position.x;
        ypos = campos_y - Camera.main.transform.position.y;

        transform.position = new Vector2 (transform.position.x - xpos * x_follow, transform.position.y - ypos * y_follow);

        campos_x = Camera.main.transform.position.x;
        campos_y = Camera.main.transform.position.y;
    }
}
