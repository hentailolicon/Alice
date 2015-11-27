using UnityEngine;
using System.Collections;

public class Enemy2 : AI 
{
    // Use this for initialization
    new void Start()
    {
        base.Start();       
    }

    // Update is called once per frame
    new void Update()
    {
        if (Time.time - aiThankLastTime >= 3.0f)
        {
            aiThankLastTime = Time.time;
            target = player.position;
            target = GameManager.instance.GetDirectionForce(transform.position, target, speed);
            SetState(THINK);
        }
    }
    void FixedUpdate()
    {
        if (state == THINK)
        {
            RunTowards();
        }
        else if (state == SlOWDOWN)
        {
            SlowDown();
        }
    }
}
