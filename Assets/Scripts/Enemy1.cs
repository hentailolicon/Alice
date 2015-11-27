using UnityEngine;
using System.Collections;

public class Enemy1 : AI
{
    // Use this for initialization
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }
    void FixedUpdate()
    {
        Track();
    }
}
