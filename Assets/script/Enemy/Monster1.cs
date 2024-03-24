using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : EnemyAi
{
    // Start is called before the first frame update
    private void Awake()
    {
        first();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyFrame();

    }
}
