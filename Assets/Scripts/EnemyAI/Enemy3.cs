using UnityEngine;
using System.Collections;

public class Enemy3 : AI 
{
    private Animator anim;
    // Use this for initialization
    public new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public new void Update()
    {
        if (Time.time - thankTime >= 3.0f)
        {
            thankTime = Time.time;
            target = GameManager.instance.GetVelocity(transform.position, player.position, moveSpeed) * 1.2f;
            SetState(CHASE);
        }
    }
    void FixedUpdate()
    {
        switch (state)
        {
            case CHASE:
                collideDamage = 15f;
                anim.SetInteger("state", 2);
                RunTowards();
                if (Time.time - thankTime >= 1f)
                {
                    SetState(SlOWDOWN);
                }
                break;
            case SlOWDOWN:
                SlowDown();
                break;
            case NORMAL:
                collideDamage = 5f;
                GetComponent<Rigidbody2D>().Sleep();
                if (Time.time - thankTime >= 2.5f)
                {
                    anim.SetInteger("state", 1);
                }
                else
                {
                    anim.SetFloat("positionSub", player.position.x - transform.position.x);
                    anim.SetInteger("state", 0);
                }
                break;
        }
    }
}
