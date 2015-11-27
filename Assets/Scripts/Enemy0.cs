using UnityEngine;
using System.Collections;

public class Enemy0 : AI 
{
	// Use this for initialization
	new void Start () 
    {
        base.Start();
	}
	
	// Update is called once per frame
	new void Update () 
    {
	    base.Update();
	}
    void FixedUpdate()
    {
        if (state == THINK)
        {
            MoveTo();
            if ((Mathf.Abs(transform.position.x - target.x) < 0.01f) && (Mathf.Abs(transform.position.y - target.y) < 0.01f))
            {
                SetState(NORMAL);
            }
        }
        else if (state == SlOWDOWN)
        {
            SlowDown();
        }
    }
}
