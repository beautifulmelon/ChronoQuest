using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iswall : MonoBehaviour
{
    public bool wallreach = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)//자식 혼자만 적용
    {
        if (collision.gameObject.tag == "Ground")
        {
            wallreach = true;
            Debug.Log("벽 닿음");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            wallreach = false;

        }
    }
}
