using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2 : EnemyAi
{
    // Start is called before the first frame update
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("Think", 1f);
    }

    // Update is called once per frame
    void Update()
    {

        EnemyMove();
        Hit();

    }
}
