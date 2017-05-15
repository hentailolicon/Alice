using UnityEngine;
using System.Collections;

public class Enemy6 : AI {

    private Animator anim;
    private float range;
    // Use this for initialization
    public new void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
        Bloodtear attr = bloodtear.GetComponent<Bloodtear>();
        tearSpeed = attr.speed;
        attr.range = 4f;
        range = Mathf.Pow(attr.range, 2);
    }

    void FixedUpdate()
    {
        if (Vector2.SqrMagnitude(transform.position - player.position) < range)
        {
            anim.SetBool("isAttact", true);
            if (Time.time - thinkTime >= 2f)
            {
                thinkTime = Time.time;
                Shoot(tearSpeed);
            }
        }
        else
        {
            anim.SetBool("isAttact", false);
        }
    }
}
