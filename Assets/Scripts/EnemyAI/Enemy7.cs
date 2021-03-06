﻿using UnityEngine;
using System.Collections;

public class Enemy7 : AI 
{
    private Animator anim;

    public new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    public new void Update()
    {
        if (Time.time - thinkTime>= 1.5f)
        {
            thinkTime = Time.time;
            target = player.position;
           // float distance = Mathf.Pow(transform.position.x - player.position.x, 2) + Mathf.Pow(transform.position.y - player.position.y, 2);
            if (Random.Range(0,100) < 20)
            {
                GetComponent<Collider2D>().isTrigger = true;
                SetState(ATTACK);
            }
            else
            {
                GetComponent<Collider2D>().isTrigger = false;
                walkTime = Time.time;
                RandomWalk(0.5f, 1f, 2f);
            }
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case ATTACK:
                MoveTo();
                anim.SetInteger("state", 2);
                if ((Mathf.Abs(transform.position.x - target.x) < 0.1f) && (Mathf.Abs(transform.position.y - target.y) < 0.1f))
                {
                    SetState(NORMAL);
                }
                if (Time.time - thinkTime>= 0.5f)
                {
                    SetState(NORMAL);
                }
                break;
            case WALK:
                anim.SetInteger("state", 1);
                RandomWalk(0.5f, 1f, 2f);
                break;
            case SlOWDOWN:
                SlowDown();
                break;
            case NORMAL:
                GetComponent<Collider2D>().isTrigger = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                anim.SetInteger("state", 0);
                break;
        }
    }

    public new void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Player")
        {
            SetState(SlOWDOWN);
        }
    }
}
