using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isGround : MonoBehaviour
{
    public GameObject Player;
    public bool Groundreach = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    //        isGround = Ground.GetComponent<isGround>().Groundreach;
    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Groundreach = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Groundreach = false;
        }
    }
}
