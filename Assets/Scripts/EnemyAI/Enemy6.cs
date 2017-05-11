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

        float distance = Mathf.Pow(transform.position.x - player.position.x, 2) + Mathf.Pow(transform.position.y - player.position.y, 2);
        if (distance < range)
        {
            anim.SetBool("isAttact", true);
            if (Time.time - thankTime >= 2f)
            {
                thankTime = Time.time;
                Shoot(tearSpeed);
            }
        }
        else
        {
            anim.SetBool("isAttact", false);
        }
    }
}
